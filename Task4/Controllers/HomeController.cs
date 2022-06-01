using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Task4.Data;
using Task4.Models;
using Task4.Models.ViewModels;

namespace Task4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            if (user != null && user.Status != (int)UserStatus.Blocked)
            {
                var users = await _db.Users.ToListAsync();
                var model = new UserVM
                {
                    Users = users,
                    CurrentUser = user
                };
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == GetHashString(model.Password));
                
                if (user != null)
                {
                    await Authenticate(model.Email);

                    user.LastLoginTime = DateTime.Now;

                    await _db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Login or password is incorrect");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    _db.Users.Add(new User 
                    { 
                        Name = model.Name,
                        Email = model.Email, 
                        Password = GetHashString(model.Password),
                        Status = (int)UserStatus.Default,
                        RegistrationDate = DateTime.Now
                    });

                    await _db.SaveChangesAsync();

                    await Authenticate(model.Email); 

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        private async Task Authenticate(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserActions(UserVM model, [FromForm] int[] ids)
        {
            if (ids.Length > 0)
            {
                var users = new List<User>();

                foreach (int id in ids)
                {
                    users.Add(await _db.Users.FirstOrDefaultAsync(u => u.Id == id));
                }

                if (model.Block != null)
                {
                    _db.Users.UpdateRange(users
                        .Where(u => ids.Contains(u.Id))
                        .Select(u =>
                        {
                            u.Status = (int)UserStatus.Blocked;
                            return u;
                        })
                    );
                } 
                else if (model.UnBlock != null)
                {
                    _db.Users.UpdateRange(users
                        .Where(u => ids.Contains(u.Id))
                        .Select(u =>
                            {
                                u.Status = (int)UserStatus.Default;
                                return u;
                            })
                    );
                }
                else
                {
                    _db.Users.RemoveRange(users);
                }

                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        private string GetHashString(string input)
        {
            var sb = new StringBuilder();
            byte[] buffer;

            using (var algorithm = SHA256.Create())
            {
                buffer = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            foreach (byte b in buffer)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }

    public enum UserStatus { Default = 1, Blocked = 2 }
}
