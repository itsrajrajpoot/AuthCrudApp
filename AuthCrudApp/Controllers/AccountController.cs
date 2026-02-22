using AuthCrudApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(User user)
    {
        user.Role = "User";
        _context.Users.Add(user);
        _context.SaveChanges();
        return RedirectToAction("Login");
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == email && x.Password == password);

        if (user == null)
        {
            ViewBag.Error = "Invalid login";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "MyCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("MyCookie", principal);
        return RedirectToAction("Dashboard");
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookie");
        return RedirectToAction("Login");
    }
}
