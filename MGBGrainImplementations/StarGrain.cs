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
    /// Orleans grain implementation class StarGrain
    /// </summary>
    public class StarGrain : Orleans.Grain, MGBGrains.IStarGrain
    {
        private string _name;
        private IGameGrain _game;
        private IPlayerGrain _owner;

        public StarGrain()
        {
            _name = "UNNAMED";
        }

        public Task Initialize(IGameGrain game, string name)
        {
            _game = game;
            _name = name;
//            Console.WriteLine(" -- {0,-10} -- {1} initialized.", "Star", _name);
            return TaskDone.Done;
        }

        public Task ChangeOwnership(IPlayerGrain player)
        {
            _owner = player;
            Console.WriteLine(" -- {0,-10} -- {1} changed hands.", "Star", _name);
            return TaskDone.Done;
        }
    }
}
