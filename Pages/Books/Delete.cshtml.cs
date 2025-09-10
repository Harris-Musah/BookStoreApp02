using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp02.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly BookStoreContext _context;
        public DeleteModel(BookStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Book Book { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Book = await _context.Books.FindAsync(id);
            if (Book == null)
                return NotFound();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var book = await _context.Books.FindAsync(Book.Id);
            if (book == null)
                return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
