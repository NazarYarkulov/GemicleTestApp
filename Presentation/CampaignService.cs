using Domain.Entities;
using Infrastracture;

namespace GemicleAppTest.Presentation;

public class CampaignService
{
    private readonly CustomerRepository _customerRepository;
    
    public CampaignService(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public List<Customer> GetAllCustomers()
    {
        return _customerRepository.GetAll();
    }

}