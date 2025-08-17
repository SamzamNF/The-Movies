using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using The_Movies.Model;
using The_Movies.MVVM;

namespace The_Movies.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private string _title { get; set; }
        private string _duration { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public string Duration
        {
            get => _duration;
            set
            {
                _duration = value; 
                OnPropertyChanged();
            }
        }

        // Array til alle genrer
        public Array AllGenres => Enum.GetValues(typeof(Genre));

        // Liste over de valgte genrer
        public ObservableCollection<Genre> SelectedGenres { get; set; } = new();

        // Liste over film
        public ObservableCollection<Movie> Movies { get; set; } //= new();


        // Konstruktør
        public MainViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            
            // Mangler at hente direkte fra filen af film og indlæse dem når program starter
        }

        // Metoder
        private void AddMovie(object selectedItems)
        {
            var movie = new Movie
            {
                Title = this.Title,
                Duration = TimeSpan.TryParse(this.Duration, out var dur) ? dur : TimeSpan.Zero,
                Genres = new List<Genre>()
            };

            if (selectedItems is IList items)
            {
                movie.Genres = items.OfType<Genre>().ToList();
            }

            Movies.Add(movie);

            // Reset felter
            Title = string.Empty;
            Duration = string.Empty;
            SelectedGenres.Clear();
        }


        // Metoder med condition til knapper
        private bool CanAddMovie() => !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Duration);

        // Metoder til knapper
        public RelayCommand AddMovieCommand => new RelayCommand(execute => AddMovie(execute), canExecute => CanAddMovie()); 


    }
}
