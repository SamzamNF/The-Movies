using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    
    public enum Genre
    {
        Action,
        Adventure,
        Animation,
        Biography,
        Comedy,
        Crime,
        Documentary,
        Drama,
        Family,
        Fantasy,
        History,
        Horror,
        Musical,
        Mystery,
        Romance,
        SciFi,
        Sport,
        Thriller,
        War,
        Western
    }
    public class Movie
    {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Genre> Genres { get; set; } = new();
    }
}
