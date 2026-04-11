using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebServer.Data;
using WebServer.Models.DTO;
using WebServer.Models.Entities;
using WebServer.Services;

namespace WebServer.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDBContext dbContext;

        public UserController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        [CheckUserStatus]
        public async Task<ActionResult> MainPage()
        {
            var users = await dbContext.Users
                .OrderByDescending(u => u.LastLoginTime)
                .ToListAsync();

            return View(users);
        }

        [Authorize]
        [HttpPost]
        [CheckUserStatus]
        public async Task<ActionResult> MainPage(List<int> ids)
        {
            var users = await dbContext.Users
                .OrderByDescending(u => u.LastLoginTime)
                .ToListAsync();

            return View(users);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(UserDto _user)
        {
            if (!ModelState.IsValid)
            {
                return View(_user);
            }
            var token = Guid.NewGuid().ToString();

            var user = new User
            {
                Email = _user.Email,
                Password = _user.Password,
                Name = _user.Name,
                Surname = _user.Surname,
                Status = UserStatus.Unverified,
                LastLoginTime = DateTime.UtcNow,
                EmailConfirmationToken = token
            };

            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "User",
                new { email = user.Email, token = token },
                Request.Scheme
            );

            var emailService = new EmailService();

            await Task.Run(() => emailService.SendEmail(
                user.Email,
                "Confirm your email\n",
                $"Click here: <a href='{confirmationLink}'>Confirm</a>"
            ));

            if (await dbContext.Users.AnyAsync(u => u.Email == _user.Email))
            {
                ModelState.AddModelError("Email", "User with this email already exists");
                return View(_user);
            }
            await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();

            var claims = new List<Claim> // Create claims for the user
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync("Cookies",
                new ClaimsPrincipal(identity));

            return RedirectToAction("MainPage");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.EmailConfirmationToken != token)
            {
                return Content("Invalid confirmation link");
            }

            if (user.Status != UserStatus.Blocked)
            {
                user.Status = UserStatus.Active;
            }
            else
            {
                return Content("You are blocked");
            }

            user.EmailConfirmationToken = null;

            await dbContext.SaveChangesAsync();

            return Content("Email confirmed successfully!");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model, string? returnUrl)
        {

            if (!ModelState.IsValid)
                return View(model);

            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            if (user.Status == UserStatus.Blocked)
            {
                ModelState.AddModelError("", "You are blocked");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync("Cookies",
                new ClaimsPrincipal(identity));


            // update last login time
            user.LastLoginTime = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("MainPage");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
        {
            var users = await dbContext.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();

            dbContext.Users.RemoveRange(users);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> BlockSelected([FromBody] List<int> ids)
        {
            var users = await dbContext.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();

            foreach (var user in users)
                user.Status = UserStatus.Blocked;

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UnblockSelected([FromBody] List<int> ids)
        {
            var users = await dbContext.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();

            foreach (var user in users)
                if (user.Status == UserStatus.Blocked)
                    user.Status = UserStatus.Active;

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUnverified()
        {
            var users = await dbContext.Users
                .Where(u => u.Status == UserStatus.Unverified)
                .ToListAsync();

            dbContext.Users.RemoveRange(users);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}