using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BookStoreApp02.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly BookStoreContext _context;
        public DetailsModel(BookStoreContext context)
        {
            _context = context;
        }
        public Book Book { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Book = await _context.Books.FindAsync(id);
            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
