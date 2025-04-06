using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;

public static class PdfService
{
    public static byte[] GenerateTransactionReceipt(Transaction transaction, Account account)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6);
                page.Margin(30);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content()
                    .Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().AlignCenter().Text("ATM BANK SYSTEM").Bold().FontSize(16);
                        col.Item().AlignCenter().Text("Transaction Receipt").Italic();

                        col.Item().LineHorizontal(1);

                        col.Item().Text($"Date:         {transaction.Date:G}");
                        col.Item().Text($"Account No.:  {account.AccountNumber}");
                        col.Item().Text($"Customer:     {account.Customer?.Name}");
                        col.Item().Text($"Transaction:  {transaction.Type}");
                        col.Item().Text($"Amount:       ${transaction.Amount:F2}");
                        col.Item().Text($"Balance:      ${account.Balance:F2}");

                        col.Item().LineHorizontal(1);
                        col.Item().AlignCenter().Text("Thank you for banking with us!");
                    });
            });
        });

        return document.GeneratePdf();
    }
}
