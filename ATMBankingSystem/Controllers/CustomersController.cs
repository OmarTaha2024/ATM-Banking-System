

using Microsoft.AspNetCore.Mvc;
using ATMBankingSystem.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: Customers
    public async Task<IActionResult> Index()
    {
        var customers = await _customerService.GetAllAsync();
        return View(customers);
    }

    // GET: Customers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var customer = await _customerService.GetByIdAsync(id.Value);
        if (customer == null) return NotFound();

        return View(customer);
    }

    // GET: Customers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Customers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,NationalId,Email,Phone")] Customer customer)
    {
        if (ModelState.IsValid)
        {
            await _customerService.AddAsync(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var customer = await _customerService.GetByIdAsync(id.Value);
        if (customer == null) return NotFound();

        return View(customer);
    }

    // POST: Customers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NationalId,Email,Phone")] Customer customer)
    {
        if (id != customer.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _customerService.UpdateAsync(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var customer = await _customerService.GetByIdAsync(id.Value);
        if (customer == null) return NotFound();

        return View(customer);
    }

    // POST: Customers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _customerService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
