using ContosoPets3.Data;
using ContosoPets3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContosoPets3.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly ContosoPetsContext _context;

        public DetailsModel(ContosoPetsContext context)
        {
            _context = context;
        }

        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
