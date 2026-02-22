using AuthCrudApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class EmployeeController : Controller
{
    private readonly AppDbContext _context;

    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string search)
    {
        var data = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            data = data.Where(x => x.Name.Contains(search));

        // Optional: show newest first
        data = data.OrderByDescending(x => x.Id);

        return View(data.ToList());
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(Employee emp)
    {
        _context.Employees.Add(emp);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        return View(_context.Employees.Find(id));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(Employee emp)
    {
        _context.Employees.Update(emp);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var emp = _context.Employees.Find(id);
        _context.Employees.Remove(emp);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
