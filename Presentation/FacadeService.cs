using Domain.Entities;
using GemicleAppTest.Presentation;
using Infrastracture;

namespace Domain;

public class FacadeService
{
    public List<Timer> Start()
    {
        var campaigns = GetCampaigns();
        var campaignService = new CampaignService(new CustomerRepository());
        var customers = campaignService.GetAllCustomers();
        foreach (var campaign in campaigns.OrderBy(x => x.Priority))
        {
            var allowedCustomers = customers.Where(campaign.requironment);
            campaign.TargetCustomers.AddRange(allowedCustomers);
            customers.RemoveAll(x => campaign.requironment(x));
        }
        
        return StartJobs(campaigns);
    }

    private List<Timer> StartJobs(List<Campaign> campaigns)
    {
        var filePath = $"Campaigns_Job.txt";
        if (!File.Exists(filePath))
            using (var sw = File.Create(filePath));
        var writer = new MultiThreadFileWriterService(filePath);
        
        var timers = new List<Timer>();
        foreach (var campaign in campaigns)
        {
            timers.Add(new TaskSchedulerService(campaign, writer).StartAsync());
        }
        return timers;
    }

    public static void WriteToFile(Campaign campaign, MultiThreadFileWriterService writerService)
    {
        foreach (var customer in campaign.TargetCustomers)
        {
            var content = $"{campaign.Template}: Customer ID: {customer.CustomerId} \n";

            writerService.WriteLine(content);
        }
    }

    private List<Campaign> GetCampaigns()
    {
        return new List<Campaign>()
        {
            new Campaign
            {
                CampaignName = "Campaign 1",
                Template = "Template A",
                requironment = x => x.Gender == Gender.Male,
                ScheduleTime = new TimeOnly(10, 15),
                Priority = 1
            },
            new Campaign
            {
                CampaignName = "Campaign 2",
                Template = "Template B",
                requironment = x => x.Age > 45,
                ScheduleTime = new TimeOnly(10, 05),
                Priority = 2
            },
            new Campaign
            {
                CampaignName = "Campaign 3",
                Template = "Template C",
                requironment = x => x.City == "New York",
                ScheduleTime = new TimeOnly(10, 10),
                Priority = 5
            },
            new Campaign
            {
                CampaignName = "Campaign 4",
                Template = "Template A",
                requironment = x => x.Deposit > 100,
                ScheduleTime = new TimeOnly(10, 15),
                Priority = 3
            },
            new Campaign
            {
                CampaignName = "Campaign 5",
                Template = "Template C",
                requironment = x => x.NewCustomer,
                ScheduleTime = new TimeOnly(10, 05),
                Priority = 4
            }
        };
    }
}