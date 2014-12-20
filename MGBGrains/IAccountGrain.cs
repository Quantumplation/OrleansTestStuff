using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace MGBGrains
{
    /// <summary>
    /// Orleans grain communication interface IAccountGrain
    /// </summary>
	public interface IAccountGrain : Orleans.IGrain
    {
        Task SetUsername(string username);
        Task JoinGame(IGameGrain game, IPlayerGrain player);
    }
}
