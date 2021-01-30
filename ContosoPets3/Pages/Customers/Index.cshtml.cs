using ContosoPets3.Data;
using ContosoPets3.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoPets3.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ContosoPetsContext _context;

        public IndexModel(ContosoPetsContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get; set; }

        public async Task OnGetAsync()
        {
            int condition = 999;
            Customer = await _context.Customers
                .FromSqlInterpolated($"SELECT * FROM Customers WHERE Id < {condition}")
                .ToListAsync();
        }
    }
}