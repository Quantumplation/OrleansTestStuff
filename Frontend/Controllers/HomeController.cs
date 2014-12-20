using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frontend.Models;
using MGBGrains;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {
            var vm = new GameListViewModel();
            var grain = GameListGrainFactory.GetGrain(0);
            foreach (var game in await grain.Unstarted())
            {
                var gamevm = new GameListViewModel.GameViewModel
                {
                    Name = await game.Name(),
                    Players = new List<string>()
                };
                var players = await game.Players();
                foreach (var player in players)
                {
                    gamevm.Players.Add(await player.Name());
                }
                vm.Games.Add(gamevm);
            }
            return View(vm);
        }
    }
}