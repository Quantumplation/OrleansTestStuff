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
    /// Orleans grain implementation class GameListGrain
    /// </summary>
    public class GameListGrain : Orleans.Grain, MGBGrains.IGameListGrain
    {
        private Dictionary<Guid, IGameGrain> _unstarted;  
        private Dictionary<Guid, IGameGrain> _inProgress;  
        private Dictionary<Guid, IGameGrain> _finished;

        public GameListGrain()
        {
            _unstarted = new Dictionary<Guid, IGameGrain>();
            _inProgress = new Dictionary<Guid, IGameGrain>();
            _finished = new Dictionary<Guid, IGameGrain>();
        }

        public Task NewGame(IGameGrain game)
        {
            var gameId = game.GetPrimaryKey();
            _unstarted.Add(gameId, game);
            return TaskDone.Done;
        }

        public Task ChangeState(IGameGrain game, GameState oldState, GameState newState)
        {
            var gameId = game.GetPrimaryKey();
            switch (oldState)
            {
                case GameState.Unstarted:
                    _unstarted.Remove(gameId);
                    break;
                case GameState.InProgress:
                    _inProgress.Remove(gameId);
                    break;
                case GameState.Finished:
                    _finished.Remove(gameId);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            switch (newState)
            {
                case GameState.Unstarted:
                    _unstarted.Add(gameId, game);
                    break;
                case GameState.InProgress:
                    _inProgress.Add(gameId, game);
                    break;
                case GameState.Finished:
                    _finished.Add(gameId, game);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return TaskDone.Done;
        }

        public Task<IEnumerable<IGameGrain>> Unstarted()
        {
            return Task.FromResult((IEnumerable<IGameGrain>) _unstarted.Values);
        }

        public Task<IEnumerable<IGameGrain>> InProgress()
        {
            return Task.FromResult((IEnumerable<IGameGrain>) _inProgress.Values);
        }

        public Task<IEnumerable<IGameGrain>> Finished()
        {
            return Task.FromResult((IEnumerable<IGameGrain>)_finished.Values);
        }
    }
}
