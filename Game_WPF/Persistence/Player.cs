using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_WPF.Persistence
{
    public class Player
    {
        #region fields

        private int _x; // x-koordináta
        private int _y; // y-koordináta

        #endregion

        #region properties
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        #endregion

        #region events
        //esemény a játékos mozgásához
        public event EventHandler<MoveEventArgs> OnPlayerMove;
        #endregion

        #region constructors
        public Player(int xCoord, int yCoord)
        {
            _x = xCoord;
            _y = yCoord;
        }
        #endregion

        #region methods
        //a játékos lépése az adott direction-ba ha nem ütközik akadályba (fa), vagy a pálya szélébe
        public void Move(Direction direction, List<Coord> trees, int size)
        {
            switch (direction)
            {
                case Direction.Up:
                    if(_y > 0 && !trees.Contains(new Coord(_x, _y-1)))
                    {
                        if(OnPlayerMove != null)
                        {
                            OnPlayerMove(this, new MoveEventArgs(new Coord(_x, _y - 1)));
                        }
                        
                        _y = _y - 1;
                    }//különben ne változzon semmi
                    break;
                case Direction.Down:
                    if (_y < size - 1 && !trees.Contains(new Coord(_x, _y + 1)))
                    {
                        if (OnPlayerMove != null)
                        { 
                            OnPlayerMove(this, new MoveEventArgs(new Coord(_x, _y + 1)));
                        }
                        _y = _y + 1 ;
                    }//különben ne változzon semmi
                    break;
                case Direction.Left:
                    if(_x > 0 && !trees.Contains(new Coord(_x-1,_y)))
                    {
                        if(OnPlayerMove != null)
                        {
                            OnPlayerMove(this, new MoveEventArgs(new Coord(_x - 1, _y)));
                        }
                        _x = _x - 1;
                    }//különben ne változzon semmi
                    break;
                case Direction.Right:
                    if(_x < size - 1 && !trees.Contains(new Coord(_x+1, _y)))
                    {
                        if(OnPlayerMove != null)
                        {
                            OnPlayerMove(this, new MoveEventArgs(new Coord(_x + 1, _y)));
                        }
                        
                        _x = _x + 1;
                    }//különben ne változzon semmi
                    break;
            }
        }
        #endregion
    }
}
