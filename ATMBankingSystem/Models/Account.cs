using ATMBankingSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Account
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Account Number")]
    public string AccountNumber { get; set; }

    [Required]
    public decimal Balance { get; set; } = 0;

    [Required]
    public string PinCode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [NotMapped]
    public string DisplayText => $"{AccountNumber} - {Customer?.Name}";

    [Required]
    public int CustomerId { get; set; }
    [ValidateNever]
    public Customer Customer { get; set; }  

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
