using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ATMBankingSystem.Models;
using System.IO;

public static class PdfService
{
    public static byte[] GenerateTransactionReceipt(Transaction transaction, Account account)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Footer().AlignCenter().Text("Thank you for using our ATM service.");

                page.Content().Column(col =>
                {
                    col.Item().Text($"Date: {transaction.Date}").FontSize(12);
                    col.Item().Text($"Account Number: {account.AccountNumber}");
                    col.Item().Text($"Transaction Type: {transaction.Type}");
                    col.Item().Text($"Amount: ${transaction.Amount}");
                    col.Item().Text($"Remaining Balance: ${account.Balance}");
                    col.Item().Text($"Transaction ID: {transaction.Id}");
                });

                page.Footer().AlignCenter().Text("Thank you for using our ATM service.");
            });
        });

        return document.GeneratePdf();
    }
    public static string SaveTransactionReceiptToServer(Transaction transaction, Account account, string wwwRootPath)
    {
        var pdfBytes = GenerateTransactionReceipt(transaction, account);

        string folderPath = Path.Combine(wwwRootPath, "receipts");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = $"Receipt_{transaction.Id}.pdf";
        string filePath = Path.Combine(folderPath, fileName);

        File.WriteAllBytes(filePath, pdfBytes);

        return filePath; // ترجع المسار الكامل للملف
    }

}
