namespace Domain.Entities;

public class Campaign
{
    public string CampaignName { get; set; }
    public string Template { get; set; }
    public Func<Customer, bool> requironment { get; set; }
    public List<Customer> TargetCustomers { get; set; } = new List<Customer>();
    public TimeOnly ScheduleTime { get; set; }
    public int Priority { get; set; }

    public void AddTargetCustomer(Customer customer)
    {
        TargetCustomers.Add(customer);
    }
}