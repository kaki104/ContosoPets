using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

public class Program
{
    // ********* Provide details here ***********
    /// <summary>
    /// Azure Key Vault의 Key값에 해당하는 Url을 입력합니다.
    /// 예) https://kakisamplekey.vault.azure.net/keys/Always-Encrypted-Auto1/219cxxxxxxxxxxxxxxxxxxxxxxxxxxxx﻿
    /// </summary>
    private static readonly string s_akvUrl = "AKV Url을 입력하세요";
    /// <summary>
    /// Azure App 등록에 등록된 ClientId를 입력합니다.
    /// </summary>
    private static readonly string s_clientId = "ClientId값을 입력하세요";
    /// <summary>
    /// Azure App 등록에 등록된 Client의 Secret를 입력합니다.
    /// </summary>
    private static readonly string s_clientSecret = "Secret값을 입력하세요";
    /// <summary>
    /// Database 연결 문자열을 입력합니다.
    /// Column Encryption Setting=enabled 를 사용해야 암호화된 컬럼의 내용을 읽을 수 있습니다.
    /// </summary>

    //암호화 컬럼을 읽을 수 있는 연결 문자열
    //private static readonly string s_connectionString = "Data Source=localhost;Database=ContosoUniversity;Integrated Security=true;TrustServerCertificate=true;Column Encryption Setting=enabled";
    //암호화 컬럼을 읽을 수 없는 연결 문자열
    private static readonly string s_connectionString = "Data Source=localhost;Database=ContosoUniversity;Integrated Security=true;TrustServerCertificate=true";
    // ******************************************

