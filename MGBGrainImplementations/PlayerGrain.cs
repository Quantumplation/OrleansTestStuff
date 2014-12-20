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
    /// Orleans grain implementation class PlayerGrain
    /// </summary>
    public class PlayerGrain : Orleans.Grain, MGBGrains.IPlayerGrain
    {
        private string _name;
        private IAccountGrain _accountGrain;
        private IGameGrain _game;
        private List<IStarGrain> _stars;

        public PlayerGrain()
        {
            _name = "UNNAMED";
        }

        public Task<string> Name()
        {
            return Task.FromResult(_name);
        }

        public Task Create(IGameGrain game, IAccountGrain account, string name)
        {
            _game = game;
            _accountGrain = account;
            _name = name;
            _stars = new List<IStarGrain>();
            Console.WriteLine(" -- {0,-10} -- {1} created.", "Player", _name);
            return TaskDone.Done;
        }

        public Task AcquireStar(IStarGrain star)
        {
            _stars.Add(star);
            Console.WriteLine(" -- {0,-10} -- {1} acquired a star.", "Player", _name);
            return TaskDone.Done;
        }
    }
}
