using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Game_WPF.Persistence
{
    public class GameDataAccess : IGameDataAccess
    {
        #region tasks
        public async Task<GameInfos> LoadAsync(String path)
        {
            
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    List<Coord> trees = new List<Coord>();
                    List<Coord> baskets = new List<Coord>();
                    List<Hunter> hunters = new List<Hunter>();
                    String line = await reader.ReadLineAsync();
                    int tableSize = Convert.ToInt32(line);
                    line = await reader.ReadLineAsync();
                    double elapsedSeconds = Convert.ToDouble(line);
                    line = await reader.ReadLineAsync();
                    string[] splitted = line.Split(",");
                    int playerX = Convert.ToInt32(splitted[0] + "");
                    int playerY = Convert.ToInt32(splitted[1] + "");

                    line = await reader.ReadLineAsync();
                    splitted = line.Split(";");
                    for(int i = 0; i < splitted.Length; i++)
                    {
                        string actual = splitted[i];
                        String[] splitCoord = actual.Split(",");
                        Coord treeCoord = new Coord(Int32.Parse(splitCoord[0]), Int32.Parse(splitCoord[1]));
                        trees.Add(treeCoord);
                    }

                    line = await reader.ReadLineAsync();
                    splitted = line.Split(";");
                    for (int i = 0; i < splitted.Length; i++)
                    {
                        string actual = splitted[i];
                        String[] splitCoord = actual.Split(",");
                        Hunter hunter = null;
                        switch (splitCoord[2])
                        {
                            case "Up":
                                hunter = new Hunter(Int32.Parse(splitCoord[0]), Int32.Parse(splitCoord[1]), Direction.Up);
                                break;
                            case "Down":
                                hunter = new Hunter(Int32.Parse(splitCoord[0]), Int32.Parse(splitCoord[1]), Direction.Down);
                                break;
                            case "Left":
                                hunter = new Hunter(Int32.Parse(splitCoord[0]), Int32.Parse(splitCoord[1]), Direction.Left);
                                break;
                            case "Right":
                                hunter = new Hunter(Int32.Parse(splitCoord[0]), Int32.Parse(splitCoord[1]), Direction.Right);
                                break;
                        }
                        hunters.Add(hunter);
                    }

                    line = await reader.ReadLineAsync();
                    splitted = line.Split(";");
                    for (int i = 0; i < splitted.Length; i++)
                    {
                        string actual = splitted[i];
                        String[] splitCoord = actual.Split(",");
                        Coord basketCoord = new Coord(Int32.Parse(splitCoord[0]), Int32.Parse(splitCoord[1]));
                        baskets.Add(basketCoord);
                    }

                    return new GameInfos(baskets, trees, hunters, tableSize, new Coord(playerX, playerY), elapsedSeconds);

                }
            }
            catch
            {
                throw new Exception("File not found!");
            }

        }

        public async Task SaveAsync(String path, GameInfos infos, Player player)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteLineAsync(infos.TableSize.ToString());
                    await writer.WriteLineAsync(Math.Floor(infos.ElapsedSeconds).ToString());
                    await writer.WriteLineAsync(player.X + "," + player.Y);
                    for(int i = 0; i < infos.Trees.Count; i++)
                    {
                        await writer.WriteAsync(infos.Trees[i].X + "," + infos.Trees[i].Y);
                        if(i < infos.Trees.Count - 1)
                        {
                            await writer.WriteAsync(";");
                        }
                    }
                    await writer.WriteLineAsync();

                    for(int i = 0; i < infos.Hunters.Count; i++)
                    {
                        await writer.WriteAsync(infos.Hunters[i].CoordX + "," + infos.Hunters[i].CoordY + ",");
                        switch (infos.Hunters[i].Direction)
                        {
                            case Direction.Up:
                                await writer.WriteAsync("Up");
                                break;
                            case Direction.Down:
                                await writer.WriteAsync("Down");
                                break;
                            case Direction.Left:
                                await writer.WriteAsync("Left");
                                break;
                            case Direction.Right:
                                await writer.WriteAsync("Right");
                                break;
                        }
                        if (i < infos.Trees.Count - 1)
                        {
                            await writer.WriteAsync(";");
                        }
                    }
                    await writer.WriteLineAsync();

                    for (int i = 0; i < infos.Baskets.Count; i++)
                    {
                        await writer.WriteAsync(infos.Baskets[i].X + "," + infos.Baskets[i].Y);
                        if (i < infos.Baskets.Count - 1)
                        {
                            await writer.WriteAsync(";");
                        }
                    }
                    await writer.WriteLineAsync();
                    writer.Close();
                }
            }
            catch
            {
                throw new Exception();
            }
        }
        #endregion
    }

}
