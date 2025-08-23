using System;
using System.Collections;
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
    internal class MovieViewModel : ViewModelBase
    {
        // Repository
        private readonly CsvMovieGuide _csvMovieGuide;

        
        
        // Felter til film
        
        private string _title { get; set; }
        private string _duration { get; set; }

        // Properties til film

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

        // Array til alle genrer - Bruges til Listboxen
        public Array AllGenres => Enum.GetValues(typeof(Genre));

        // Liste over de valgte genrer
        public ObservableCollection<Genre> SelectedGenres { get; set; } = new();

        // Liste over film
        public ObservableCollection<Movie> Movies { get; set; } //= new();


        // Konstruktør
        public MovieViewModel()
        {
            // Intialisere et objekt af repository, sætter movies = en ny observablecollection og loader derefter alle film ind fra fil i listen.

            _csvMovieGuide = new CsvMovieGuide();
            Movies = new ObservableCollection<Movie>();
            LoadMoviesAsync();
        }

        
        // Henter alle film fra repository
        // Await sørger for at programmet kører videre, selvom den kan hente en stor fil.
        private async void LoadMoviesAsync()
        {
            var moviesFromCsv = await _csvMovieGuide.IndlæsFilmFraCsv();
            foreach (var movie in moviesFromCsv)
            {
                Movies.Add(movie);
            }
        }

        // Metoder

        // Genrer kommer fra ListBox, som har en indbygget SelectedItems. Det sender så netop de valgte genrer til metoden her.
        // De bliver sendt igennem CommandParameter i xaml når knappen senere bruges. Genren(ene) kommer fra RelayCommand efter, hvor execute er variablen med "selecteditems i"
        private async void AddMovie(object selectedItems)
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
            await _csvMovieGuide.GemEnFilmTilCsv(movie);

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
