using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using SimpleApp.Models;
using SimpleApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FootballDataService _footballDataService;
        public List<Match> Matches { get; set; }
        public DateTime UtcDate { get; set; }


        public IndexModel(FootballDataService footballDataService)
        {
            _footballDataService = footballDataService;
        }

        public async Task OnGetAsync(DateTime? datetime)
        {
            // Om datum skickas in via formul�ret anv�nds det, annars dagens datum
            DateTime startDate = datetime ?? DateTime.Now.Date;

            // Spara startdatumet f�r att kunna visa det igen i formul�ret
            UtcDate = startDate;

            // Skapa ett intervall f�r dagens matcher
            DateTime endDate = startDate.AddDays(1);
            string dateFrom = startDate.ToString("yyyy-MM-dd");
            string dateTo = endDate.ToString("yyyy-MM-dd");

            // H�mta bara matcher fr�n de valda ligorna
            var selectedLeagues = new List<string> {"PL","CL"};
            Matches = await _footballDataService.GetMatchesAsync(dateFrom, dateTo, selectedLeagues);

            // Konvertera matchtider och ber�kna odds
            foreach (var match in Matches)
            {
                match.UtcDate = TimeZoneInfo.ConvertTimeFromUtc(match.UtcDate, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
                match.Odds = CalculateOdds(match.HomeTeam, match.AwayTeam);
            }
        }

        public Odds CalculateOdds(Team homeTeam, Team awayTeam)
        {
            
            bool isHomeFavorite = homeTeam.TeamRatingScale > awayTeam.TeamRatingScale;

            // Om det �r en j�mn match
            if (Math.Abs(homeTeam.TeamRatingScale - awayTeam.TeamRatingScale) <= 1)
            {
                return new Odds
                {
                    HomeWin = 2.0,
                    Draw = 4.0,
                    AwayWin = 3.0
                };
            }

            // Om hemmalaget �r favoriten
            if (isHomeFavorite)
            {
                return new Odds
                {
                    HomeWin = 2.0,  
                    Draw = 3.0,
                    AwayWin = 5.0   
                };
            }
            else
            {   
                // Om bortalaget �r favoriten
                return new Odds
                {
                    HomeWin = 5.0,  
                    Draw = 4.0,
                    AwayWin = 3.0   
                };
            }
            
    
        }
        


    }
}

    



