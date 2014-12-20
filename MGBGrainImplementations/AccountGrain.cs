using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using MGBGrains;
using Orleans;

namespace MGBGrainImplementations
{
    /// <summary>
    /// Orleans grain implementation class AccountGrain
    /// </summary>
    public class AccountGrain : Orleans.Grain, MGBGrains.IAccountGrain
    {
        private string _username;
        private IDictionary<Guid, IGameGrain> _activeGames;  
        private IDictionary<Guid, IPlayerGrain> _activePlayers;

        public AccountGrain()
        {
            _activeGames = new Dictionary<Guid, IGameGrain>();
            _activePlayers = new Dictionary<Guid, IPlayerGrain>();
            _username = "UNNAMED";
        }

        public Task SetUsername(string username)
        {
            Console.WriteLine(" -- {0,-10} -- {1} changed username to {2}.", "Account", _username, username);
            _username = username;
            return TaskDone.Done;
        }

        public Task JoinGame(IGameGrain game, IPlayerGrain player)
        {
            var primaryKey = game.GetPrimaryKey();
            if (_activeGames.ContainsKey(primaryKey)) return TaskDone.Done;
            _activeGames.Add(primaryKey, game);
            _activePlayers.Add(primaryKey, player);
            Console.WriteLine(" -- {0,-10} -- {1} joined a game.", "Account", _username);
            return TaskDone.Done;
        }
    }
}