    public static void Main()
    {
        // Initialize AKV provider
        SqlColumnEncryptionAzureKeyVaultProvider akvProvider = new(new LegacyAuthCallbackTokenCredential());

        // Register AKV provider
        SqlConnection.RegisterColumnEncryptionKeyStoreProviders(
            customProviders: new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(
                capacity: 1,
                comparer: StringComparer.OrdinalIgnoreCase)
            {
                {
                    SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, akvProvider
                }
            });
        Console.WriteLine("AKV provider Registered");

        // Create connection to database
        using SqlConnection sqlConnection = new(s_connectionString);
        try
        {
            sqlConnection.Open();
            // Read data from table
            SelectData(sqlConnection);
            Console.WriteLine("Data validated successfully.");
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// 테이블에서 데이터 읽어 오기
    /// </summary>
    /// <param name="sqlConnection"></param>
    private static void SelectData(SqlConnection sqlConnection)
    {
        // Test INPUT parameter on an encrypted parameter
        using SqlCommand sqlCommand = new($"SELECT * FROM Employees", sqlConnection);
        //SqlParameter customerFirstParam = sqlCommand.Parameters.AddWithValue(@"firstName", @"Microsoft");
        //customerFirstParam.Direction = System.Data.ParameterDirection.Input;
        //customerFirstParam.ForceColumnEncryption = true;

        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        ValidateResultSet(sqlDataReader);
    }
    /// <summary>
    /// 읽어온 데이터 출력하기
    /// </summary>
    /// <param name="sqlDataReader"></param>
    private static void ValidateResultSet(SqlDataReader sqlDataReader)
    {
        Console.WriteLine(" * Row available: " + sqlDataReader.HasRows);
        while (sqlDataReader.Read())
        {
            Console.WriteLine($"{sqlDataReader[0]}  {sqlDataReader[1]}  {sqlDataReader[2]}  {sqlDataReader[3]}  {sqlDataReader[4]}");
        }
    }
    /// <summary>
    /// AKV 인증
    /// </summary>
    private class LegacyAuthCallbackTokenCredential : TokenCredential
    {
        private string _authority = "";
        private string _resource = "";
        private string _akvUrl = "";

        [Obsolete]
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return AcquireTokenAsync().GetAwaiter().GetResult();
        }

        [Obsolete]
        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return await AcquireTokenAsync();
        }

        [Obsolete]
        private async Task<AccessToken> AcquireTokenAsync()
        {
            // Added to reduce HttpClient calls.
            // For multi-user support, a better design can be implemented as needed.
            if (_akvUrl != s_akvUrl)
            {
                using (HttpClient httpClient = new())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(s_akvUrl);
                    string? challenge = response?.Headers.WwwAuthenticate.FirstOrDefault()?.ToString();
                    string trimmedChallenge = ValidateChallenge(challenge);
                    string[] pairs = trimmedChallenge.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    if (pairs != null && pairs.Length > 0)
                    {
                        for (int i = 0; i < pairs.Length; i++)
                        {
                            string[]? pair = pairs[i]?.Split('=');

                            if (pair.Length == 2)
                            {
                                string? key = pair[0]?.Trim().Trim(new char[] { '\"' });
                                string? value = pair[1]?.Trim().Trim(new char[] { '\"' });

                                if (!string.IsNullOrEmpty(key))
                                {
                                    if (key.Equals("authorization", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        _authority = value;
                                    }
                                    else if (key.Equals("resource", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        _resource = value;
                                    }
                                }
                            }
                        }
                    }
                }
                _akvUrl = s_akvUrl;
            }

            string strAccessToken = await AzureActiveDirectoryAuthenticationCallback(_authority, _resource);
            DateTime expiryTime = InterceptAccessTokenForExpiry(strAccessToken);
            return new AccessToken(strAccessToken, new DateTimeOffset(expiryTime));
        }
        /// <summary>
        /// 엑세스토큰 유효기간
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        private DateTime InterceptAccessTokenForExpiry(string accessToken)
        {
            if (null == accessToken)
            {
                throw new ArgumentNullException(accessToken);
            }

            JwtSecurityTokenHandler jwtHandler = new();
            string jwtOutput = string.Empty;

            // Check Token Format
            if (!jwtHandler.CanReadToken(accessToken))
            {
                throw new FormatException(accessToken);
            }

            JwtSecurityToken token = jwtHandler.ReadJwtToken(accessToken);

            // Re-serialize the Token Headers to just Key and Values
            string jwtHeader = JsonConvert.SerializeObject(token.Header.Select(h => new { h.Key, h.Value }));
            jwtOutput = $"{{\r\n\"Header\":\r\n{JToken.Parse(jwtHeader)},";

            // Re-serialize the Token Claims to just Type and Values
            string jwtPayload = JsonConvert.SerializeObject(token.Claims.Select(c => new { c.Type, c.Value }));
            jwtOutput += $"\r\n\"Payload\":\r\n{JToken.Parse(jwtPayload)}\r\n}}";

            // Output the whole thing to pretty JSON object formatted.
            string jToken = JToken.Parse(jwtOutput).ToString(Newtonsoft.Json.Formatting.Indented);
            JToken payload = JObject.Parse(jToken).GetValue("Payload");

            return new DateTime(1970, 1, 1).AddSeconds((long)payload[4]["Value"]);
        }

        private static string ValidateChallenge(string challenge)
        {
            string Bearer = "Bearer ";
            if (string.IsNullOrEmpty(challenge))
            {
                throw new ArgumentNullException(nameof(challenge));
            }

            string trimmedChallenge = challenge.Trim();

            return !trimmedChallenge.StartsWith(Bearer)
                ? throw new ArgumentException("Challenge is not Bearer", nameof(challenge))
                : trimmedChallenge[Bearer.Length..];
        }

        /// <summary>
        /// Legacy implementation of Authentication Callback, used by Azure Key Vault provider 1.0.
        /// This can be leveraged to support multi-user authentication support in the same Azure Key Vault Provider.
        /// </summary>
        /// <param name="authority">Authorization URL</param>
        /// <param name="resource">Resource</param>
        /// <returns></returns>
        [Obsolete]
        public static async Task<string> AzureActiveDirectoryAuthenticationCallback(string authority, string resource)
        {
            AuthenticationContext authContext = new(authority);
            ClientCredential clientCred = new(s_clientId, s_clientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);
            return result == null
                ? throw new InvalidOperationException($"Failed to retrieve an access token for {resource}")
                : result.AccessToken;
        }
    }

}