using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    class CustomerProgramFileRepo : ICustomerProgramRepo
    {
        private readonly string _filePath = "customers.txt";

        // Samme stil som ReservationProgramFileRepo
        public CustomerProgramFileRepo(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
        }

        private List<Customer> GetAll()
        {
            var list = new List<Customer>();
            try
            {
                using (var sr = new StreamReader(_filePath))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var c = Customer.FromString(line);
                        if (c != null) list.Add(c);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return list;
        }

        private void SaveAll(IEnumerable<Customer> customers)
        {
            try
            {
                using (var sw = new StreamWriter(_filePath, append: false))
                {
                    foreach (var c in customers)
                        sw.WriteLine(c.ToString());
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void Add(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            try
            {
                var all = GetAll();
                if (customer.ID == 0)
                    customer.ID = all.Any() ? all.Max(x => x.ID) + 1 : 1;

                using (var sw = new StreamWriter(_filePath, append: true))
                {
                    sw.WriteLine(customer.ToString());
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void Edit(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            var all = GetAll();
            var idx = all.FindIndex(c => c.ID == customer.ID);
            if (idx < 0) return;
            all[idx] = customer;
            SaveAll(all);
        }

        // Finder på ID -> ellers Email -> ellers Phone -> ellers Name
        public Customer? FindCustomer(Customer probe)
        {
            try
            {
                var all = GetAll();
                if (probe.ID != 0)
                    return all.FirstOrDefault(c => c.ID == probe.ID);
                if (!string.IsNullOrWhiteSpace(probe.Email))
                    return all.FirstOrDefault(c =>
                        c.Email.Equals(probe.Email, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(probe.PhoneNumber))
                    return all.FirstOrDefault(c => c.PhoneNumber == probe.PhoneNumber);
                if (!string.IsNullOrWhiteSpace(probe.Name))
                    return all.FirstOrDefault(c =>
                        c.Name.Equals(probe.Name, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
        }

        public void DeleteCustomer(Customer customer)
        {
            var all = GetAll();
            int before = all.Count;

            if (customer.ID != 0)
                all.RemoveAll(c => c.ID == customer.ID);
            else if (!string.IsNullOrWhiteSpace(customer.Email))
                all.RemoveAll(c => c.Email.Equals(customer.Email, StringComparison.OrdinalIgnoreCase));
            else if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                all.RemoveAll(c => c.PhoneNumber == customer.PhoneNumber);
            else if (!string.IsNullOrWhiteSpace(customer.Name))
                all.RemoveAll(c => c.Name.Equals(customer.Name, StringComparison.OrdinalIgnoreCase));

            if (all.Count != before)
                SaveAll(all);
        }
    }
}
