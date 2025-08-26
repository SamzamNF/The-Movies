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
using The_Movies.View;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("The-Movies.Tests")]

namespace The_Movies.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // Konstruktør som sørger for programmet starter med at sætte viewet "Movie" til at være det første program vist
        public MainViewModel()
        {
            CurrentView = new MovieControl();
        }       
        
        //Opretter view felt
        private object _currentView;

        //Property til at holde og skifte det aktuelle view
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        //Command, der skifter til MovieView når knappen klikkes (ikke implementeret endnu)
        public RelayCommand ShowMovieViewCommand => new RelayCommand(execute => ShowMovieView());

        public RelayCommand ShowProgramViewCommand => new RelayCommand(execute => ShowProgramView());

        //Skifter det det valgte view til MovieControl
        private void ShowMovieView()
        {
            CurrentView = new MovieControl(); // Skift til MovieView
        }

        private void ShowProgramView()
        {
            CurrentView = new ScheduleControl(); // Skift til ScheduleControlView
        }

    }


   
}
