using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_WPF.Persistence
{
    public class Coord
    {
        #region fields
        private int _x; // x-koordináta
        private int _y; // y-koordináta
        #endregion

        #region properties
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        #endregion

        #region constructors
        public Coord(int x, int y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region override methods
        public override bool Equals(object obj)
        {
            if(obj is Coord)
            {
                var that = obj as Coord;
                return this.X == that.X && this.Y == that.Y;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
