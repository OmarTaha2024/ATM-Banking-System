using System.ComponentModel.DataAnnotations;

public class Customer
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Full Name")]
    public string Name { get; set; }

    [Required]
    public string NationalId { get; set; }  

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string Phone { get; set; }

    public ICollection<Account> Accounts { get; set; } = new List<Account>();

}
