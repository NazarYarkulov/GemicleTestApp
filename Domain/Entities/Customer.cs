namespace Domain.Entities;

public class Customer
{
    public int CustomerId { get; set; }

    public int Age { get; set; }

    public Gender? Gender { get; set; }

    public string? City { get; set; }

    public decimal Deposit { get; set; }

    public bool NewCustomer { get; set; }
}