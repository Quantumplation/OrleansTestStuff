using System;
using System.Collections.Generic;
using MGBGrains;

namespace MGBOrleansTest
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });

            Orleans.OrleansClient.Initialize("DevTestClientConfiguration.xml");
            var gameGrain = MGBGrains.GameGrainFactory.GetGrain(Guid.NewGuid());
            var gameList = MGBGrains.GameListGrainFactory.GetGrain(0);
            gameList.NewGame(gameGrain);
            var accounts = new Dictionary<string, IAccountGrain>
            {
                ["Quantumplation"] = MGBGrains.AccountGrainFactory.GetGrain(Guid.NewGuid()),
                ["Nyarlathothep"] = MGBGrains.AccountGrainFactory.GetGrain(Guid.NewGuid()),
                ["SuperNaviX"] = MGBGrains.AccountGrainFactory.GetGrain(Guid.NewGuid()),
                ["Pigyman"] = MGBGrains.AccountGrainFactory.GetGrain(Guid.NewGuid()),
            };
            var players = new Dictionary<string, IPlayerGrain>();

            gameGrain.Rename("First Game").Wait();
            gameGrain.GenerateMap(150);
            foreach (var account in accounts)
            {
                Console.WriteLine("Press any key to add next player...");
                Console.ReadLine();
                account.Value.SetUsername(account.Key).Wait();
                var player = gameGrain.NewPlayer(account.Value, account.Key).Result;
                players.Add(account.Key, player);
            }

            Console.WriteLine("Press Any key to start the game...");
            Console.ReadLine();

            gameGrain.StartGame().Wait();

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            hostDomain.DoCallBack(ShutdownSilo);
        }

        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }

        private static OrleansHostWrapper hostWrapper;
    }
}
