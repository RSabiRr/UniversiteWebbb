using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universite_Web.Data;

namespace Universite_Web.Areas.admin.Controllers
{
    [Area("admin")]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public TeacherController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/Sliders1
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomUser.ToListAsync());
        }

        // GET: admin/Sliders1/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = _context.Specialty.FirstOrDefault();

            var user = await _context.CustomUser.Include(m => m.EducationSection).Include(r => r.Faculty).Include(r => r.Specialty)
                .Where(m => m.FacultyId == specialty.FacultyId)
                   .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }




        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = _context.Specialty.FirstOrDefault();

            var user = await _context.CustomUser.Include(m => m.EducationSection).Include(r => r.Faculty).Include(r => r.Specialty)
                .Where(m => m.FacultyId == specialty.FacultyId)
                   .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.CustomUser.FindAsync(id);
            _context.CustomUser.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
    }
}
