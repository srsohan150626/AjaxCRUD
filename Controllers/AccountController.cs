using AdvanceAjaxCRUD.Data;
using AdvanceAjaxCRUD.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace AdvanceAjaxCRUD.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new UserViewModel());
        }
        [HttpPost]
        public IActionResult SignUp(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_context.User.Any(_ => _.Email.ToLower() == viewModel.Email.ToLower()))
                {
                    User newUser = new User
                    {
                        Email = viewModel.Email,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Password = viewModel.Password
                    };
                    _context.User.Add(newUser);
                    _context.SaveChanges();

                    return RedirectToAction("Login", "Account");
                }
            }
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // note : real time we save password with encryption into the database
                // so to check that viewModel.Password also need to encrypt with same algorithm 
                // and then that encrypted password value need compare with database password value
                User user = _context.User.Where(_ => _.Email.ToLower() == viewModel.Email.ToLower() && _.Password == viewModel.Password).FirstOrDefault();
                if (user != null)
                {
                    user.LastLoginTime = DateTime.Now;
                    _context.SaveChanges();
                    var claims = new List<Claim>
                    {
                     new Claim(ClaimTypes.Name, user.Email),
                     new Claim("FirstName",user.FirstName),

                    };
                    var userRoles = _context.UserRole.Join(
                         _context.Roles,
                         ur => ur.RoleId,
                         r => r.Id,
                         (ur, r) => new
                         {
                             ur.RoleId,
                             r.RoleName,
                             ur.UserId
                         }).Where(_ => _.UserId == user.Id).ToList();
                    // SQL query to fetch user roles
                    //var sqlQuery = @"
                    //    SELECT ur.RoleId, r.RoleName, ur.UserId FROM UserRole ur
                    //    JOIN Roles r ON ur.RoleId = r.Id
                    //    WHERE ur.UserId = @userId";
                    //var userRoles = await _context.Query<UserRole>()
                    //   .FromSql(sqlQuery, new SqlParameter("@userId", user.Id))
                    //   .ToListAsync();
                    foreach (var ur in userRoles)
                    {
                        var roleClaim = new Claim(ClaimTypes.Role, ur.RoleName);
                        claims.Add(roleClaim);
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties() { IsPersistent = viewModel.IsPersistant };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("InvalidCredentials", "Either username or password is not correct");
                }
            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
