using Microsoft.AspNetCore.Mvc;
using ATMBankingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AccountsController : Controller
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<IActionResult> Index()
    {

        var accounts = await _accountService.GetAllAsync();
        return View(accounts);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var account = await _accountService.GetByIdAsync(id.Value);
        if (account == null) return NotFound();
        return View(account);
    }

    public async Task<IActionResult> CreateAsync()
    {
        var customers = await _accountService.GetAllCustomersAsync();
        ViewData["CustomerId"] = new SelectList(customers, "Id", "Name");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Account account)
    {
        if (ModelState.IsValid)
        {
            await _accountService.AddAsync(account);
            return RedirectToAction(nameof(Index));
        }
        foreach (var error in ModelState)
        {
            Console.WriteLine($"Key: {error.Key} - Error: {string.Join(",", error.Value.Errors.Select(e => e.ErrorMessage))}");
        }

        var customers = await _accountService.GetAllCustomersAsync();
        ViewData["CustomerId"] = new SelectList(customers, "Id", "Name", account.CustomerId);
        return View(account);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var account = await _accountService.GetByIdAsync(id.Value);
        if (account == null) return NotFound();

        return View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Account account)
    {
        if (id != account.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _accountService.UpdateAsync(account);
            return RedirectToAction(nameof(Index));
        }

        return View(account);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var account = await _accountService.GetByIdAsync(id.Value);
        if (account == null) return NotFound();

        return View(account);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _accountService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
