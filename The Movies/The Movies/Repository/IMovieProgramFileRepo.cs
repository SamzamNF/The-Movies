using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    class IMovieProgramFileRepo : IMovieProgramRepo
    {
        private readonly string _filePath = "moviePrograms.txt";

        
        // Konstruktør der sørger for filen er oprettet
        public IMovieProgramFileRepo(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
        }
        
        
        // Metode til at tilføje film til CSV fil ved brug af StreamWriter og ToString fra MovieProgram klassen
        public void Add(MovieProgram movieProgram)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_filePath, append: true))
                {
                    sw.WriteLine(movieProgram.ToString());
                }
            }
            catch (IOException msg)
            {
                Console.WriteLine(msg.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        // Metode til at slette film fra CSV fil, bruger metoden SaveAll() for at override CSV bagefter - ikke færdig implementeret endnu
        public void Delete(MovieProgram movieProgram)
        {
            /*
            var MovieProgramList = GetAll();

            var ProgramToDelete = MovieProgramList.FirstOrDefault(m => m.HallNumber == movieProgram.HallNumber); */
        }

        // Metode til at hente alle film fra CSV så det kan loades ind i en ObserverableCollection, til at slette data eller andet hvor man skal bruge alt dataen raw først.
        public List<MovieProgram> GetAll()
        {
            var moviePList = new List<MovieProgram>();

            try
            {
                using (StreamReader sr = new StreamReader(_filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        MovieProgram movieProgram = MovieProgram.FromString(line);
                        if (movieProgram != null)
                        {
                            moviePList.Add(movieProgram);
                        }
                    }
                }
                return moviePList;
            }
            catch (IOException msg)
            {
                Console.WriteLine(msg.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
