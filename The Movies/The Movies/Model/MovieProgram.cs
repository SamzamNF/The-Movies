using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace The_Movies.Model
{
    public class MovieProgram
    {
        public Movie Movie { get; set; }
        public DateTime PlayTime { get; set; }
        public string HallNumber { get; set; }
        public TimeSpan PlayDuration { get; set; }
        public int Tickets { get; set; }
        public int MovieProgramID { get; set; }

        
        public override string ToString()
        {
            string genres = string.Join(",", Movie.Genres); // Bruger komma inde i feltet, semikolon mellem felter

            return $"{Movie.Title};{Movie.Director};{Movie.PremierDate:dd-MM-yyyy};{genres};{Movie.Duration};{PlayDuration};{PlayTime:dd-MM-yyyy HH:mm};{HallNumber};{Tickets};{MovieProgramID}";
        }

        public static MovieProgram FromString(string data)
        {
            // Tjekker om dataen er tomt
            if (string.IsNullOrEmpty(data))
                return null;

            // Splitter dataen op og tjekker om der er 8 parts
            var parts = data.Split(';');
            if (parts.Length != 10)
                return null;

            // Tildeler de forskellige variabler deres data, ud fra parts nummer
            var title = parts[0];
            var director = parts[1];
            var premierDate = DateTime.ParseExact(parts[2], "dd-MM-yyyy", null);

            
            // Parser vores string med genrer tilbage til en enum med de korrekte genrer.
            // Ved at splitte dem med (,) i ToString gør det at vi kan finde alle de korrekte genrer, da resten af dataen er sepereret med (;)
            var genreStrings = parts[3].Split(',');
            var genres = new List<Genre>();
            foreach (var genre in genreStrings)
            {
                if (Enum.TryParse(genre, out Genre parsedGenre))
                {
                    genres.Add(parsedGenre);
                }
            }


            // Tildeler de resterende dataparts deres variabel ud fra deres nummer
            var movieDuration = TimeSpan.Parse(parts[4]);
            var playDuration = TimeSpan.Parse(parts[5]);
            var playTime = DateTime.ParseExact(parts[6], "dd-MM-yyyy HH:mm", null);
            var hallNumber = parts[7];
            if (!int.TryParse(parts[8], out var tickets))
                return null;
            if (!int.TryParse(parts[9], out tickets))
                return null;

            
            // Opretter movie objekt
            var movie = new Movie
            {
                Title = title,
                Director = director,
                PremierDate = premierDate,
                Duration = playDuration,
                Genres = genres,
            };

            //Opretter MovieProgram objekt
            return new MovieProgram
            {
                Movie = movie,
                PlayTime = playTime,
                HallNumber = hallNumber,
                PlayDuration = playDuration,
                Tickets = tickets,
            };



        }


    }
}
