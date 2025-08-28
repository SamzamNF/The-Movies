using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    public interface ICustomerProgramRepo
    {
        void Add(Customer customer);
        void Edit(Customer customer);
        Customer? FindCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
