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
    /// Orleans grain implementation class GameGrain
    /// </summary>
    public class GameGrain : Orleans.Grain, MGBGrains.IGameGrain
    {
        private string _name;
        private GameState _state;
        private Dictionary<Guid, IPlayerGrain> _players;
        private List<IStarGrain> _stars;

        public GameGrain()
        {
            _players = new Dictionary<Guid, IPlayerGrain>();
            _stars = new List<IStarGrain>();
            _name = "UNNAMED";
        }

        public Task<string> Name()
        {
            return Task.FromResult(_name);
        }

        public Task Rename(string name)
        {
            Console.WriteLine(" -- {0,-10} -- {1} renamed to {2}.", "Game", _name, name);
            _name = name;
            return TaskDone.Done;
        }

        public Task<IEnumerable<IPlayerGrain>> Players()
        {
            return Task.FromResult((IEnumerable<IPlayerGrain>) _players.Values);
        } 

        public Task GenerateMap(int starCount)
        {
            if(_state != GameState.Unstarted)
                throw new InvalidOperationException("Can only generate maps on unstarted games.");

            for (var x = 0; x < starCount; x++)
            {
                var star = StarGrainFactory.GetGrain(Guid.NewGuid());
                star.Initialize(this, "RandomStar");
                _stars.Add(star);
            }
            Console.WriteLine(" -- {0,-10} -- Map generated for {1}.", "Game", _name);
            return TaskDone.Done;
        }

        public Task<IPlayerGrain> NewPlayer(IAccountGrain account, string name)
        {
            if (_state == GameState.Unstarted)
            {
                var accountKey = account.GetPrimaryKey();
                IPlayerGrain playerGrain;
                if (_players.TryGetValue(accountKey, out playerGrain)) return Task.FromResult(playerGrain);

                var playerGuid = Guid.NewGuid(); 
                playerGrain = PlayerGrainFactory.GetGrain(playerGuid);
                playerGrain.Create(this, account, name);
                _players.Add(accountKey, playerGrain);
                account.JoinGame(this, playerGrain);

                Console.WriteLine(" -- {0,-10} -- New player joined {1}.", "Game", _name);
                return Task.FromResult(playerGrain);
            }
            else
            {
                Console.WriteLine(" -- {0,-10} -- Cannot join a started game.", "Game");
                return Task.FromResult<IPlayerGrain>(null);
            }
        }

        public Task StartGame()
        {
            _state = GameState.InProgress;

            var usedStars = _stars.Count*0.25;
            var starsPerPlayer = (int)(usedStars / _players.Count);
            Console.WriteLine(" -- {0,-10} -- {1} stars per player.", "Game", starsPerPlayer);
            var offset = 0;
            foreach (var player in _players.Values)
            {
                foreach (var star in _stars.Skip(offset).Take(starsPerPlayer))
                {
                    star.ChangeOwnership(player);
                    player.AcquireStar(star);
                    offset++;
                }
            }
            Console.WriteLine(" -- {0,-10} -- {1} started.", "Game", _name);
            GameListGrainFactory.GetGrain(0).ChangeState(this, GameState.Unstarted, GameState.InProgress);
            return TaskDone.Done;
        }
    }
}
