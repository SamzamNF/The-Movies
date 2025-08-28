using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ID { get; set; }


        public override string ToString()
        {
            return $"{Name};{Email};{PhoneNumber};{ID}";
        }

        public static Customer FromString(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var parts = data.Split(';');
            if (parts.Length != 4)
                return null;

            var name = parts[0];
            var email = parts[1];
            var phoneNumber = parts[2];
            
            // Hvis værdien kan laves til en integer, så sættes det i variablen reservationID - Ellers returneres null
            if (!int.TryParse(parts[3], out var customerID))
                return null;

            Customer customer = new Customer()
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                ID = customerID
            };
            return customer;
        }
    }
}
