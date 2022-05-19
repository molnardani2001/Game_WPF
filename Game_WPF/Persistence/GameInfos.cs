using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_WPF.Persistence
{
    public class GameInfos
    {
        #region fields

        private List<Coord> _baskets; // kosarak koordinátái
        private List<Coord> _trees; // fák koordinátái
        private List<Hunter> _hunters; // vadászok koordinátái
        private int _tableSize; // pálya mérete
        private Coord _playerCoords; // játékos koordinátái
        private double _elapsedSeconds; // eltelt másodpercek

        #endregion

        #region properties
        public List<Coord> Baskets { get { return _baskets; } }
        public List<Coord> Trees { get { return _trees; } }
        public List<Hunter> Hunters { get { return _hunters; } }

        public int TableSize { get { return _tableSize; } }

        public Coord PlayerCoords { get { return _playerCoords; } }

        public double ElapsedSeconds { get { return _elapsedSeconds; } }

        #endregion

        #region constructors

        //konstruktor a mockolt táblához
        public GameInfos(int tableSize)
        {
            switch (tableSize)
            {
                case 12:
                    #region add manually baskets
                    _baskets = new List<Coord>();
                    _baskets.Add(new Coord(1, 6));
                    _baskets.Add(new Coord(4, 1));
                    _baskets.Add(new Coord(5, 11));
                    _baskets.Add(new Coord(7, 3));
                    _baskets.Add(new Coord(9, 11));
                    _baskets.Add(new Coord(11, 6));
                    #endregion
                    #region add manually trees
                    _trees = new List<Coord>();
                    _trees.Add(new Coord(2, 1));
                    _trees.Add(new Coord(9, 1));
                    _trees.Add(new Coord(6, 5));
                    _trees.Add(new Coord(7, 5));
                    _trees.Add(new Coord(10, 9));
                    _trees.Add(new Coord(3, 8));
                    #endregion
                    #region add manually hunters
                    _hunters = new List<Hunter>();
                    _hunters.Add(new Hunter(2, 3, Direction.Right));
                    _hunters.Add(new Hunter(2, 10, Direction.Up));
                    _hunters.Add(new Hunter(5, 4, Direction.Left));
                    _hunters.Add(new Hunter(6, 7, Direction.Up));
                    _hunters.Add(new Hunter(7, 8, Direction.Left));
                    _hunters.Add(new Hunter(11, 1, Direction.Down));
                    #endregion
                    _tableSize = tableSize;
                    _playerCoords = new Coord(0, 0);
                    _elapsedSeconds = 0.0;
                    break;
                case 18:
                    #region add manually baskets
                    _baskets = new List<Coord>();
                    _baskets.Add(new Coord(1, 5));
                    _baskets.Add(new Coord(1, 10));
                    _baskets.Add(new Coord(2, 16));
                    _baskets.Add(new Coord(7, 10));
                    _baskets.Add(new Coord(7, 14));
                    _baskets.Add(new Coord(14, 7));
                    _baskets.Add(new Coord(14, 11));
                    _baskets.Add(new Coord(16, 2));
                    _baskets.Add(new Coord(16, 17));
                    #endregion
                    #region add manually trees
                    _trees = new List<Coord>();
                    _trees.Add(new Coord(2, 8));
                    _trees.Add(new Coord(4, 12));
                    _trees.Add(new Coord(6, 2));
                    _trees.Add(new Coord(7, 3));
                    _trees.Add(new Coord(8, 2));
                    _trees.Add(new Coord(10, 9));
                    _trees.Add(new Coord(11, 8));
                    _trees.Add(new Coord(13, 14));
                    _trees.Add(new Coord(14, 15));
                    #endregion
                    #region add manually hunters
                    _hunters = new List<Hunter>();
                    _hunters.Add(new Hunter(2, 17, Direction.Right));
                    _hunters.Add(new Hunter(4, 4, Direction.Down));
                    _hunters.Add(new Hunter(4, 14, Direction.Left));
                    _hunters.Add(new Hunter(5, 8, Direction.Left));
                    _hunters.Add(new Hunter(8, 5, Direction.Down));
                    _hunters.Add(new Hunter(10, 2, Direction.Right));
                    _hunters.Add(new Hunter(11, 12, Direction.Up));
                    _hunters.Add(new Hunter(13, 4, Direction.Down));
                    _hunters.Add(new Hunter(14, 14, Direction.Right));
                    #endregion
                    _tableSize = tableSize;
                    _playerCoords = new Coord(0, 0);
                    _elapsedSeconds = 0.0;
                    break;
                case 24:
                    #region add manually baskets
                    _baskets = new List<Coord>();
                    _baskets.Add(new Coord(0, 16));
                    _baskets.Add(new Coord(5, 4));
                    _baskets.Add(new Coord(8, 21));
                    _baskets.Add(new Coord(10, 3));
                    _baskets.Add(new Coord(11, 12));
                    _baskets.Add(new Coord(12, 18));
                    _baskets.Add(new Coord(12, 23));
                    _baskets.Add(new Coord(15, 13));
                    _baskets.Add(new Coord(15, 16));
                    _baskets.Add(new Coord(17, 1));
                    _baskets.Add(new Coord(23, 7));
                    _baskets.Add(new Coord(23, 22));
                    #endregion
                    #region add manually trees
                    _trees = new List<Coord>();
                    _trees.Add(new Coord(2, 11));
                    _trees.Add(new Coord(4, 3));
                    _trees.Add(new Coord(4, 4));
                    _trees.Add(new Coord(5, 3));
                    _trees.Add(new Coord(5, 16));
                    _trees.Add(new Coord(13, 18));
                    _trees.Add(new Coord(14, 11));
                    _trees.Add(new Coord(14, 12));
                    _trees.Add(new Coord(14, 13));
                    _trees.Add(new Coord(14, 18));
                    _trees.Add(new Coord(20, 5));
                    _trees.Add(new Coord(23, 23));
                    #endregion
                    #region add manually hunters
                    _hunters = new List<Hunter>();
                    _hunters.Add(new Hunter(2, 6, Direction.Down));
                    _hunters.Add(new Hunter(3, 23, Direction.Right));
                    _hunters.Add(new Hunter(5, 1, Direction.Right));
                    _hunters.Add(new Hunter(5, 13, Direction.Up));
                    _hunters.Add(new Hunter(5, 18, Direction.Left));
                    _hunters.Add(new Hunter(8, 9, Direction.Right));
                    _hunters.Add(new Hunter(11, 5, Direction.Down));
                    _hunters.Add(new Hunter(18, 4, Direction.Left));
                    _hunters.Add(new Hunter(18, 11, Direction.Right));
                    _hunters.Add(new Hunter(18, 23, Direction.Left));
                    _hunters.Add(new Hunter(20, 13, Direction.Left));
                    _hunters.Add(new Hunter(20, 17, Direction.Up));
                    #endregion
                    _tableSize = tableSize;
                    _playerCoords = new Coord(0, 0);
                    _elapsedSeconds = 0.0;
                    break;
            }
            
        }
        public GameInfos(List<Coord> baskets, List<Coord> trees, List<Hunter> hunters, int tableSize, Coord playerCoords, double elapsedSeconds)
        {
            _baskets = baskets;
            _trees = trees;
            _hunters = hunters;
            _tableSize = tableSize;
            _playerCoords = playerCoords;
            _elapsedSeconds = elapsedSeconds;
        }

        #endregion

        #region methods
        // a már felvett kosarat kiveszi a listából
        public void RemoveFromBaskets(Coord toRemove)
        {
            _baskets.Remove(toRemove);
        }

        //beállítja az eltelt időt másodpercben ha nem negatív
        public void SetElapsedSeconds(double elapsedSeconds)
        {
            if(elapsedSeconds >= 0)
            {
                _elapsedSeconds = elapsedSeconds;
            }
            else
            {
                throw new Exception("Invalid time format");
            }
        }
        #endregion
    }
}
