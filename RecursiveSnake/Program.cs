using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SnakeConsoleApplication
{
    class MainProgram
    {
        public static int W = 20, H = 40;
        public static List<Cell> walls = new List<Cell>();
        public static int score = 0;
        public static Dictionary<ConsoleKey, Cell> Map1 =
            new Dictionary<ConsoleKey, Cell>();
        public static Dictionary<ConsoleKey, Cell> Map2 =
            new Dictionary<ConsoleKey, Cell>();
        public static Food food;
        public static Bonus bonus;
        public static Snake snake1, snake2;

        public static void EditCell(Cell c, ConsoleColor f, String ch)
        {
            Console.SetCursorPosition(c.x, c.y);
            Console.ForegroundColor = f;
            Console.Write(ch);
            Console.SetCursorPosition(H + 10, W / 2 + 5);
        }

        public static void DrawBorder()
        {
            for (int i = 0; i <= W; i++)
            {
                EditCell(new Cell(0, i), ConsoleColor.White, "#");
                EditCell(new Cell(H, i), ConsoleColor.White, "#");
                walls.Add(new Cell(0, i)); walls.Add(new Cell(H, i));
            }
            for (int i = 0; i <= H; i++)
            {
                EditCell(new Cell(i, 0), ConsoleColor.White, "#");
                EditCell(new Cell(i, W), ConsoleColor.White, "#");
                walls.Add(new Cell(i, 0)); walls.Add(new Cell(i, W));
            }
        }

        public static bool onWall(Cell c)
        {
            foreach (Cell w in walls)
            {
                if (w.equal(c))
                    return true;
            }
            return false;
        }

        static List<Cell> InitialBody1()
        {
            List<Cell> sn = new List<Cell>();
            sn.Add(new Cell(10, 10));
            sn.Add(new Cell(9, 10));
            sn.Add(new Cell(8, 10));
            sn.Add(new Cell(7, 10));
            return sn;
        }

        static List<Cell> InitialBody2()
        {
            List<Cell> sn = new List<Cell>();
            sn.Add(new Cell(35, 10));
            sn.Add(new Cell(36, 10));
            sn.Add(new Cell(37, 10));
            sn.Add(new Cell(38, 10));
            return sn;
        }


        public static void Initiate()
        {

            Map1[ConsoleKey.LeftArrow] = new Cell(-1, 0);
            Map1[ConsoleKey.UpArrow] = new Cell(0, -1);
            Map1[ConsoleKey.RightArrow] = new Cell(1, 0);
            Map1[ConsoleKey.DownArrow] = new Cell(0, 1);

            Map2[ConsoleKey.A] = new Cell(-1, 0);
            Map2[ConsoleKey.W] = new Cell(0, -1);
            Map2[ConsoleKey.D] = new Cell(1, 0);
            Map2[ConsoleKey.S] = new Cell(0, 1);

            snake1 = new Snake(InitialBody2());
            snake1.dir = Map1[ConsoleKey.LeftArrow];
            snake1.ID = 1;
            snake2 = new Snake(InitialBody1());
            snake2.dir = Map2[ConsoleKey.D];
            snake2.ID = 2;
            food = new Food();
            bonus = new Bonus();
            Draw();
        }

        public static void drawScore(Cell c)
        {
            EditCell(c, ConsoleColor.White, "PLAYER 1 : " + snake1.score.ToString());
            c.y += 2;
            EditCell(c, ConsoleColor.White, "PLAYER 2 : " + snake2.score.ToString());

        }

        public static void Draw()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            DrawBorder();
            snake1.Draw();
            snake2.Draw();
            food.Draw();
            bonus.Draw();
            drawScore(new Cell(H + 10, W / 2));
        }

        static void Main()
        {
            Initiate();

            ConsoleKeyInfo pressed = Console.ReadKey(true);

            //preDraw ();

            while (true)
            {
                //showTime ();
                if (!snake1.Move(pressed.Key))
                    break;
                if (!snake2.Move(pressed.Key))
                    break;
                bonus.Move();
                Thread.Sleep(100);
                if (Console.KeyAvailable) pressed = Console.ReadKey();
                drawScore(new Cell(H + 10, W / 2));
            }
            Thread.Sleep(1000);
            Console.Clear();
            string over = "DRAW!";
            if (snake1.score > snake2.score)
                over = "PLAYER 1 WON!";
            if (snake1.score < snake2.score)
                over = "PLAYER 2 WON!";

            EditCell(new Cell(Console.WindowWidth / 2 - 5, 10), ConsoleColor.Cyan, over);
            drawScore(new Cell(Console.WindowWidth / 2 - 5, 12));
            EditCell(new Cell(40, 20), ConsoleColor.Cyan, "Press ENTER for restart.");
            Thread.Sleep(1000);
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Main();
                return;
            }
        }
    }
}
