using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema
{
    public class DatabaseInitializer
    {
        public static void Initialize()
        {
            if (!Core.Context.Genres.Any())
            {
                Core.Context.Genres.AddRange(
                    new Genre { Name = "Drama" },
                    new Genre { Name = "Thriller" },
                    new Genre { Name = "Comedy" },
                    new Genre { Name = "Crime" },
                    new Genre { Name = "Action" },
                    new Genre { Name = "Biography" },
                    new Genre { Name = "Fantasy" }
                    );
                Core.Context.SaveChanges();
            }

            var genres = Core.Context.Genres.ToDictionary(g => g.Name);

            if (!Core.Context.Movies.Any())
            {
                AddMovie("Fight Club",
                    "An insurance company employee suffers from chronic insomnia and desperately seeks escape from his excruciatingly boring life. " +
                    "One day, on a business trip, he meets Tyler Durden, a charismatic soap salesman with a twisted philosophy. " +
                    "Tyler believes that self-improvement is for the weak, and the only thing worth living is self-destruction." +
                    "A short time passes, and the new friends are beating each other senseless in the bar parking lot, the cathartic brawl bringing them supreme bliss. " +
                    "Introducing other men to the simple joys of physical violence, they found a secret Fight Club, which became incredibly popular.", 
                    8.7m, "18+", genres, "Thriller", "Drama", "Crime");

                AddMovie("Green Book",
                    "It's the 1960s. After a New York nightclub closes for renovations, bouncer Tony \"Talkie\" Shirley is looking for a few months' work. " +
                    "At the same time, Don Shirley—a sophisticated socialite, a wealthy and talented black classical musician—is planning a tour of the southern states, where racism and segregation are still rife. " +
                    "He hires Tony as his driver, bodyguard, and problem-solver. These two have so little in common, and this trip will change both their lives forever.", 
                    8.6m, "18+", genres, "Biography", "Comedy", "Drama");

                AddMovie("Real Steel",
                    "The film takes place in a future where boxing is banned for its inhumanity and replaced by fights between 2,000-pound robots controlled by humans. " +
                    "A former boxer turned promoter who defects to Robobox believes he's finally found his champion when he encounters a flawed but highly capable robot. " +
                    "Meanwhile, an 11-year-old boy, who turns out to be his son, crosses his path. And as the machine fights its way to the top, the newly reunited father and son develop a friendship.", 
                    7.7m, "18+", genres, "Fantasy", "Action");

                AddMovie("Drive",
                    "A masterful driver, he performs stunts on Hollywood sets by day and plays the high-stakes game by night. " +
                    "But one dangerous contract puts a bounty on his life. Now, to stay alive and save his charming neighbor, he must do what he does best—masterfully evade capture.", 
                    7.2m, "18+", genres, "Drama", "Crime", "Thriller");

                AddMovie("Fast & Furious",
                    "His name is Brian, and he's a fan of turbochargers and nitro boosts. " +
                    "He's trying to infiltrate the auto gang of the legendary Dominic Toretto, a champion of dangerous and illegal street racing. " +
                    "Brian is also a cop, and his mission is to gain the trust of Toretto, who is suspected of involvement in daring heists of trucks carried out on the move.", 
                    7.8m, "18+", genres, "Action", "Thriller", "Crime");

                AddMovie("John Wick",
                    "John Wick is, at first glance, a perfectly ordinary, average American, leading a quiet, peaceful life. " +
                    "However, few people know that he was a contract killer, and one of the most accomplished professionals in the business. " +
                    "After the son of a gang leader and his buddies steal his beloved 1969 Mustang, killing his dog, Daisy, a gift from his recently deceased wife, John is forced to return to his past. " +
                    "Now, Wick is hunting down those who were careless enough to cross him, and he's willing to do anything to get revenge.", 
                    7.0m, "18+", genres, "Action", "Thriller", "Crime");

                AddMovie("The Wolf of Wall Street",
                    "It's 1987. Jordan Belfort becomes a broker at a successful investment bank. " +
                    "The bank soon closes after a sudden crash in the Dow Jones Industrial Average. " +
                    "On the advice of his wife, Teresa, Jordan takes a job at a small penny stock brokerage. " +
                    "His assertive client-facing style and natural charisma quickly pay off. He meets his next-door neighbor, Donny, a trader who immediately connects with Jordan and decides to open their own firm with him. " +
                    "They hire several of Belfort's friends and his father, Max, as employees, and name the company \"Stratton Oakmont.\" " +
                    "In his free time, Jordan lives a life of luxury: he jets from one party to the next, has sexual relationships with prostitutes, and uses a variety of drugs, including cocaine and Quaaludes. One day, an FBI agent becomes interested in Belfort's rapid enrichment.", 
                    8.1m, "18+", genres, "Drama", "Crime", "Biography", "Comedy");

                AddMovie("Joker",
                    "Gotham, early 1980s. Comedian Arthur Fleck lives with his ailing mother, who has taught him since childhood to \"walk with a smile.\" " +
                    "Trying to bring goodness into the world and bring joy, Arthur encounters human cruelty and gradually comes to the conclusion that the world will receive from him not a kind smile, but the smirk of the villainous Joker.", 
                    8.0m, "18+", genres, "Drama", "Crime", "Thriller");

                AddMovie("Taxi Driver",
                    "The dim light of blind street lamps, the skeletal factory chimneys choking in their own smoke. " +
                    "The Babylonian towers of skyscrapers—all this is the hell of the New Age, New York. " +
                    "Vietnam veteran Travis Bickle drives his lone taxi through the nighttime streets of an endless city, a grim panorama of human sin unfolding before him. Like an Old Testament prophet, he hopes that one day the heavens will send a saving rain to cleanse New York of its centuries-old filth. " +
                    "And when hope dies, only hatred remains. A rain of fire will fall upon the heads of sinners. The taxi driver will see to that.", 
                    7.7m, "18+", genres, "Drama", "Crime", "Thriller");

                AddMovie("Se7en",
                    "Detective William Somerset is a veteran detective who dreams of retiring and getting away from the city and its sinful inhabitants. " +
                    "Seven days before retirement, Somerset is confronted with two troubles: his young partner, Mills, and a particularly sophisticated murder. " +
                    "The keen mind of the experienced investigator immediately determines that this crime is likely to be followed by others.", 
                    8.3m, "18+", genres, "Drama", "Crime", "Thriller");

                Core.Context.SaveChanges();
            }
            foreach (var movie in Core.Context.Movies)
            {
                movie.PosterPath = GetPosterPath(movie.Title);
            }

            Core.Context.SaveChanges();

            if (!Core.Context.Halls.Any())
            {
                Core.Context.Halls.AddRange(
                    new Hall { Name = "Hall 1", Rating = 9.2m },
                    new Hall { Name = "Hall 2", Rating = 8.5m },
                    new Hall { Name = "Hall 3", Rating = 9.0m }
                    );
                Core.Context.SaveChanges();
            }

            var halls = Core.Context.Halls.ToList();

            foreach (var hall in halls)
            {
                for (int row = 1; row <= 10; row++)
                {
                    for (int seat = 1; seat <= 24; seat++)
                    {
                        bool exists = Core.Context.Seats.Any(s =>
                            s.HallId == hall.Id &&
                            s.RowNumber == row &&
                            s.SeatNumber == seat);
                        if (!exists)
                        {
                            Core.Context.Seats.Add(new Seat
                            {
                                HallId = hall.Id,
                                RowNumber = row,
                                SeatNumber = seat
                            });
                        }
                    }
                }
            }
            Core.Context.SaveChanges();

            if (!Core.Context.Sessions.Any())
            {
                var movies = Core.Context.Movies.ToList();
                var sessionHalls = Core.Context.Halls.ToList();

                for(int i=0; i<movies.Count; i++)
                {
                    var hall = sessionHalls[i%sessionHalls.Count];
                    
                    Core.Context.Sessions.Add(new Session
                    {
                        MovieId = movies[i].Id,
                        HallId = hall.Id,
                        StartDateTime = DateTime.Today.AddDays(1+i%5).AddHours(11),
                        TicketPrice = 340
                    });

                    Core.Context.Sessions.Add(new Session
                    {
                        MovieId = movies[i].Id,
                        HallId = hall.Id,
                        StartDateTime = DateTime.Today.AddDays(1+i%5).AddHours(14),
                        TicketPrice = 590
                    });

                    Core.Context.Sessions.Add(new Session
                    {
                        MovieId = movies[i].Id,
                        HallId = hall.Id,
                        StartDateTime = DateTime.Today.AddDays(1+i%5).AddHours(18),
                        TicketPrice = 640
                    });

                    Core.Context.Sessions.Add(new Session
                    {
                        MovieId = movies[i].Id,
                        HallId = hall.Id,
                        StartDateTime = DateTime.Today.AddDays(1+i%5).AddHours(20),
                        TicketPrice = 699
                    });

                    Core.Context.Sessions.Add(new Session
                    {
                        MovieId = movies[i].Id,
                        HallId = hall.Id,
                        StartDateTime = DateTime.Today.AddDays(1+i%5).AddHours(22),
                        TicketPrice = 550
                    });
                }
            }
        }
        private static void AddMovie(string title, string description, decimal rating, string ageRating, Dictionary<string, Genre> genres, params string[] genreNames)
        {
            var movie = new Movie
            {
                Title = title,
                Description = description,
                Rating = rating,
                AgeRating = ageRating,
                StartDate = DateTime.Today.AddDays(-7),
                PosterPath = GetPosterPath(title)
            };
            foreach(var genreName in genreNames)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    Genre = genres[genreName]
                });
            }
            Core.Context.Movies.Add(movie);
        }
        private static string GetPosterPath(string title) 
        {
            switch (title)
            {
                case "Fight Club": return "/Images/fightclub.jpg";
                case "Green Book": return "/Images/greenbook.jpg";
                case "Real Steel": return "/Images/realsteel.jpg";
                case "Drive": return "/Images/drive.jpg";
                case "Fast & Furious": return "/Images/fastfurious.jpg";
                case "John Wick": return "/Images/johnwick.jpg";
                case "The Wolf of Wall Street": return "/Images/wolfofwallstreet.jpg";
                case "Joker": return "/Images/joker.jpg";
                case "Taxi Driver": return "/Images/taxidriver.jpg";
                case "Se7en": return "/Images/seven.jpg";
                default: return "";
            }
        }
    }
}
