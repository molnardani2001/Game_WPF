using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_WPF.Persistence
{
    public class MoveEventArgs : EventArgs
    {
        #region fields
        private Coord _nextPosition; //a bábu következő pozíciója
        #endregion

        #region properties
        public Coord NextPosition { get { return _nextPosition; } }
        #endregion

        #region constructors
        public MoveEventArgs(Coord nextPosition)
        {
            _nextPosition = nextPosition;
        }
        #endregion
    }
}
