using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Constants;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamMemberController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<TeamMember> members = await _context.TeamMembers.ToListAsync();
            return View(members);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamMemberVM member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!member.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{member.Photo.FileName} {Messages.FileTypeMustBeImage}");
                return View();
            }
            if (!member.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", $"{member.Photo.FileName} - {Messages.FileSizeMustBe200KB}");
                return View();
            }
           
            string root = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
            string fileName = await member.Photo.SaveAsync(root);
            TeamMember teamMember = new TeamMember()
            {
                FullName = member.FullName,
                ImagePath = fileName,
                Profession = member.Profession
            };
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            TeamMember member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TeamMember member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            TeamMember result = await _context.TeamMembers.FirstOrDefaultAsync(t => t.Id == member.Id);
            if (result is null)
            {
                TempData["Exists"] = "Bu Member bazada yoxdur";
                return RedirectToAction(nameof(Index));
            }
            result.FullName = member.FullName;
            result.Profession = member.Profession;
            result.ImagePath = member.ImagePath;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            TeamMember teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null) return NotFound();
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", teamMember.ImagePath);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

             _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
