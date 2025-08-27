using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using The_Movies.Model;
using The_Movies.MVVM;
using The_Movies.Repository;


namespace The_Movies.ViewModel
{
    internal class ScheduleControlViewModel : ViewModelBase
    {
        // Repository objekt
        private readonly CsvMovieGuide _csvMovieGuide;
        private readonly IMovieProgramRepo _movieProgramRepo;

        // Liste med Biografer, som bruges til ComboBoxen og deres tilhørende sale som er tilføjet som lister under deres respektive biografer        
        public List<Cinema> Cinemas { get; set; } = new()
        {
            new Cinema { CinemaName = "Hjerm", Halls = new List<Hall> {new Hall { HallNumber = "1A"}, new Hall { HallNumber = "2A"} } },

            new Cinema { CinemaName = "VideBæk", Halls = new List<Hall> {new Hall { HallNumber = "1B"}, new Hall { HallNumber = "2B"},
                new Hall { HallNumber = "3B"} } },

            new Cinema { CinemaName = "Thorsminde", Halls = new List<Hall> {new Hall { HallNumber = "1C"}, new Hall { HallNumber = "2C"},
                new Hall { HallNumber = "3C"}, new Hall { HallNumber = "4C" }, new Hall {HallNumber = "5C"} } },

            new Cinema { CinemaName = "Ræhr", Halls = new List<Hall> {new Hall { HallNumber = "1D"}, new Hall { HallNumber = "2D"},
                new Hall { HallNumber = "3D"}, new Hall { HallNumber = "4D"} } }


        };

        // Holder på valget af den valgte biograf
        private Cinema _selecetedCinema;

        // Holder på valget af den valgte film, som gemmes i MovieProgram objekt
        private Movie _selectedMovie;

        // Holder på det valgte Hall-objekt fra ComboBoxen
        private Hall _selectedHall;

        // Holder på listen af den nuværende valgte biograf, som viser listen af dens tilhørende halls
        private ObservableCollection<Hall> _halls;

        // Holder på listen af film som er oprettet i MovieViewModel
        public ObservableCollection<Movie> Movies { get; private set; }


        // Til at vælge biograf i ComboBoxen - Den gemmer altså valget
        // Opdatere DataGrid med valgte film som passer til, og opdatere den ComboBox der holder på de sale der passer til biografen.
        public Cinema SelectedCinema
        {
            get => _selecetedCinema;
            set
            {
                if (_selecetedCinema != value)
                {
                    _selecetedCinema = value;
                    OnPropertyChanged();
                    UpdateMoviePrograms();
                    UpdateHalls();
                }
            }
        }

        
        // Til at hente listen af sale der hører til Biografen
        public ObservableCollection<Hall> Halls
        {
            get => _halls;
            private set
            {
                _halls = value;
                OnPropertyChanged();
            }
        }

        // Til at vælge en Hall man gemmer i AddMovieProgram
        public Hall SelectedHall
        {
            get => _selectedHall;
            set
            {
                _selectedHall = value;
                OnPropertyChanged();
            }

        }

        // Property af "Movie" til at vælge film i XAML ComboBox
        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
            }
        }

        
        // Konstruktør
        public ScheduleControlViewModel()
        {
            _movieProgramRepo = new IMovieProgramFileRepo("movie_programs.txt"); // Dette kan nok fjernes til code-behind (Txt delen) når det er lavet
            _csvMovieGuide = new CsvMovieGuide();
            Movies = new ObservableCollection<Movie>();
            MoviePrograms = new ObservableCollection<MovieProgram>();
            LoadMoviesAsync();
        }

        // Henter alle film fra repository
        // Await sørger for at programmet kører videre, selvom den kan hente en stor fil.
        private async Task LoadMoviesAsync()
        {
            var loadedMovies = await _csvMovieGuide.IndlæsFilmFraCsv();
            Movies.Clear();
            foreach (var movie in loadedMovies)
            {
                Movies.Add(movie);
            }
        }

        //Metode til at opdatere listen med sale som hører til den valgte biograf
        private void UpdateHalls()
        {
            if (SelectedCinema != null)
            {
                Halls = new ObservableCollection<Hall>(SelectedCinema.Halls);
            }
        }


        // *-------*-------*-------*-------*-------*-------*-------*-------*-------*-------*-------*
        // Kode til selve at tilføje MovieProgram

        // Felter til MovieProgram
        private DateTime _playTime;
        private int _tickets;
        

        // Properties til MovieProgram
        public DateTime PlayTime
        {
            get => _playTime;
            set
            {
                _playTime = value;
                OnPropertyChanged();
            }
        }
        public int Tickets
        {
            get => _tickets;
            set
            {
                _tickets = value;
                OnPropertyChanged();
            }
        }



        // Liste til at holde over programmerne
        public ObservableCollection<MovieProgram> MoviePrograms { get; set; }

        // Metode til at opdatere filmprogrammer ud fra valgt Biograf
        private void UpdateMoviePrograms()
        {
            MoviePrograms.Clear();

            if (SelectedCinema != null)
            {
                // Henter alle programmer fra repository
                var AllMoviePrograms = _movieProgramRepo.GetAll();

                // Filtrere i en liste, alle de halls der passer til den valgte biograf
                var hallsForCinema = new HashSet<string>(SelectedCinema.Halls.Select(h => h.HallNumber));

                // Filtrere alle programmerne ned i en liste, hvor alle programmer der matcher deres hallNumber med de valgte halls
                var filteredMoviePrograms = AllMoviePrograms.Where(mp => hallsForCinema.Contains(mp.HallNumber));

                // Alle programmer hvor hallNumber = den valgte biografs hallNumber, tilføjes til listen af MoviePrograms
                foreach (var program in filteredMoviePrograms)
                {
                    MoviePrograms.Add(program);
                }
            }

        }


        // Metode til at tilføje et MovieProgram
        private void AddMovieProgram()
        {
            var movieProgram = new MovieProgram
            {
                Movie = SelectedMovie,
                PlayTime = this.PlayTime,
                HallNumber = SelectedHall.HallNumber,
                PlayDuration = SelectedMovie.Duration.Add(TimeSpan.FromMinutes(30)),
                Tickets = this.Tickets
            };

            _movieProgramRepo.Add(movieProgram);
            UpdateMoviePrograms();

            // Nulstiller felter og comboboxe
            SelectedMovie = null;
            SelectedHall = null;
            PlayTime = default;
            Tickets = default;



        }

        // Conditions til at knapper er aktive
        private bool CanAddMovieProgram() => SelectedMovie != null && SelectedHall != null && PlayTime != default;

        // Knapper til knapper
        public RelayCommand AddMovieProgramCommand => new RelayCommand(execute => AddMovieProgram(), canExecute => CanAddMovieProgram());






    }
}
