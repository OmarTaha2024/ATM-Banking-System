using Microsoft.AspNetCore.Mvc;
using ATMBankingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "User,Admin")]
public class TransactionsController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;

    public enum TransactionType
    {
        Deposit,
        Withdraw
    }

    public TransactionsController(ITransactionService transactionService,IAccountService accountService)
    {
        _transactionService = transactionService;
         _accountService = accountService;
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

    [Authorize]
    public async Task<IActionResult> Create()
    {
        List<Account> accounts;

        if (User.IsInRole("Admin"))
        {
            accounts = await _accountService.GetAllAsync(); 
        }
        else
        {
            var userEmail = User.Identity.Name;
            accounts = await _accountService.GetAccountsByUserEmailAsync(userEmail);
        }

        ViewData["AccountId"] = new SelectList(accounts, "Id", "DisplayText");
        ViewData["Type"] = new SelectList(Enum.GetValues(typeof(TransactionType)));

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
