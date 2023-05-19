using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Controllers
{
    public class RecentWorkController : Controller
    {
        protected readonly AppDbContext _context;

        public RecentWorkController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.dbRecentWorkCount = await _context.RecentWorks.CountAsync();

            return View(await _context.RecentWorks
                .OrderByDescending(s => s.Id)
                .Where(s => !s.IsDeleted)
                .Take(8)
                .Include(s => s.RecentworkImages)
                .ToListAsync());
        }

        public async Task<IActionResult> LoadMore(int skip = 0, int take = 8)
        {

            List<RecentWork> recentWorks = await _context.RecentWorks
                .OrderByDescending(s => s.Id)
                .Where(s => !s.IsDeleted)
                .Skip(skip)
                .Take(take)
                .Include(s => s.RecentWorkImages)
                .ToListAsync();
            return PartialView("_RecentWorkPartialView", recentWorks);
        }
    }
}
    }
}
