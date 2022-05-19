using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_WPF.Persistence;

namespace Game_WPF.Model
{
    public class GameOverEventArgs : EventArgs
    {
        private bool _isWon;
        private Hunter _hunter;

        public bool IsWon { get { return _isWon; } }
        public Hunter Hunter { get { return _hunter; } }

        public GameOverEventArgs(bool isWon, Hunter hunter)
        {
            _isWon = isWon;
            _hunter = hunter;
        }
    }
}
