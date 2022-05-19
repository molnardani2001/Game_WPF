using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Game_WPF.Model;
using Game_WPF.Persistence;
using Moq;
using System.Threading.Tasks;

namespace Game_WPF.GameTest
{
    [TestClass]
    public class GameTest
    {
        private GameModel _model;
        private GameInfos _mockedInfos;
        private Mock<IGameDataAccess> _mock;


        private void Initialize(int tableSize)
        {
            _mockedInfos = new GameInfos(tableSize);

            _mock = new Mock<IGameDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedInfos));

            _model = new GameModel(_mock.Object);
            _model.NewGame(_mockedInfos);

            _model.OnGameOver += new EventHandler<GameOverEventArgs>(Model_GameOver);
            //_model.OnBasketFound += new EventHandler(Model_BasketFound);

        }

        [TestMethod]
        public void GameModelNewGameEasyTest()
        {
            Initialize(12);
            Assert.AreEqual(0, _model.BasketCount); // még egy kosarat sem gyûjtöttünk
            Assert.AreEqual(new Coord(0, 0), _model.GameInfos.PlayerCoords); // játékos kezdetben a 0,0 koordinátán áll

            int emptyFields = (int)Math.Pow(_model.GetGeneratedFieldCountEasy, 2) - _model.GameInfos.Hunters.Count -
                _model.GameInfos.Trees.Count - _model.GameInfos.Baskets.Count - 1;

            Assert.AreEqual(125, emptyFields); // szabad mezõk száma is megfelelõ
        }

        [TestMethod]
        public void GameModelNewGameMediumTest()
        {
            Initialize(18);
            Assert.AreEqual(0, _model.BasketCount);
            Assert.AreEqual(new Coord(0, 0), _model.GameInfos.PlayerCoords);

            int emptyFields = (int)Math.Pow(_model.GetGeneratedFieldCountMedium, 2) - _model.GameInfos.Hunters.Count -
                _model.GameInfos.Trees.Count - _model.GameInfos.Baskets.Count - 1;

            Assert.AreEqual(296, emptyFields); // szabad mezõk száma is megfelelõ
        }

        [TestMethod]
        public void GameModelNewGameHardTest()
        {
            Initialize(24);
            Assert.AreEqual(0, _model.BasketCount);
            Assert.AreEqual(new Coord(0, 0), _model.GameInfos.PlayerCoords);

            int emptyFields = (int)Math.Pow(_model.GetGeneratedFieldCountHard, 2) - _model.GameInfos.Hunters.Count -
                _model.GameInfos.Trees.Count - _model.GameInfos.Baskets.Count - 1;

            Assert.AreEqual(539, emptyFields); // szabad mezõk száma is megfelelõ
        }

        [TestMethod]
        public void GameModelPlayerMoveTest()
        {
            Initialize(12);
            Assert.AreEqual(new Coord(0, 0), _model.GameInfos.PlayerCoords);
            _model.PlayerMove(Direction.Up);
            Assert.AreEqual(new Coord(0, 0), new Coord(_model.Player.X, _model.Player.Y)); // mivel a bal felsõ sarokban áll nem változik a koordináta
            _model.PlayerMove(Direction.Left);
            Assert.AreEqual(new Coord(0, 0), new Coord(_model.Player.X, _model.Player.Y)); // balra sem léphet
            _model.PlayerMove(Direction.Right);
            Assert.AreEqual(new Coord(1, 0), new Coord(_model.Player.X, _model.Player.Y)); // ellépett egyel jobra
            _model.PlayerMove(Direction.Right);
            Assert.AreEqual(new Coord(2, 0), new Coord(_model.Player.X, _model.Player.Y)); //ellépett még egyel jobbra
            _model.PlayerMove(Direction.Down);
            Assert.AreEqual(new Coord(2, 0), new Coord(_model.Player.X, _model.Player.Y)); // mivel fába ütközne ezért nem változik a koordináta
        }

        [TestMethod]
        public void GameModelHuntersMoveTest()
        {
            Initialize(12);
            foreach (Hunter hunter in _model.GameInfos.Hunters)
            {
                Hunter previoustPosition = new Hunter(hunter.CoordX, hunter.CoordY, hunter.Direction); // lementjük az elõzõ pozíciót
                hunter.Move(_model.GameInfos.Trees, 12); // léptetjük egyel a vadászt a megfelelõ irányába
                Hunter currentPosition = new Hunter(hunter.CoordX, hunter.CoordY, hunter.Direction); // lementjük a jelenlegi pozícióját
                Assert.IsFalse(_model.GameInfos.Hunters.Contains(previoustPosition));
                Assert.IsTrue(_model.GameInfos.Hunters.Contains(currentPosition)); // megnézzük, hogy az elõzõ pozíciójáról tényleg ellépett
            }

        }

        [TestMethod]
        public void GameModelCheckBasketFoundTest()
        {
            Initialize(12);
            _model.PlayerMove(Direction.Right);
            _model.PlayerMove(Direction.Right);
            _model.PlayerMove(Direction.Right);
            _model.PlayerMove(Direction.Right);
            _model.PlayerMove(Direction.Down); // felvesszük az egyik kosarat

            Assert.AreEqual(5, _model.GameInfos.Baskets.Count);
            Assert.AreEqual(1, _model.BasketCount);
        }

        [TestMethod]
        public void GameModelCheckNoHuntersAroundPlayerTest()
        {
            Initialize(12);
            _model.PlayerMove(Direction.Down);
            _model.PlayerMove(Direction.Down);
            _model.PlayerMove(Direction.Down);
            //ezeknek köszönhetõen kiváltódik az esemény, hogy meglátott egy vadász és elvesztettük a játékot

        }

        private void Model_GameOver(object sender, GameOverEventArgs e)
        {
            Assert.IsTrue(_model.GameOver); //vége a játéknak
            Assert.AreEqual(new Hunter(2, 3, Direction.Right), e.Hunter); //ez a vadász látta meg a játékost
            Assert.IsFalse(e.IsWon); // nem nyert a játékos
        }

        [TestMethod]
        public void GameModelCheckHunterAndBasketOnSameFieldTest()
        {
            Initialize(12);
            for (int i = 0; i < 5; ++i)
            {
                _model.HuntersMove();
            }

            bool areHuntersOnBasket = false;
            int numbersOfHunters = 0;

            foreach (Hunter hunter in _model.GameInfos.Hunters)
            {
                bool thisHunterOnSameField = _model.CheckHunterAndBasketOnSameField(hunter);
                areHuntersOnBasket = areHuntersOnBasket || thisHunterOnSameField;
                if (thisHunterOnSameField)
                {
                    ++numbersOfHunters;
                }

            }

            Assert.IsTrue(areHuntersOnBasket); // kell lennie 2 vadásznak is olyan mezõn hol kosár van
            Assert.AreEqual(2, numbersOfHunters);

            _model.HuntersMove();
            areHuntersOnBasket = false;
            numbersOfHunters = 0;

            foreach (Hunter hunter in _model.GameInfos.Hunters)
            {
                bool thisHunterOnSameField = _model.CheckHunterAndBasketOnSameField(hunter);
                areHuntersOnBasket = areHuntersOnBasket || thisHunterOnSameField;
                if (thisHunterOnSameField)
                {
                    ++numbersOfHunters;
                }

            }

            Assert.IsFalse(areHuntersOnBasket); // mostmár egy vadásznak sem szabad azonos mezõn lennie egy kosárral
            Assert.AreEqual(0, numbersOfHunters);
        }

        [TestMethod]
        public async Task GameModelLoadNewGameTest()
        {
            Initialize(12);
            await _model.LoadNewGameAsync(string.Empty);

            foreach (Coord tree in _model.GameInfos.Trees)
            {
                Assert.IsTrue(_mockedInfos.Trees.Contains(tree));
                //ellenõrizzük, hogy valamennyi fa megfelelõ-e
            }

            foreach (Hunter hunter in _model.GameInfos.Hunters)
            {
                Assert.IsTrue(_mockedInfos.Hunters.Contains(hunter));
                //ellenõrizzük, hogy valamennyi vadász megfelelõ-e
            }

            foreach (Coord basket in _model.GameInfos.Baskets)
            {
                Assert.IsTrue(_mockedInfos.Baskets.Contains(basket));
                //ellenõrizzük, hogy valamennyi kosár megfelelõ-e
            }
        }
    }
}
