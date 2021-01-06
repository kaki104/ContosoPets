using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoPets3.Data;
using ContosoPets3.Models;

namespace ContosoPets3.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ContosoPets3.Data.ContosoPetsContext _context;

        public IndexModel(ContosoPets3.Data.ContosoPetsContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; }

        public async Task OnGetAsync()
        {
            var condition = 999;
            Customer = await _context.Customers
                .FromSqlInterpolated($"SELECT * FROM Customers WHERE Id < {condition}")
                .ToListAsync();
        }
    }
}