using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using The_Movies.ViewModel;
using The_Movies.Model;

namespace The_Movies.Tests
{
    public class MovieViewModelTests
    {
        private MovieViewModel _vm;

        [SetUp]
        public void Setup()
        {
            _vm = new MovieViewModel();

             // Fjern udkommenteringen, hvis der skal ryddes op i filen inden
           _vm.Movies.Clear();
        }

        [Test]
        public void AddMovieCommand_WithValidData_AddsMovieAndClearsFields()
        {
            // Opretter Filmen, med noget mock data
            _vm.Title = "Dr.DoLittle-3 Raven Hunter";
            _vm.Duration = "02:28:00";
            // tilføjer nogle random genrer
            var selectedGenres = new List<Genre> { Genre.Action, Genre.SciFi };

            //tester execute, samme som i vores UI
            if (_vm.AddMovieCommand.CanExecute(selectedGenres))
            {
                _vm.AddMovieCommand.Execute(selectedGenres);
            }

            Assert.AreEqual(1, _vm.Movies.Count, "Der burde være 1 film i listen.");
            Assert.AreEqual("Dr.DoLittle-3 Raven Hunter", _vm.Movies[0].Title, "Filmens titel matcher ikke.");
            Assert.AreEqual(2, _vm.Movies[0].Genres.Count, "Filmen skal have 2 genrer.");

            Assert.AreEqual(string.Empty, _vm.Title, "Title skulle være nulstillet.");
            Assert.AreEqual(string.Empty, _vm.Duration, "Duration skulle være nulstillet.");
            Assert.AreEqual(0, _vm.SelectedGenres.Count, "SelectedGenres skulle være nulstillet.");
        }
    }
}
