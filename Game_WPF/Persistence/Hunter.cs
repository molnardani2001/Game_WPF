using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_WPF.Persistence
{
    public enum Direction { Up, Down, Left, Right, NoDirection }

    public class Hunter
    {
        #region fields

        private int _coordX; // x-koordináta
        private int _coordY; // y-koordináta
        private Direction _direction; //irány

        #endregion

        #region properties
        public int CoordX { get { return _coordX; } }

        public int CoordY { get { return _coordY; } }

        public Direction Direction { get { return _direction; } }

        #endregion

        #region events

        public event EventHandler<MoveEventArgs> OnHunterMove;

        #endregion

        #region constructors
        public Hunter(int coordX, int coordY, Direction direction)
        {
            _coordX = coordX;
            _coordY = coordY;
            _direction = direction;
            
        }
        #endregion

        #region methods
        //lépteti egyel a vadászt, ha nem fára vagy a pályán kívűlre lépne
        public void Move(List<Coord> trees, int size)
        {
            switch (_direction)
            {
                case Direction.Up:
                    if (_coordY > 0 && !trees.Contains(new Coord(_coordX,_coordY-1)))
                    {
                        if(OnHunterMove != null)
                        {
                            OnHunterMove(this, new MoveEventArgs(new Coord(_coordX, _coordY - 1)));
                        }
                        _coordY--;
                    } else
                    {
                        TurnAround();
                    }
                    break;
                case Direction.Down:
                    if (_coordY < size - 1 && !trees.Contains(new Coord(_coordX,_coordY + 1)))
                    {   if(OnHunterMove != null)
                        {
                            OnHunterMove(this, new MoveEventArgs(new Coord(_coordX, _coordY + 1)));
                        }
                        
                        _coordY++;
                    }
                    else
                    {
                        TurnAround();
                    }
                    break;
                case Direction.Left:
                    if (_coordX > 0 && !trees.Contains(new Coord(_coordX - 1, _coordY)))
                    {
                        if(OnHunterMove != null)
                        {
                            OnHunterMove(this, new MoveEventArgs(new Coord(_coordX - 1, _coordY)));
                        }
                        
                        _coordX--;
                    }
                    else
                    {
                        TurnAround();
                    }
                    break;
                case Direction.Right:
                    if (_coordX < size - 1 && !trees.Contains(new Coord(_coordX + 1, _coordY)))
                    {
                        if(OnHunterMove != null)
                        {
                            OnHunterMove(this, new MoveEventArgs(new Coord(_coordX + 1, _coordY)));
                        }
                        
                        _coordX++;
                    }
                    else
                    {
                        TurnAround();
                    }
                    break;
            }
        }

        //ha olyan mezőre lépne amire nem szabad akkor visszafordul
        private void TurnAround()
        {
            switch (_direction)
            {
                case Direction.Up:
                    _direction = Direction.Down;
                    if(OnHunterMove != null)
                    {
                        OnHunterMove(this, new MoveEventArgs(new Coord(_coordX, _coordY + 1)));
                    }
                    
                    _coordY++;
                    break;
                case Direction.Down:
                    _direction = Direction.Up;
                    if(OnHunterMove != null)
                    {
                        OnHunterMove(this, new MoveEventArgs(new Coord(_coordX, _coordY - 1)));
                    }
                    
                    _coordY--;
                    break;
                case Direction.Left:
                    _direction = Direction.Right;
                    if(OnHunterMove != null)
                    {
                        OnHunterMove(this, new MoveEventArgs(new Coord(_coordX + 1, _coordY)));
                    }
                    
                    _coordX++;
                    break;
                case Direction.Right:
                    _direction = Direction.Left;
                    if(OnHunterMove != null)
                    {
                        OnHunterMove(this, new MoveEventArgs(new Coord(_coordX - 1, _coordY)));
                    }
                    
                    _coordX--;
                    break;
            }
        }
        #endregion

        #region override methods
        public override bool Equals(object obj)
        {
            if(obj is Hunter)
            {
                var that = obj as Hunter;
                return that.CoordX == this.CoordX && that.CoordY == this.CoordY;
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
