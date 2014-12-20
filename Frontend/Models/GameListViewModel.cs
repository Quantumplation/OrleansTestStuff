using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class GameListViewModel
    {
        public class GameViewModel
        {
            public string Name;
            public List<string> Players;
        }
        public List<GameViewModel> Games = new List<GameViewModel>();
    }
}