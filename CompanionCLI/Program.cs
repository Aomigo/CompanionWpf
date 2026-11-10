using CompanionCLI.Entities;
using CompanionCLI.Repositories;
using CompanionCLI.Services;
using Spectre.Console;
using System.Text;
using System.Globalization;
namespace companion;


public class FestivalCompanion
{
    Budget budget = new(0.0m);
    JsonService jsonService = new();
    InMemoryArtistRepository ArtistRepository;
    InMemoryConcertRepository ConcertRepository;
    ConcertService concertService;
    GlobalMerger globalFile = new();

    DateTime festivalStart = DateTime.Now;
    DateTime festivalEnd = DateTime.Now;
    int festivalDuration = 5;

    

    public FestivalCompanion()
    {
        ArtistRepository = new();
        ConcertRepository = new();
        concertService = new ConcertService(ConcertRepository);
    }
    private void ConcertQuestions()
    {
        var artist = AnsiConsole.Prompt(
            new SelectionPrompt<Artist>()
                .Title("Who will be the performing artist?")
                .UseConverter(artist => artist.Name)
                .AddChoices(ArtistRepository.GetAll()));

        var scene = AnsiConsole.Prompt(
            new TextPrompt<int>($"On which scene will [green]{artist.Name}[/] perform?")
                .Validate(scene => scene > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Scene must be greater than 0[/]")));

        var concertStart = AnsiConsole.Prompt(
            new TextPrompt<DateTime>("When will the concert start?")
                .PromptStyle("green"));

        var concertEnd = AnsiConsole.Prompt(
            new TextPrompt<DateTime>("When will the concert end?")
                .PromptStyle("green")
                .Validate(end =>
                    end > concertStart
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]End time must be after start time[/]")));

        var price = Math.Round((decimal)(concertEnd - concertStart).TotalHours * artist.PricePerHour);
        AnsiConsole.MarkupLine(
            $"[gold1]The price of the concert will be[/] [green]{price}€[/]");


        var verif = concertService.CanScheduleConcert(concertStart, concertEnd, scene, festivalStart, festivalEnd);
        if (!verif)
        {
            AnsiConsole.MarkupLine(
                "[red]❌ The concert has been canceled due to management errors[/]");
        } 
        else if (!budget.RemoveFunds(price))
        {
            AnsiConsole.MarkupLine(
                "[red]💰 The concert has been canceled due to a financial problem[/]");
        }
        else
        {
            concertService.AddConcert(artist, concertStart, concertEnd, price, scene, festivalStart, festivalEnd);

            AnsiConsole.MarkupLine(
                $"[green]✅ {artist.Name}[/] will sing " +
                $"[yellow]{(concertStart.Date == DateTime.Now.Date ? "Today" : concertStart.ToString("MM/dd/yy"))}[/] " +
                $"at [deepskyblue1]{concertStart.ToString("hh:mm tt", CultureInfo.InvariantCulture)}[/] " +
                $"for [gold1]{Math.Round((concertEnd - concertStart).TotalHours)}h[/], " +
                $"on [orchid]scene n°{scene}[/].");
        }
        //AN ARTIST WILL NEED TO REST FOR 6H + 1/2h OF THE PREVIOUS CONCERT BEFORE PERFORMING AGAIN
    }

    public void GetPlanningSetup()
    {
        Console.Clear();
        var currentDay = (DateTime.Now - festivalStart).Days + 1;
        DateOnly date = DateOnly.FromDateTime(festivalStart);

        var tree = new Tree($"[bold deepskyblue1]Festival Planning[/] - Day [gold1]{currentDay}[/]")
            .Style(Style.Parse("bold"));

        for (int i = 0; i < festivalDuration; i++)
        {
            var dayI = tree.AddNode($"[yellow]Day {i + 1}[/] ([HoneyDew2]{date.AddDays(i)}[/])");

            foreach (var scene in concertService.GetSchedule(date.AddDays(i)))
            {
                var scenery = dayI.AddNode($"[DeepSkyBlue1]🎤 Scene n°{scene.Key}[/]");
                foreach (Concert concert in scene)
                {
                    var color =
                        concert.ConcertStart > DateTime.Now ? "SpringGreen3": 
                        concert.ConcertEnd > DateTime.Now ? "Gold1"
                        : "IndianRed1";
                    bool inAction = color == "Gold1";
                    scenery.AddNode(
                        $"[{color}]•[/] [white]{concert.Artist.Name}[/] " +
                        $"{(inAction ? "until" : "at")} [{color}]{(inAction ? concert.ConcertEnd : concert.ConcertStart).ToString("hh:mm tt", CultureInfo.InvariantCulture)}[/]" +
                        $" ([LightGoldenrod1]{concert.ConcertPrice}€[/])");
                }
            }
        }
        AnsiConsole.Write(tree);
        AnsiConsole.MarkupLine("[SpringGreen3]• Announced[/]  [Gold1]• In progress[/]  [IndianRed1]• Over[/]");
    }

    public async Task Instantiate()
    {
        //Check if json exist and not empty
        if (!jsonService.Exists() || jsonService.EmptyFile())
        {
            Console.MarkupLine("How much time will the festival last ?");
                if (int.TryParse(Console.ReadLine(), out var answer))
                {
                    festivalStart = DateTime.Now;
                    festivalDuration = answer;
                    festivalEnd = festivalStart.AddDays(festivalDuration);
                }
        }
        else
        {
            if(JsonService.IsCorrupted())
            {
                AnsiConsole.MarkupLine("[red]Save file is corrupted[/]");
            }
            else
            {
                globalFile = JsonService.Load();
                festivalStart = globalFile.FestivalStart;
                festivalEnd = globalFile.FestivalEnd;
                festivalDuration = (int)(festivalEnd - festivalStart).TotalDays;
                budget.AddFunds(globalFile.Budget);
            }

            foreach(Concert concert in globalFile.Concerts)
            {
                ConcertRepository.Add(concert);
            }
            
        }//Make sure to also do budget
    }

    public void FestivalStats()
    {
        Console.Clear();

        var concerts = ConcertRepository.GetAll().ToList();

        if (!concerts.Any())
        {
            AnsiConsole.MarkupLine("[red]No concerts were held during the festival.[/]");
            return;
        }

        var totalSpent = concerts.Sum(c => c.ConcertPrice);

        var mostExpensive = concerts
            .OrderByDescending(c => c.ConcertPrice)
            .First();

        var mostBookedArtist = concerts
            .GroupBy(c => c.Artist.Name)
            .OrderByDescending(g => g.Count())
            .First();

        var averageDuration = concerts
            .Average(c => (c.ConcertEnd - c.ConcertStart).TotalHours);

        var uniqueArtists = concerts
            .Select(c => c.Artist.Name)
            .Distinct()
            .Count();

        var favoriteArtist = concerts
            .Select(c => c.Artist)
            .FirstOrDefault(a => a.isFavorite);

        AnsiConsole.Write(
            new Rule("[gold1]Festival Statistics[/]")
            .Centered());

        AnsiConsole.MarkupLine(
            $"🎵 Concerts organized: [green]{concerts.Count}[/]");

        AnsiConsole.MarkupLine(
            $"🎤 Artists booked: [green]{uniqueArtists}[/]");

        AnsiConsole.MarkupLine(
            $"💰 Total spent: [gold1]{totalSpent:N0}€[/]");

        AnsiConsole.MarkupLine(
            $"🏆 Most booked artist: [deepskyblue1]{mostBookedArtist.Key}[/] ({mostBookedArtist.Count()} concerts)");

        AnsiConsole.MarkupLine(
            $"💎 Most expensive concert: [orchid]{mostExpensive.Artist.Name}[/] ({mostExpensive.ConcertPrice:N0}€)");

        AnsiConsole.MarkupLine(
            $"⏱ Average concert duration: [yellow]{averageDuration:F1}h[/]");

        AnsiConsole.MarkupLine(
            $"🏦 Remaining budget: [green]{budget.Fund:N0}€[/]");

        var ranking = concerts
            .GroupBy(c => c.Artist.Name)
            .OrderByDescending(g => g.Count());
        AnsiConsole.MarkupLine("\n[bold]Artist Ranking[/]");
        foreach (var artist in ranking)
        {
            AnsiConsole.MarkupLine(
                $"{artist.Count(),2}x [green]{artist.Key}[/]");
        }

        if (favoriteArtist is not null)
        {
            var favoriteConcerts = concerts.Count(c => c.Artist.isFavorite);

            AnsiConsole.MarkupLine(
                $"⭐ Favorite artist: [gold1]{favoriteArtist.Name}[/] ({favoriteConcerts} concert(s))");
        }
    }


    private void MainLoop()
    {
        Console.Clear();

        AnsiConsole.Write(
            new Rule("[#ffbc70]Festival Companion[/]")
                .RuleStyle("DarkSlateGray3")
                .Centered());

        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[#ffbc70]📋Centre d'utilisation[/]");
        AnsiConsole.MarkupLine(
            "[yellow]1[/]) Add a concert\n" +
            "[yellow]2[/]) Get daily scene schedule \n" +
            "[yellow]3[/]) Change Favorite artist\n" +
            "[yellow]4[/]) Add funding\n" +
            "[yellow]5[/]) Save and leave");

        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine($"💰 Budget: [gold1]{budget.Fund:N0}€[/]");

        AnsiConsole.Markup("[#ffbc70]> Votre choix : [/]");
        var answer = Console.ReadLine();
        switch (answer)
        {
            case "1":
                ConcertQuestions();
                break;
            case "2":
                GetPlanningSetup();
                break;
            case "3":
                // Add/Change favorite
                break;
            case "4":
                budget.AddFunds(2000.0m);
                break;
            case "5":
                //Save before exiting
                globalFile.Budget = budget.Fund;
                globalFile.FestivalStart = festivalStart;
                globalFile.FestivalEnd = festivalEnd;
                globalFile.Concerts = [];
                foreach(Concert concert in ConcertRepository.GetAll())
                {
                    globalFile.Concerts.Add(concert);
                }
                jsonService.Save(globalFile);
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Merci de répondre par 1,2,3,4 ou 5");
                break;

        }
        var awaitPrompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Ok !"));

    }
    public static async Task Main()
    {
        FestivalCompanion companion = new();
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        await companion.Instantiate();
        DateTime end = companion.festivalEnd;

        while (!companion.concertService.FestivalEnded(end))
        {
            companion.MainLoop();
        }
        companion.FestivalStats();

        //1 fav artist, -10% on his concert
    }
};
