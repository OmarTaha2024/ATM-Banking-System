using ATMBankingSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum TransactionType
{
    Deposit,
    Withdraw
}

public class Transaction
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    public TransactionType Type { get; set; }

    [Required]
    public decimal Amount { get; set; }
    [Required]

    public int AccountId { get; set; }

    
    [ValidateNever]
    public Account Account { get; set; }
}
