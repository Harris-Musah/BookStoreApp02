using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApp02.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly BookStoreContext _context;

        public DashboardModel(BookStoreContext context)
        {
            _context = context;
        }

        public int TotalBooks { get; set; }
        public int PendingOrders { get; set; }
        public int NewUsers { get; set; }
        public decimal MonthlyRevenue { get; set; }

        public async Task OnGetAsync()
        {
            // Get total books
            TotalBooks = await _context.Books.CountAsync();

            // For now, set placeholder values since Orders and Users models don't exist yet
            PendingOrders = 0; // Placeholder
            NewUsers = 0; // Placeholder
            MonthlyRevenue = 0.00M; // Placeholder

            // TODO: Implement when Order and User models are added
            // PendingOrders = await _context.Orders.Where(o => o.Status == "Pending").CountAsync();
            // NewUsers = await _context.Users.Where(u => u.CreatedDate >= DateTime.Now.AddDays(-30)).CountAsync();
            // MonthlyRevenue = await _context.Orders.Where(o => o.OrderDate >= DateTime.Now.AddMonths(-1) && o.Status == "Completed").SumAsync(o => o.TotalAmount);
        }
    }
} 