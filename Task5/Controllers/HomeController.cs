using Task5.Models;
using Microsoft.AspNetCore.Mvc;
using Task5.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Task5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            if (ModelState.IsValid)
            {
                var userFromDb = _db.Users.FirstOrDefault(u => u.Name == user.Name);

                if (userFromDb != null)
                {
                    return RedirectToAction("Chat", new { name = userFromDb.Name });
                }

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return RedirectToAction("Chat", new { name = user.Name });
            }

            return View();
        }
        
        [HttpGet]
        public IActionResult Chat(string name)
        {
            var user = _db.Users.
                Include(u => u.MessageUsers).
                ThenInclude(x => x.Message).
                FirstOrDefault(u => u.Name == name);

            if (user == null)
            {
                return NotFound();
            }

            return View(new SendVM { User = user });
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendVM model, [FromForm] string[] names)
        {
            names = names.Distinct().ToArray();

            foreach (string name in names)
            {
                var recepient = _db.Users.FirstOrDefault(u => u.Name == name);
                _db.MessageUser.Add(new MessageUser { User = recepient, Message = model.Message });
            }

            _db.Messages.Add(model.Message);
            await _db.SaveChangesAsync();

            return Ok();
        }

        #region API
        [HttpGet]
        public IActionResult Users() => Json(new { data = _db.Users.ToList() });
        #endregion
    }
}
