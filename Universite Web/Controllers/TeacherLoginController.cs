using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universite_Web.Data;
using Universite_Web.ViewModel;

namespace Universite_Web.Controllers
{
    public class TeacherLoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public TeacherLoginController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Login(VmTeacherLogin model)
        {
            var yoxla = _context.CustomUser.Where(m => m.Email == model.Email).Select(i => i.IsAdmin).FirstOrDefault();
            if (yoxla == false)
            {
                var yoxla2 = _context.CustomUser.Where(m => m.Email == model.Email).Select(i => i.IsStudent).FirstOrDefault();
                if (yoxla2 == false)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email or password is not valid");
                        return View(model);
                    }
                }

            }

            return View(model);

        }
        public IActionResult Index()
        {
            var salam = _signInManager.IsSignedIn(User);
            var sagol = _userManager.GetUserId(User);

            Users users = new Users()
            {
                CustomUsers = _context.CustomUser.Where(m => m.Id == sagol).ToList()
            };
            return View(users);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [Authorize]
        public async Task<IActionResult> About(string id)
        {
            var user = await _context.CustomUser.Include(m=>m.Subject)
                   .FirstOrDefaultAsync(m => m.Id == id);

            return View(user);
        }
        [Authorize]
        public async Task<IActionResult> Subject(int id)
        {
            var user = await _context.CustomUser.Include(m => m.Subject)
                   .FirstOrDefaultAsync(m => m.SubjectId == id);

            return View(user);
        }
    }
}
