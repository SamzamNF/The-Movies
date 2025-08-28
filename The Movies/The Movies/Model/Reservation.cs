using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public int TicketAmount { get; set; }
        public string Movie {  get; set; }
        public DateTime ReservationDateTime { get; set; }


        public override string ToString()
        {
            return $"{ReservationID};{CustomerID};{TicketAmount};{Movie};{ReservationDateTime:dd-MM-yyyy HH:mm}";
        }

        public static Reservation FromString(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var parts = data.Split(';');

            if (parts.Length != 5)
                return null;

            if (!int.TryParse(parts[0], out var reservationId))
                return null;
            if (!int.TryParse(parts[1], out var customerID))
                return null;
            if (!int.TryParse(parts[2], out var rTicketAmount))
                return null;
            string movie = parts[3];

            var ReservationTime = DateTime.ParseExact(parts[4], "dd-MM-yyyy HH:mm", null);


            var reservation = new Reservation()
            {
                ReservationID = reservationId,
                CustomerID = customerID,
                TicketAmount = rTicketAmount,
                Movie = movie,
                ReservationDateTime = ReservationTime
            };

            return reservation;
        }
    }
}
