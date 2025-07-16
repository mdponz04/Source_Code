using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using BusinessObjects.Models;

namespace OrchidSellerClient.Pages.OrderPages
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.MyStoreDbContext _context;

        public IndexModel(BusinessObjects.MyStoreDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.Account).ToListAsync();
        }
    }
}
