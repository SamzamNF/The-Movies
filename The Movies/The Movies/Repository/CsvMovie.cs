using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{

    public class CsvMovieGuide
    {
        //hvad skal hans lorte peters film csv hedde?
        private string csvFilePath = "mine_film.csv";

        public async Task GemEnFilmTilCsv(Movie film)
        {
            try
            {
                // 1. Lav film om til en CSV linje
                string csvLinje = LavMovieOmTilCsvLinje(film);
                
                // 2. Tjek om filen eksisterer hvis ikke, lav header først
                if (!File.Exists(csvFilePath))
                {
                    string header = "Titel;Varighed_Minutter;Genrer";
                    await File.WriteAllTextAsync(csvFilePath, header + Environment.NewLine, Encoding.UTF8);
                    Console.WriteLine("Ny CSV fil oprettet med header!");
                }

                // 3. Tilføj filmen til CSV filen
                await File.AppendAllTextAsync(csvFilePath, csvLinje + Environment.NewLine, Encoding.UTF8);
                
                Console.WriteLine($"Film '{film.Title}' gemt i CSV!");
            }
            catch (Exception fejl)
            {
                Console.WriteLine($"PETER FOR HELVEDE: {fejl.Message}");
            }
        }
        

        public async Task GemFlereFilmTilCsv(List<Movie> film)
        {
            try
            {
                // vi laver en writeline for at vise at vi er kommet her ind.
                Console.WriteLine($"Gemmer {film.Count} film til CSV...");

                // 1. Byg hele CSV indholdet som tekst
                var csvIndhold = new StringBuilder();

                // 2. Tilføj header først
                csvIndhold.AppendLine("Titel;Varighed_Minutter;Genrer");

                // 3. Tilføj hver film som en linje
                foreach (var enFilm in film)
                {
                    string csvLinje = LavMovieOmTilCsvLinje(enFilm);
                    csvIndhold.AppendLine(csvLinje);
                }

                // 4. Skriv alt til filen på én gang
                await File.WriteAllTextAsync(csvFilePath, csvIndhold.ToString(), Encoding.UTF8);

                Console.WriteLine($"{film.Count} film gemt i '{csvFilePath}'!");
            }
            catch (Exception fejl)
            {
                Console.WriteLine($"FORHELVEDE PETER: {fejl.Message}");
            }
        }


        public async Task<List<Movie>> IndlæsFilmFraCsv()
        {
            var film = new List<Movie>();

            try
            {
                // 1. Tjek om filen eksisterer
                if (!File.Exists(csvFilePath))
                {
                    Console.WriteLine("CSV filen eksisterer ikke endnu!");
                    return film;
                }

                // 2. Læs alle linjer fra filen
                string[] linjer = await File.ReadAllLinesAsync(csvFilePath, Encoding.UTF8);

                // 3. Spring header over 
                for (int i = 1; i < linjer.Length; i++)
                {
                    try
                    {
                        Movie enFilm = LavCsvLinjeOmTilMovie(linjer[i]);
                        if (enFilm != null)
                        {
                            film.Add(enFilm);
                        }
                    }
                    catch (Exception linjeFejl)
                    {
                        Console.WriteLine($"Kunne ikke læse linje {i + 1}: {linjeFejl.Message}");
                    }
                }

                Console.WriteLine($"{film.Count} film indlæst fra CSV!");
            }
            catch (Exception fejl)
            {
                Console.WriteLine($" Fejl ved indlæsning: {fejl.Message}");
            }

            return film;
        }

        // en funktion til at sæge i vores csv fil, dewn returnere ingen ting hvis ikke der er noget
        public async Task<List<Movie>> SøgIFilmCsv(string søgeord)
        {
            var fundneFilm = new List<Movie>();

            try
            {
                if (!File.Exists(csvFilePath))
                {
                    Console.WriteLine("Ingen CSV fil at søge i!");
                    return fundneFilm;
                }

                // Læs filen linje for linje
                using var reader = new StreamReader(csvFilePath, Encoding.UTF8);

                // Spring header over
                await reader.ReadLineAsync();

                string? linje;
                while ((linje = await reader.ReadLineAsync()) != null)
                {
                    // Tjek om linjen indeholder søgeordet
                    if (linje.ToLower().Contains(søgeord.ToLower()))
                    {
                        try
                        {
                            Movie film = LavCsvLinjeOmTilMovie(linje);
                            if (film != null)
                            {
                                fundneFilm.Add(film);
                            }
                        }
                        catch
                        {
                            //catch peters cat.
                        }
                    }
                }

                Console.WriteLine($"Fandt {fundneFilm.Count} film der matcher '{søgeord}'");
            }
            catch (Exception fejl)
            {
                Console.WriteLine($"Søgefejl: {fejl.Message}");
            }

            return fundneFilm;
        }

        // den her skal vi finde ud af hvordan vi håndtere, hvordan læser vi filmene ind?
            // er det med ; eller , eller hvad tænker vi?
        private string LavMovieOmTilCsvLinje(Movie film)
        {
            // 1. Titel
            string titel = film.Title.Replace(";", ","); // Erstat ; med , så det ikke ødelægger CSV

            // 2. Varighed
            double minutter = film.Duration.TotalMinutes;

            // 3. Genrer
            string genrer = string.Join("|", film.Genres);

            //Men skal det hele samles med semikolon?
            return $"{titel};{minutter};{genrer}";
        }


        private Movie? LavCsvLinjeOmTilMovie(string csvLinje)
        {
            // 1. Split linjen op ved semikolon
            string[] dele = csvLinje.Split(';');
            
            // 2. Tjek at vi har nok dele
            if (dele.Length < 3)
            {
                Console.WriteLine($"Ugyldig CSV linje (for få dele): {csvLinje}");
                return null;
            }

            try
            {
                // 3. Parse titel
                string titel = dele[0].Trim();
                
                // 4. Parse varighed
                if (!double.TryParse(dele[1], out double minutter))
                {
                    Console.WriteLine($"Ugyldig varighed: {dele[1]}");
                    return null;
                }
                
                // 5. Parse genrer
                var genrer = new List<Genre>();
                if (!string.IsNullOrWhiteSpace(dele[2]))
                {
                    string[] genreNavne = dele[2].Split('|');
                    foreach (string genreNavn in genreNavne)
                    {
                        if (Enum.TryParse<Genre>(genreNavn.Trim(), out Genre genre))
                        {
                            genrer.Add(genre);
                        }
                    }
                }

                // 6. Lav Movie objektet
                return new Movie
                {
                    Title = titel,
                    Duration = TimeSpan.FromMinutes(minutter),
                    Genres = genrer
                };
            }
            catch (Exception fejl)
            {
                Console.WriteLine($"Fejl ved parsing af linje: {fejl.Message}");
                return null;
            }
        }

        public async Task LavBackup()
        {
            try
            {
                if (!File.Exists(csvFilePath))
                {
                    Console.WriteLine(" Ingen CSV fil at lave backup af!");
                    return;
                }

                // Lav backup filnavn med timestamp
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupNavn = $"backup_film_{timestamp}.csv";
                
                // Kopier filen
                await File.WriteAllTextAsync(backupNavn, await File.ReadAllTextAsync(csvFilePath, Encoding.UTF8), Encoding.UTF8);
                
                Console.WriteLine($" Backup gemt som: {backupNavn}");
            }
            catch (Exception fejl)
            {
                Console.WriteLine($" Backup fejlede: {fejl.Message}");
            }
        }

        public async Task VisStatistikker()
        {
            try
            {
                var alleFilm = await IndlæsFilmFraCsv();
                
                if (!alleFilm.Any())
                {
                    Console.WriteLine("Ingen film at vise statistikker for!");
                    return;
                }

                Console.WriteLine("FILM STATISTIKKER:");
                Console.WriteLine($"   🎬 Antal film: {alleFilm.Count}");
                
                // Total spilletid
                var totalSpilletid = TimeSpan.FromMinutes(alleFilm.Sum(f => f.Duration.TotalMinutes));
                Console.WriteLine($" Total spilletid: {totalSpilletid.TotalHours:F1} timer");
                
                // Længste film
                var længsteFilm = alleFilm.OrderByDescending(f => f.Duration).FirstOrDefault();
                Console.WriteLine($"Længste film: {længsteFilm?.Title} ({længsteFilm?.Duration.TotalMinutes:F0} min)");
                
                // Genre fordeling
                var genreTælling = new Dictionary<Genre, int>();
                foreach (var film in alleFilm)
                {
                    foreach (var genre in film.Genres)
                    {
                        genreTælling[genre] = genreTælling.GetValueOrDefault(genre, 0) + 1;
                    }
                }
                
                Console.WriteLine(" Genre fordeling:");
                foreach (var genre in genreTælling.OrderByDescending(g => g.Value))
                {
                    Console.WriteLine($"      {genre.Key}: {genre.Value} film");
                }
            }
            catch (Exception fejl)
            {
                Console.WriteLine($"Fejl ved statistikker: {fejl.Message}");
            }
        }


    
    }

}
