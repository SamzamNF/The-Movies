using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    // konkret file-implementering (uden I-prefix)
    class ReservationProgramFileRepo : IReservationProgramRepo
    {
        private readonly string _filePath = "reservations.txt";

        public ReservationProgramFileRepo(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
        }

        private List<Reservation> GetAll()
        {
            var list = new List<Reservation>();
            try
            {
                using var sr = new StreamReader(_filePath);
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    var r = Reservation.FromString(line);
                    if (r != null) list.Add(r);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return list;
        }

        private void SaveAll(IEnumerable<Reservation> items)
        {
            try
            {
                using var sw = new StreamWriter(_filePath, append: false);
                foreach (var r in items) sw.WriteLine(r.ToString());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public Reservation CreateReservation(Customer customer, MovieProgram program, int amount)
        {
            var reservation = new Reservation
            {
                CustomerID = customer.ID,
                TicketAmount = amount,
                Movie = program.Movie.Title,
                ReservationDateTime = program.PlayTime
            };

            try
            {
                var all = GetAll();
                reservation.ReservationID = all.Any() ? all.Max(x => x.ReservationID) + 1 : 1;
                using var sw = new StreamWriter(_filePath, append: true);
                sw.WriteLine(reservation.ToString());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return reservation;
        }

        public Reservation? FindReservation(int reservationID)
        {
            try
            {
                using var sr = new StreamReader(_filePath);
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    var r = Reservation.FromString(line);
                    if (r != null && r.ReservationID == reservationID)
                        return r;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
        }

        public bool CancelReservation(int reservationID)
        {
            var all = GetAll();
            var removed = all.RemoveAll(r => r.ReservationID == reservationID) > 0;
            if (removed) SaveAll(all);
            return removed;
        }
    }
}
