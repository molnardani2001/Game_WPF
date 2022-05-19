using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_WPF.Persistence;

namespace Game_WPF.Model
{
    public enum Difficulty { Easy, Medium, Hard }

    public class GameModel
    {
        #region constants

        private const int GeneratedFieldCountEasy = 12;
        private const int GeneratedFieldCountMedium = 18;
        private const int GeneratedFieldCountHard = 24;

        private const int BasketFieldEasy = 6;
        private const int BasketFieldMedium = 9;
        private const int BasketFieldHard = 12;

        #endregion

        #region private fields

        private int _basketCount;
        private Player _player;
        private GameInfos _gameInfos;
        private IGameDataAccess _dataAccess;
        private bool _gameOver;
        private bool _paused;
        private DateTime _startTime;
        private double _elapsedSeconds;

        #endregion


        #region properties

        public int GetGeneratedFieldCountEasy { get { return GeneratedFieldCountEasy; } }
        public int GetGeneratedFieldCountMedium { get { return GeneratedFieldCountMedium; } }
        public int GetGeneratedFieldCountHard { get { return GeneratedFieldCountHard; } }
        public GameInfos GameInfos { get { return _gameInfos; } }

        public Player Player { get { return _player; } }

        public int BasketCount { get { return _basketCount; } }

        public bool GameOver { get { return _gameOver; } }

        public bool Paused { get { return _paused; } }

        public DateTime StartTime { get { return _startTime; } }

        public double ElapsedSeconds { get { return _elapsedSeconds; } }

        #endregion

        #region events

        public event EventHandler<GameOverEventArgs> OnGameOver;
        public event EventHandler OnBasketFound;
        public event EventHandler GameAdvenced;

        #endregion

        #region constructors
        public GameModel(IGameDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameOver = false;
            _paused = false;
        }

        #endregion

        #region tasks

        public async Task LoadNewGameAsync(string path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _gameInfos = await _dataAccess.LoadAsync(path); // betölti a privát változóba a játék információkat
            _player = new Player(_gameInfos.PlayerCoords.X, _gameInfos.PlayerCoords.Y); // létrehoz egy új játékost
            _startTime = DateTime.Now; // beállítja a kezdeti időt
            _gameOver = false;
            _paused = false;
            _elapsedSeconds = _gameInfos.ElapsedSeconds;

            switch (_gameInfos.TableSize)
            {
                case GeneratedFieldCountEasy: _basketCount = BasketFieldEasy - _gameInfos.Baskets.Count; 
                    break;
                case GeneratedFieldCountMedium: _basketCount = BasketFieldMedium - _gameInfos.Baskets.Count;
                    break;
                case GeneratedFieldCountHard: _basketCount = BasketFieldHard - _gameInfos.Baskets.Count;
                    break;
                default: _basketCount = _gameInfos.Hunters.Count - _gameInfos.Baskets.Count;
                    break;
            }
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _gameInfos.SetElapsedSeconds(_elapsedSeconds); // ellenőrzés után elmenti az eltelt időt
            await _dataAccess.SaveAsync(path, _gameInfos, _player); // kiírja egy fájlba az információkat
        }

        #endregion

        #region methods

        public void NewGame(GameInfos gameInfos)
        {
            _gameInfos = gameInfos;
            _player = new Player(_gameInfos.PlayerCoords.X, _gameInfos.PlayerCoords.Y);
            _gameOver = false;
            _paused = false;
            _elapsedSeconds = _gameInfos.ElapsedSeconds;
        }

        public void PlayerMove(Direction direction)
        {
            if (_paused)
            {
                return;
            }
            if (_gameOver)
            {
                return;
            }
            _player.Move(direction, _gameInfos.Trees, _gameInfos.TableSize);
            CheckNoHuntersAroundPlayer();
            CheckBasketFound();
            if(_gameInfos.Baskets.Count == 0)
            {
                _gameOver = true;
                if(OnGameOver != null)
                {
                    OnGameOver(this, new GameOverEventArgs(true, null));
                }
                
            }
        }

        public void HuntersMove()
        {
            if(GameAdvenced != null)
            {
                GameAdvenced(this, EventArgs.Empty);
            }
            
            foreach(var item in _gameInfos.Hunters)
            {
                item.Move(_gameInfos.Trees, _gameInfos.TableSize);
            }
            CheckNoHuntersAroundPlayer();

        }

        private void CheckBasketFound()
        {
            Coord tmpCoord = new Coord(_player.X, _player.Y);
            if (_gameInfos.Baskets.Contains(tmpCoord))
            {
                _basketCount++;
                _gameInfos.RemoveFromBaskets(tmpCoord);
                if(OnBasketFound != null)
                {
                    OnBasketFound(this, null);
                }
                
            }
        }

        private void CheckNoHuntersAroundPlayer()
        {
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <=1; j++)
                {
                    Hunter tmpHunter = new Hunter(_player.X + i, _player.Y + j, Direction.NoDirection);
                    if (_gameInfos.Hunters.Contains(tmpHunter))
                    {
                        _gameOver = true;
                        if(OnGameOver != null)
                        {
                            OnGameOver(this, new GameOverEventArgs(false, tmpHunter));
                        }
                        
                    }
                }
            }
        }

        public bool CheckHunterAndBasketOnSameField(Hunter hunter)
        {
            Coord tmpCoord = new Coord(hunter.CoordX, hunter.CoordY);
            return _gameInfos.Baskets.Contains(tmpCoord);
        }

        #endregion

        #region time methods

        public void PauseGame()
        {
            if (!_paused)
            {
                _paused = true;
                _elapsedSeconds += (DateTime.Now - _startTime).TotalSeconds;
            }
        }

        public void ContinueGame()
        {
            if (_paused)
            {
                _paused = false;
                _startTime = DateTime.Now;
            }
            
        }

        #endregion
    } 
}
