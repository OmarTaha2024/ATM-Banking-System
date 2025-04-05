using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("AdminDashboard");
        else
            return RedirectToAction("UserDashboard");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminDashboard()
    {
        var customerCount = await _context.Customers.CountAsync();
        var accountCount = await _context.Accounts.CountAsync();
        var totalBalance = await _context.Accounts.SumAsync(a => a.Balance);
        var transactionCount = await _context.Transactions.CountAsync();

        ViewBag.CustomerCount = customerCount;
        ViewBag.AccountCount = accountCount;
        ViewBag.TotalBalance = totalBalance;
        ViewBag.TransactionCount = transactionCount;

        return View();
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> UserDashboard()
    {
        var userEmail = User.Identity.Name;
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == userEmail);

        if (customer == null)
        {
            ViewBag.Message = "No customer found.";
            return View();
        }
        var accountIds = await _context.Accounts
    .Where(a => a.CustomerId == customer.Id)
    .Select(a => a.Id)
    .ToListAsync();


        var transactions = await _context.Transactions
    .Where(t => accountIds.Contains(t.AccountId))
    .OrderByDescending(t => t.Date)
    .Take(10)  
    .ToListAsync();

        var accounts = await _context.Accounts
    .Where(a => a.CustomerId == customer.Id)
    .ToListAsync();

        ViewBag.Accounts = accounts;
        ViewBag.Transactions = transactions;


        return View();
    }
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> PerformTransaction(int AccountId, decimal Amount, TransactionType Type)
    {
        var account = await _context.Accounts.FindAsync(AccountId);

        if (account == null)
        {
            TempData["TransactionMessage"] = "Account not found.";
            return RedirectToAction("UserDashboard");
        }

        if (Type == TransactionType.Withdraw && account.Balance < Amount)
        {
            TempData["TransactionMessage"] = "Insufficient balance!";
            return RedirectToAction("UserDashboard");
        }

        if (Type == TransactionType.Deposit)
            account.Balance += Amount;
        else if (Type == TransactionType.Withdraw)
            account.Balance -= Amount;

        var transaction = new Transaction
        {
            AccountId = account.Id,
            Amount = Amount,
            Type = Type,
            Date = DateTime.Now
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        var wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var receiptPath = PdfService.SaveTransactionReceiptToServer(transaction, account, wwwRoot);
        await EmailService.SendTransactionEmailAsync(
    toEmail: User.Identity.Name,
    subject: "Your ATM Transaction Receipt",
    body: $"Attached is your receipt for the recent {Type} of ${Amount}.",
    attachmentPath: receiptPath
);

        TempData["TransactionMessage"] = $"Transaction successful: {Type} ${Amount}";

        return RedirectToAction("DownloadReceipt", new { transactionId = transaction.Id });
    }
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DownloadReceipt(int transactionId)
    {
        var transaction = await _context.Transactions.Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == transactionId);
        if (transaction == null) return NotFound();

        var pdfBytes = PdfService.GenerateTransactionReceipt(transaction, transaction.Account);
        return File(pdfBytes, "application/pdf", $"Receipt_{transaction.Id}.pdf");
    }

}
