 using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  namespace ConsoleDungeon
  {
      // Oyun içi temel varlıklar (Oyuncu ve Düşmanlar)
      public abstract class Entity
      {
          public string Name { get; set; }
          public int X { get; set; }
          public int Y { get; set; }
          public int Health { get; set; }
          public int AttackPower { get; set; }
          public char Symbol { get; set; }

          public bool IsAlive => Health > 0;
      }

      public class Player : Entity
      {
          public int Level { get; set; } = 1;
          public int Experience { get; set; } = 0;
          public int Potions { get; set; } = 3;

          public void Heal()
          {
              if (Potions > 0)
              {
                  Health += 20;
                  Potions--;
                  Console.WriteLine("İksir içtiniz! +20 Can.");
              }
              else Console.WriteLine("İksiriniz kalmadı!");
          }
      }

      public class Enemy : Entity
      {
          public int ExpValue { get; set; }
      }

      public class Game
      {
          private const int MapWidth = 20;
          private const int MapHeight = 10;
          private char[,] map = new char[MapWidth, MapHeight];
          private Player player;
          private List<Enemy> enemies = new List<Enemy>();
          private Random rng = new Random();
          private bool isRunning = true;

          public Game()
          {
              player = new Player { Name = "Kahraman", Symbol = '@', Health = 100, AttackPower = 10, X = 1, Y = 1 };
              InitializeMap();
          }

          private void InitializeMap()
          {
              // Haritayı duvarlarla doldur
              for (int x = 0; x < MapWidth; x++)
                  for (int y = 0; y < MapHeight; y++)
                      map[x, y] = (x == 0 || x == MapWidth - 1 || y == 0 || y == MapHeight - 1) ? '#' : '.';

              // Rastgele duvarlar ekle
              for (int i = 0; i < 30; i++)
              {
                  int rx = rng.Next(1, MapWidth - 1);
                  int ry = rng.Next(1, MapHeight - 1);
                  if ((rx != player.X || ry != player.Y)) map[rx, ry] = '#';
              }

              // Çıkış kapısını yerleştir
              map[MapWidth - 2, MapHeight - 2] = 'E';

              // Düşmanları yerleştir
              for (int i = 0; i < 5; i++)
              {
                  int ex = rng.Next(1, MapWidth - 1);
                  int ey = rng.Next(1, MapHeight - 1);
                  if (map[ex, ey] == '.')
                  {
                      enemies.Add(new Enemy { Name = "Goblin", Symbol = 'G', Health = 30, AttackPower = 5, ExpValue =
  20, X = ex, Y = ey });
                  }
              }
          }

          public void Start()
          {
              Console.CursorVisible = false;
              while (isRunning)
              {
                  Draw();
                  HandleInput();
                  Update();
              }
              Console.Clear();
              if (player.IsAlive) Console.WriteLine("TEBRİKLER! Zindandan sağ çıktınız!");
              else Console.WriteLine("ÖLDÜNÜZ... Karanlık zindan sizi yuttu.");
              Console.ReadLine();
          }

          private void Draw()
          {
              Console.SetCursorPosition(0, 0);
              Console.WriteLine($"--- ZİNDAN GEZGİNİ ---  Seviye: {player.Level} | Can: {player.Health} | İksir:
  {player.Potions} | TP: {player.Experience}");
              Console.WriteLine("Hareket: WASD | İksir: H | Çıkış: E");
              Console.WriteLine("--------------------------------------------------");

              for (int y = 0; y < MapHeight; y++)
              {
                  for (int x = 0; x < MapWidth; x++)
                  {
                      bool enemyHere = false;
                      foreach (var e in enemies)
                      {
                          if (e.X == x && e.Y == y && e.IsAlive)
                          {
                              Console.ForegroundColor = ConsoleColor.Red;
                              Console.Write(e.Symbol);
                              Console.ResetColor();
                              enemyHere = true;
                              break;
                          }
                      }

                      if (!enemyHere)
                      {
                          if (x == player.X && y == player.Y)
                          {
                              Console.ForegroundColor = ConsoleColor.Cyan;
                              Console.Write(player.Symbol);
                              Console.ResetColor();
                          }
                          else
                          {
                              Console.Write(map[x, y]);
                          }
                      }
                  }
                  Console.WriteLine();
              }
              Console.WriteLine("--------------------------------------------------");
          }

          private void HandleInput()
          {
              if (!Console.KeyAvailable) return;
              var key = Console.ReadKey(true).Key;

              int nextX = player.X;
              int nextY = player.Y;

              if (key == ConsoleKey.W) nextY--;
              if (key == ConsoleKey.S) nextY++;
              if (key == ConsoleKey.A) nextX--;
              if (key == ConsoleKey.D) nextX++;
              if (key == ConsoleKey.H) player.Heal();

              if (nextX >= 0 && nextX < MapWidth && nextY >= 0 && nextY < MapHeight)
              {
                  if (map[nextX, nextY] != '#')
                  {
                      player.X = nextX;
                      player.Y = nextY;
                  }
              }

              if (map[player.X, player.Y] == 'E') isRunning = false;
          }

          private void Update()
          {
              // Savaş kontrolü
              var enemy = enemies.FirstOrDefault(e => e.X == player.X && e.Y == player.Y && e.IsAlive);
              if (enemy != null)
              {
                  Console.WriteLine($"\n{enemy.Name} ile savaşıyorsunuz!");
                  enemy.Health -= player.AttackPower;
                  player.Health -= enemy.AttackPower;

                  if (!enemy.IsAlive)
                  {
                      Console.WriteLine($"{enemy.Name} öldü! +{enemy.ExpValue} TP");
                      player.Experience += enemy.ExpValue;
                      if (player.Experience >= 50)
                      {
                          player.Level++;
                          player.AttackPower += 5;
                          player.Health = 100;
                          player.Experience = 0;
                          Console.WriteLine("SEVİYE ATLADINIZ! Güçlendiniz ve canınız yenilendi.");
                      }
                  }
                  Thread.Sleep(500);
              }

              if (player.Health <= 0) isRunning = false;
          }
      }

      class Program
      {
          static void Main()
          {
              Game game = new Game();
              game.Start();
          }
      }
  }
