using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shivakala.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shivakala.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShivakalaDbContext _context;

        public AccountController(ShivakalaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Please enter both username and password.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            // Search in AppUsers table using PasswordHash
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);

            if (user == null)
            {
                TempData["Error"] = "Invalid username or password.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName ?? user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "Student"),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Mobile", user.Mobile ?? user.Username)
            };

            if (user.StudentId.HasValue)
            {
                claims.Add(new Claim("StudentId", user.StudentId.Value.ToString()));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                properties);

            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Welcome back, " + user.FullName + "!";
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "You have been logged out.";
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
