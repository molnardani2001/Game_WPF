using System;
using System.Windows;
using Game_WPF.Model;
using Game_WPF.View;
using Game_WPF.ViewModel;
using Game_WPF.Persistence;
using System.Windows.Threading;
using Microsoft.Win32;
using System.ComponentModel;

namespace Game_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region fields

        private GameModel _model; //üzleti logika
        private GameViewModel _viewModel; //logika-nézet
        private MainWindow _view; //nézet
        private DispatcherTimer _timer; //időzítő

        #endregion

        #region constructors

        //indítás eseménykezelő feliratkoztatása
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region application event handlers

        //események feliratkoztatása, adatkötés beállítása, mezők példányostása
        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new GameModel(new GameDataAccess());
            _model.OnGameOver += new EventHandler<GameOverEventArgs>(Model_GameOver);

            _viewModel = new GameViewModel(_model);
            _viewModel.NewGameEasy += new EventHandler(ViewModel_NewGameEasy);
            _viewModel.NewGameMedium += new EventHandler(ViewModel_NewGameMedium);
            _viewModel.NewGameHard += new EventHandler(ViewModel_NewGameHard);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);

            _view = new MainWindow()
            {
                DataContext = _viewModel
            };
            //_view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);
            _view.Show();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_model.Paused || _model.GameOver)
            {
                return;
            }
            _model.HuntersMove();
        }

        #endregion

        #region view event handlers

        //ablak bezárása eseménykezelője
        private void View_Closing(object sender, CancelEventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();
            _model.PauseGame();

            if(MessageBox.Show("Biztos, hogy ki akar lépni?","Game",MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
                if (restartTimer)
                {
                    _timer.Start();
                    _model.ContinueGame();
                }
            }
        }

        #endregion

        #region viewmodel event handlers

        //játék betöltése eseménykezelő
        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            _timer.Stop();
            _model.PauseGame();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Játék betöltése";
                openFileDialog.Filter = "Game Info files (*.txt)|*.txt|All files (*.*)|*.*";
                if(openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadNewGameAsync(openFileDialog.FileName);
                    _viewModel.GenerateTable(_model.GameInfos.TableSize);
                    _viewModel.RegisterPlayer();
                    _viewModel.RegisterHunter();
                    _viewModel.DrawDefaultGame();
                    _viewModel.ContinueEnabled = !_model.GameOver;
                }
            }
            catch
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Game", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (_viewModel.ContinueEnabled)
            {
                _timer.Start();
                _model.ContinueGame();
                _viewModel.ContinueText = "Szünet";
            }
           
        }

        //játék mentése eseménykezelő
        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            if(_model.GameInfos == null)
            {
                return;
            }
            Boolean restartTimer = _timer.IsEnabled;
            _timer.Stop();
            _model.PauseGame();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Játék mentése";
                saveFileDialog.Filter = "Game Info files (*.txt)|*.txt|All files (*.*)|*.*";
                if(saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (restartTimer)
            {
                _timer.Start();
                _model.ContinueGame();
            }
        }

        //játék megállítása/folytatása eseménykezelő
        private void ViewModel_PauseGame(object sender, EventArgs e)
        {
            if (!_model.Paused)
            {
                _model.PauseGame();
                _viewModel.ContinueText = "Folytatás";
                _timer.Stop();
            }
            else
            {
                _model.ContinueGame();
                _viewModel.ContinueText = "Szünet";
                _timer.Start();
            }
        }

        //új könnyű játék eseménykezelő
        private void ViewModel_NewGameEasy(object sender, EventArgs e)
        {
            Initialize(_model.GetGeneratedFieldCountEasy, "EasyMode.txt");
        }

        //új közepes játék eseménykezelő
        private void ViewModel_NewGameMedium(object sender, EventArgs e)
        {
            Initialize(_model.GetGeneratedFieldCountMedium, "MediumMode.txt");
        }

        //új nehéz játék eseménykezelő
        private void ViewModel_NewGameHard(object sender, EventArgs e)
        {
            Initialize(_model.GetGeneratedFieldCountHard, "HardMode.txt");
        }

        #endregion

        #region model event handlers

        //játék vége eseménykezelő
        private void Model_GameOver(object sender, GameOverEventArgs e)
        {
            _timer.Stop();
            if (e.IsWon)
            {
                MessageBox.Show("Gratulálok, nyertél!" + Environment.NewLine
                    + "Összegyűjtötted mind a(z) " + _viewModel.BasketCount + " kosarat és összesen "
                    + _viewModel.GameTime + " ideig játszottál!", "Játék vége", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Vesztettél!" + Environment.NewLine
                    + "A(z) (" + (e?.Hunter.CoordX + 1) + "," + (e?.Hunter.CoordY + 1) + ") koordinátán lévő vadász meglátott!", "Játék végen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            _viewModel.ContinueEnabled = !_model.GameOver;
        }

        #endregion

        #region methods (helper)

        //pálya inicializálása, betölti a model-be a fájlt a fájlnév alapján, majd összekapcsolja az összes
        //vadászt és a játékost a megfelelő eseményekhez
        private async void Initialize(int startSize, string path)
        {
            _timer.Stop();
            _viewModel.ContinueText = "Szünet";
            _viewModel.GenerateTable(startSize);
            await _model.LoadNewGameAsync(path);
            _viewModel.ContinueEnabled = !_model.GameOver;
            _viewModel.RegisterHunter();
            _viewModel.RegisterPlayer();
            _viewModel.DrawDefaultGame();
            _timer.Start();
        }

        #endregion
    }
}
