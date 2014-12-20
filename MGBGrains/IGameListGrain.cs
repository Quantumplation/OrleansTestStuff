using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace MGBGrains
{
    /// <summary>
    /// Orleans grain communication interface IGameListGrain
    /// </summary>
	public interface IGameListGrain : Orleans.IGrain
    {
        Task NewGame(IGameGrain grain);
        Task ChangeState(IGameGrain grain, GameState oldState, GameState newState);

        Task<IEnumerable<IGameGrain>> Unstarted();
        Task<IEnumerable<IGameGrain>> InProgress();
        Task<IEnumerable<IGameGrain>> Finished();
    }
}
