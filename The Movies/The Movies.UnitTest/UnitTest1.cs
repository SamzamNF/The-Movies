using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using The_Movies.ViewModel;


namespace The_Movies.UnitTest
{
    [TestClass]
    public class MovieViewModelTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var movieViewModel = new MovieViewModel();

            // Act
            var result = movieViewModel.INSERTMETHODHERE();

            // Assert
            Assert.IsTrue(Result);
        }
    }
}
