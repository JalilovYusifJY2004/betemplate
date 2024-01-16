using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PraktikaBeTemplate.Areas.ViewModels;
using PraktikaBeTemplate.DAL;
using PraktikaBeTemplate.Model;
using PraktikaBeTemplate.Utilities.Extension;

namespace PraktikaBeTemplate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.ToListAsync();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM teamVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!teamVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "sekil tipi uygun deyil");
                return View();
            }
            if (!teamVM.Photo.ValidateSize(2 * 1024))
            {
                ModelState.AddModelError("Photo", "Sekil olcusu uygun deyil");
                return View();
            }
            string filename = await teamVM.Photo.CreateFile(_env.WebRootPath, "assets", "img");
            Team team = new Team()
            {
                Image = filename,
                Name = teamVM.Name,
                Description = teamVM.Description,
            };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Team existed= await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            UpdateTeamVM teamVM = new UpdateTeamVM
            {
                Image = existed.Image,
                Name = existed.Name,
                Description = existed.Description,

            };
            return View(teamVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateTeamVM teamVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Team existed = await _context.Teams.FirstOrDefaultAsync(t=>t.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            if (existed is not null)
            {
                if (!teamVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "sekil tipi uygun deyil");
                    return View();
                }
                if (!teamVM.Photo.ValidateSize(2*1024))
                {
                    ModelState.AddModelError("Photo", "sekil tipi uygun deyil");
                    return View();
                }
                string newImage = await teamVM.Photo.CreateFile(_env.WebRootPath, "assets", "img");
                existed.Image.Deletefile(_env.WebRootPath, "assets", "img");
                existed.Image = newImage;

             
            }
            existed.Name = teamVM.Name;
            existed.Description = teamVM.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id<=0)
            {
                return BadRequest();
            }
            Team team= await _context.Teams.FirstOrDefaultAsync(t=>t.Id == id);
            if (team is null)
            {
                return NotFound();
            }
            team.Image.Deletefile(_env.WebRootPath, "assets", "img");
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
