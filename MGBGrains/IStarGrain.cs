using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace MGBGrains
{
    /// <summary>
    /// Orleans grain communication interface IStarGrain
    /// </summary>
	public interface IStarGrain : Orleans.IGrain
    {
        Task Initialize(IGameGrain game, string name);
        Task ChangeOwnership(IPlayerGrain player);
    }
}
