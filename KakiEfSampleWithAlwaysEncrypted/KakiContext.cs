using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KakiEfSampleWithAlwaysEncrypted
{
    ///// <summary>
    ///// DBContext
    ///// </summary>
    //public class KakiContext : DbContext
    //{
    //    private static bool _isInitialized;
    //    private static string _clientId = "";
    //    private static string _clientSecret = "";
    //    private static ClientCredential _clientCredential;
    //    //스콥
    //    //user_impersonation
    //    //테넌트 아이디
    //    //"Instance": "https://login.microsoftonline.com/",

    //    public KakiContext(DbContextOptions<KakiContext> options) : base(options)
    //    {
    //    }
    //    /// <summary>
    //    /// Employees
    //    /// </summary>
    //    public DbSet<Employee> Employees { get; set; }
    //}
}
