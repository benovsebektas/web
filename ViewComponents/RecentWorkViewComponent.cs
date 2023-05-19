using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.Models;

namespace WebFrontToBack.ViewComponents
{
    public class RecentWorkViewComponent:ViewComponent
    {
        private readonly RecentWorkViewComponent _context;
        public RecentWorkViewComponent(AppContext context)
        {
            _context = context
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<RecentWork> recentWorks = await _context.RecentWorks
                                                 .Where(s => !s.IsDeleted)
                                                 .OrderByDescending(s => s.Id)
                                                 .Take(8)
                                                 .Include(s => s.Category)
                                                 .Include(s => s.ServiceImages)
                                                 .ToListAsync();

            return View(recentWorks);
        }
    }
}


    