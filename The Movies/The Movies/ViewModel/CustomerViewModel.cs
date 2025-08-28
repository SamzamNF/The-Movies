using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;
using The_Movies.MVVM;
using The_Movies.Repository;

namespace The_Movies.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly ICustomerProgramRepo _customerProgramRepo;

        private int _id {  get; set; }
        private string _firstName { get; set; }
        private string _email { get; set; }
        private string _phoneNumber { get; set; }


        // Behøves nok ikke? (ID)
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }
      
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        // Holder på listen af customers
        private ObservableCollection<Customer> _customers;

        // Til at hente listen af customers
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            private set
            {
                _customers = value;
                OnPropertyChanged();

            }
        }

        // Konstruktør
        public CustomerViewModel(ICustomerProgramRepo cRepository)
        {
            _customerProgramRepo = cRepository;
            Customers = new ObservableCollection<Customer>(_customerProgramRepo.GetAll());
        }


        private void AddCustomer()
        {
            //
            int nextId = Customers.Any() ? Customers.Max(c => c.ID) + 1 : 1;
            
            var customer = new Customer()
            {
                Name = this.FirstName,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                ID = nextId

            };

            Customers.Add(customer);
            _customerProgramRepo.Add(customer);


            FirstName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
        }

        private bool CanAddCustomer() => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(PhoneNumber);

        public RelayCommand AddCustomerCommand => new RelayCommand(execute => AddCustomer(), canExecute => CanAddCustomer());




    }
}
