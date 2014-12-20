using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace MGBGrains
{
    /// <summary>
    /// Orleans grain communication interface IPlayerGrain
    /// </summary>
	public interface IPlayerGrain : Orleans.IGrain
    {
        Task<string> Name();
        Task Create(IGameGrain game, IAccountGrain account, string name);
        Task AcquireStar(IStarGrain star);
    }
}
