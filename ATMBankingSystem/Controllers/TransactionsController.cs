using Microsoft.AspNetCore.Mvc;
using ATMBankingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "User,Admin")]
public class TransactionsController : Controller
{
    private readonly ITransactionService _transactionService;
    public enum TransactionType
    {
        Deposit,
        Withdraw
    }

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<IActionResult> Index()
    {
        List<Transaction> transactions;

        if (User.IsInRole("Admin"))
        {
            transactions = await _transactionService.GetAllAsync();
        }
        else
        {
            var userEmail = User.Identity.Name;
            transactions = await _transactionService.GetTransactionsForUserAsync(userEmail);
        }

        return View(transactions);
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await _transactionService.GetByIdAsync(id.Value);
        if (transaction == null) return NotFound();

        return View(transaction);
    }

    public async Task<IActionResult> Create()
    {
        var accounts = await _transactionService.GetAllAccountsAsync(); 
        ViewData["AccountId"] = new SelectList(accounts, "Id", "AccountNumber");

        ViewData["Type"] = new SelectList(Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>());

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            await _transactionService.AddAsync(transaction);
            return RedirectToAction(nameof(Index));
        }

        return View(transaction);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await _transactionService.GetByIdAsync(id.Value);
        if (transaction == null) return NotFound();

        return View(transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Transaction transaction)
    {
        if (id != transaction.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _transactionService.UpdateAsync(transaction);
            return RedirectToAction(nameof(Index));
        }

        return View(transaction);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await _transactionService.GetByIdAsync(id.Value);
        if (transaction == null) return NotFound();

        return View(transaction);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _transactionService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
