using System.Reflection;
using System.Text;
using Domain.Entities;

namespace Infrastracture;

public class CustomerRepository : IRepository<Customer>
{
    private const string _csvFileName = "customers.csv";

    public CustomerRepository()
    {
    }

    public List<Customer> GetAll()
    {
        var customers = new List<Customer>();

        var filePath = Assembly.GetExecutingAssembly().Location;

        int index = filePath.IndexOf("GemicleAppTest");
        string path = filePath.Substring(0, index + _csvFileName.Length + 1);

        var _csvFilePath = Path.Combine(path, "Infrastracture", _csvFileName);
        // Combine with the specific file path
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The CSV file was not found.", filePath);
        }

        var lines = File.ReadAllLines(_csvFilePath);

        foreach (var line in lines)
        {
            var fields = line.Split(',');

            try
            {
                if (fields.Length == 6)
                {
                    var customer = new Customer
                    {
                        CustomerId = int.Parse(fields[0]),
                        Age = int.Parse(fields[1]),
                        Gender = Enum.Parse<Gender>(fields[2]),
                        City = fields[3],
                        Deposit = decimal.Parse(fields[4]),
                        NewCustomer = int.Parse(fields[5]) == 1
                    };

                    customers.Add(customer);
                }
            }
            catch {}
        }

        return customers;
    }
}