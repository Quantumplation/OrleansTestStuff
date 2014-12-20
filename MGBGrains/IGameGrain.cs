using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace MGBGrains
{
    public enum GameState
    {
        Unstarted,
        InProgress,
        Finished
    }

    /// <summary>
    /// Orleans grain communication interface IGameGrain
    /// </summary>
	public interface IGameGrain : Orleans.IGrain
    {
        Task<string> Name();
        Task Rename(string name);
        Task<IEnumerable<IPlayerGrain>> Players(); 
        Task<IPlayerGrain> NewPlayer(IAccountGrain player, string name);
        Task GenerateMap(int starCount);
        Task StartGame();
    }
}
