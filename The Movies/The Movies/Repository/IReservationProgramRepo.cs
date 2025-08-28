using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    public interface IReservationProgramRepo
    {
        
        Reservation CreateReservation(Customer customer, MovieProgram program, int amount);
        bool CancelReservation(int reservationID);
        Reservation? FindReservation(int reservationID);
    }
}
