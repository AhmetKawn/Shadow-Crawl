 using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  namespace ConsoleDungeon
  {
      // --- TEMEL SINIFLAR ---
      public abstract class Entity
      {
          public string Name { get; set; }
          public int X { get; set; }
          public int Y { get; set; }
          public int Health { get; set; }
          public int MaxHealth { get; set; }
          public int AttackPower { get; set; }
          public char Symbol { get; set; }
          public bool IsAlive => Health > 0;
      }

      public class Player : Entity
      {
          public int Level { get; set; } = 1;
          public int Experience { get; set; } = 0;
          public int Potions { get; set; } = 3;
          public int CurrentFloor { get; set; } = 1;
          public int Defense { get; set; } = 0; // Zırh puanı
          public string WeaponName { get; set; } = "Paslı Bıçak";

          public void Heal()
          {
              if (Potions > 0)
              {
                  int healAmount = 30;
                  Health = Math.Min(MaxHealth, Health + healAmount);
                  Potions--;
                  return $"İksir kullandınız! +{healAmount} Can.";
              }
              return "İksiriniz kalmadı!";
          }
      }

      public class Enemy : Entity
      {
          public int ExpValue { get; set; }
          public ConsoleColor Color { get; set; }
      }

      public class Item
      {
          public string Name { get; set; }
          public char Symbol { get; set; }
          public string Type { get; set; } // "Weapon" or "Armor"
          public int Value { get; set; }
      }

      // --- OYUN MOTORU ---
      public class Game
      {
          private const int MapWidth = 30;
          private const int MapHeight = 12;
          private char[,] map = new char[MapWidth, MapHeight];
          private Player player;
          private List<Enemy> enemies = new List<Enemy>();
          private List<Item> items = new List<Item>();
          private List<string> gameLog = new List<string>();
          private Random rng = new Random();
          private bool isRunning = true;

          public Game()
          {
              player = new Player { Name = "Kahraman", Symbol = '@', MaxHealth = 100, Health = 100, AttackPower = 12, X
  = 1, Y = 1 };
              InitializeFloor();
          }

          private void InitializeFloor()
          {
              enemies.Clear();
              items.Clear();

              // Haritayı temizle ve duvar ör
              for (int x = 0; x < MapWidth; x++)
                  for (int y = 0; y < MapHeight; y++)
                      map[x, y] = (x == 0 || x == MapWidth - 1 || y == 0 || y == MapHeight - 1) ? '#' : '.';

              // Rastgele duvarlar
              for (int i = 0; i < 40; i++)
              {
                  int rx = rng.Next(1, MapWidth - 1);
                  int ry = rng.Next(1, MapHeight - 1);
                  if ((rx != player.X || ry != player.Y)) map[rx, ry] = '#';
              }

              map[MapWidth - 2, MapHeight - 2] = 'E'; // Çıkış

              // Kat bazlı düşman üretimi
              int enemyCount = 4 + player.CurrentFloor;
              for (int i = 0; i < enemyCount; i++)
                  SpawnEnemy();

              // Rastgele eşya üretimi
              for (int i = 0; i < 3; i++)
                  SpawnItem();

              AddLog($"--- KAT {player.CurrentFloor} --- Zindanın derinliklerine indiniz...");
          }

          private void SpawnEnemy()
          {
              int ex = rng.Next(1, MapWidth - 1);
              int ey = rng.Next(1, MapHeight - 1);
              if (map[ex, ey] != '.') return;

              Enemy enemy;
              int roll = rng.Next(0, 100);

              if (roll < 60) // Goblin
                  enemy = new Enemy { Name = "Goblin", Symbol = 'G', Health = 30 + (player.CurrentFloor * 5),
  AttackPower = 5 + player.CurrentFloor, ExpValue = 20, X = ex, Y = ey, Color = ConsoleColor.Green };
              else if (roll < 90) // Ork
                  enemy = new Enemy { Name = "Ork", Symbol = 'O', Health = 60 + (player.CurrentFloor * 10), AttackPower
  = 10 + player.CurrentFloor, ExpValue = 40, X = ex, Y = ey, Color = ConsoleColor.DarkRed };
              else // Troll
                  enemy = new Enemy { Name = "Troll", Symbol = 'T', Health = 120 + (player.CurrentFloor * 20),
  AttackPower = 15 + player.CurrentFloor, ExpValue = 80, X = ex, Y = ey, Color = ConsoleColor.DarkYellow };

              enemies.Add(enemy);
          }

          private void SpawnItem()
          {
              int ix = rng.Next(1, MapWidth - 1);
              int iy = rng.Next(1, MapHeight - 1);
              if (map[ix, iy] != '.') return;

              Item item;
              if (rng.Next(0, 2) == 0)
                  item = new Item { Name = "Keskin Kılıç", Symbol = 'W', Type = "Weapon", Value = rng.Next(5, 15) };
              else
                  item = new Item { Name = "Çelik Zırh", Symbol = 'A', Type = "Armor", Value = rng.Next(2, 8) };

              items.Add(item);
              map[ix, iy] = item.Symbol;
          }

          private void AddLog(string message)
          {
              gameLog.Add(message);
              if (gameLog.Count > 5) gameLog.RemoveAt(0);
          }

          public void Start()
          {
              Console.CursorVisible = false;
              while (isRunning)
              {
                  Draw();
                  HandleInput();
                  Update();
                  Thread.Sleep(50);
              }
              Console.Clear();
              if (player.IsAlive) Console.WriteLine($"TEBRİKLER! {player.CurrentFloor} kat boyunca hayatta kaldınız!");
              else Console.WriteLine("ÖLDÜNÜZ... Ruhunuz zindanda hapsoldu.");
              Console.ReadLine();
          }

          private void Draw()
          {
              Console.SetCursorPosition(0, 0);
              Console.ForegroundColor = ConsoleColor.Yellow;
              Console.WriteLine($"🏰 ZİNDAN GEZGİNİ v2.0 | Kat: {player.CurrentFloor} | Seviye: {player.Level} | Can:
  {player.Health}/{player.MaxHealth} | İksir: {player.Potions} | TP: {player.Experience}");
              Console.ForegroundColor = ConsoleColor.White;
              Console.WriteLine($"Silah: {player.WeaponName} (+{player.AttackPower}) | Zırh: {player.Defense} | Hareket:
  WASD | İksir: H");
              Console.WriteLine(new string('=', MapWidth + 2));

              for (int y = 0; y < MapHeight; y++)
              {
                  for (int x = 0; x < MapWidth; x++)
                  {
                      bool entityHere = false;
                      var enemy = enemies.FirstOrDefault(e => e.X == x && e.Y == y && e.IsAlive);
                      if (enemy != null)
                      {
                          Console.ForegroundColor = enemy.Color;
                          Console.Write(enemy.Symbol);
                          Console.ResetColor();
                          entityHere = true;
                      }

                      if (!entityHere)
                      {
                          if (x == player.X && y == player.Y)
                          {
                              Console.ForegroundColor = ConsoleColor.Cyan;
                              Console.Write(player.Symbol);
                              Console.ResetColor();
                          }
                          else
                          {
                              char tile = map[x, y];
                              if (tile == '#') Console.ForegroundColor = ConsoleColor.Gray;
                              else if (tile == 'E') Console.ForegroundColor = ConsoleColor.Magenta;
                              else if (tile == 'W' || tile == 'A') Console.ForegroundColor = ConsoleColor.Yellow;
                              else Console.ForegroundColor = ConsoleColor.DarkGray;
                              Console.Write(tile);
                              Console.ResetColor();
                          }
                      }
                  }
                  Console.WriteLine();
              }
              Console.WriteLine(new string('=', MapWidth + 2));

              // Log Paneli
              Console.ForegroundColor = ConsoleColor.Gray;
              Console.WriteLine("--- SAVAŞ GÜNLÜĞÜ ---");
              foreach (var log in gameLog)
              {
                  Console.WriteLine($"> {log}");
              }
              // Boşluk doldurma (Ekran titremesini önlemek için)
              for (int i = gameLog.Count; i < 5; i++) Console.WriteLine();
              Console.ResetColor();
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
              if (key == ConsoleKey.H) AddLog(player.Heal());

              if (nextX >= 0 && nextX < MapWidth && nextY >= 0 && nextY < MapHeight)
              {
                  if (map[nextX, nextY] != '#')
                  {
                      player.X = nextX;
                      player.Y = nextY;
                  }
              }

              // Eşya toplama
              var item = items.FirstOrDefault(i => i.X == player.X && i.Y == player.Y); // Not: item koordinatları map'e
  işlendi, bu kısım basitleştirildi
              // Basitlik için map üzerinden kontrol:
              if (map[player.X, player.Y] == 'W')
              {
                  player.AttackPower += 5;
                  player.WeaponName = "Keskin Kılıç";
                  AddLog("Bir kılıç buldunuz! Saldırınız arttı.");
                  map[player.X, player.Y] = '.';
              }
              else if (map[player.X, player.Y] == 'A')
              {
                  player.Defense += 3;
                  AddLog("Bir zırh buldunuz! Savunmanız arttı.");
                  map[player.X, player.Y] = '.';
              }

              if (map[player.X, player.Y] == 'E')
              {
                  player.CurrentFloor++;
                  player.X = 1; player.Y = 1;
                  InitializeFloor();
              }
          }

          private void Update()
          {
              var enemy = enemies.FirstOrDefault(e => e.X == player.X && e.Y == player.Y && e.IsAlive);
              if (enemy != null)
              {
                  // Oyuncu saldırısı
                  int damageToEnemy = player.AttackPower + rng.Next(0, 5);
                  enemy.Health -= damageToEnemy;
                  AddLog($"{enemy.Name}'e {damageToEnemy} hasar verdiniz!");

                  // Düşman saldırısı
                  int damageToPlayer = Math.Max(1, enemy.AttackPower - player.Defense + rng.Next(0, 3));
                  player.Health -= damageToPlayer;
                  AddLog($"{enemy.Name} size {damageToPlayer} hasar verdi!");

                  if (!enemy.IsAlive)
                  {
                      AddLog($"{enemy.Name} öldü! +{enemy.ExpValue} TP");
                      player.Experience += enemy.ExpValue;
                      if (player.Experience >= 100)
                      {
                          player.Level++;
                          player.MaxHealth += 20;
                          player.Health = player.MaxHealth;
                          player.AttackPower += 5;
                          player.Experience = 0;
                          AddLog("SEVİYE ATLADINIZ! Tüm canınız doldu ve güçlendiniz.");
                      }
                  }
                  Thread.Sleep(400);
              }

              if (player.Health <= 0) isRunning = false;
          }
      }

      class Program
      {
          static void Main()
          {
              Console.Clear();
              Game game = new Game();
              game.Start();
          }
      }
  }
