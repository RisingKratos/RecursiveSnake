using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeConsoleApplication
{
	class Snake
	{
		public int ID, score;
		public List<Cell> body;
		public Cell dir;
		public Snake(List<Cell> body){
			this.body = body;
		}

		public void Draw() {
			Console.ForegroundColor = ConsoleColor.Yellow;
			foreach (Cell s in body)
				MainProgram.EditCell(s, ConsoleColor.Green, "+");
			MainProgram.EditCell(body[0], ID == 1 ? ConsoleColor.DarkMagenta : ConsoleColor.Yellow, "%");
		}

		public void reDraw(Cell tail, Cell head, Cell newHead)
		{
			MainProgram.EditCell(head, ConsoleColor.Green, "+");
			MainProgram.EditCell(tail, ConsoleColor.Black, " ");
			MainProgram.EditCell(newHead, ID == 1 ? ConsoleColor.DarkMagenta : ConsoleColor.Yellow, "%");

		}

		public void eat(Cell c) {
			MainProgram.EditCell (body[0], ConsoleColor.Green, "+");
			body.Insert(0, c);
			Console.Beep(50, 100);
			MainProgram.food = new Food ();
			score += 5;
		}

		public void take(Cell c) {
			MainProgram.EditCell (body[0], ConsoleColor.Green, "+");
			body.Insert (0, c);
			Console.Beep (50, 100);
			MainProgram.bonus = new Bonus ();
			MainProgram.bonus.Draw ();
			score += 30;
		}

		public bool Change(Cell c) {
			Cell head = (new Cell(body[0].x, body[0].y)) + c;
			if (onSnake (head, this.ID))//ON SNAKE, DIE!
				return false;
			if (MainProgram.onWall (head)) {//ON WALL, TELEPORTATION!
				if (head.x == MainProgram.H) {
					head.x = 1;
					dir = MainProgram.Map1 [ConsoleKey.RightArrow];
				}
				if (head.x == 0) {
					head.x = MainProgram.H - 1;
					dir = MainProgram.Map1 [ConsoleKey.LeftArrow];
				}
				if (head.y == MainProgram.W) {	
					head.y = 1;
					dir = MainProgram.Map1 [ConsoleKey.DownArrow];
				}
				if (head.y == 0) {
					head.y = MainProgram.W - 1;
					dir = MainProgram.Map1 [ConsoleKey.UpArrow];
				}
			}

			if (head.equal(MainProgram.food.c)) { //FRESH MEAT
				eat(MainProgram.food.c);
				return true;
			}
			if (head.equal (MainProgram.bonus.c)) { // GOD MOD ACTIVATED
				take (MainProgram.bonus.c);
				return true;
			}
			//reDraw (body[body.Count() - 2], body[1], head);
			reDraw (body[body.Count() - 1], body[0], head);
			for (int i = body.Count () - 1; i > 0; i--) //I like to MOVE IT MOVE IT
				body [i] = body [i - 1];
			body [0] = head;
			return true;
		}

		public bool Move(ConsoleKey b) {
			if (ID == 1 && MainProgram.Map1.ContainsKey (b)) {
				Cell buf = MainProgram.Map1 [b];
				if (buf.x * dir.x + buf.y * dir.y == 0) // (-1, 0) * (1, 0) = IGNORE, (0, 1) * (1, 0) = ACCEPT;
					dir = buf;
			}
			if (ID == 2 && MainProgram.Map2.ContainsKey (b)) {
				Cell buf = MainProgram.Map2 [b];
				if (buf.x * dir.x + buf.y * dir.y == 0) // (-1, 0) * (1, 0) = IGNORE, (0, 1) * (1, 0) = ACCEPT;
					dir = buf;
			}

			return Change (dir);
		}

		public static bool onSnake(Cell c, int isSnake = 0) {
			foreach (Cell s in MainProgram.snake1.body) {
				if (c.equal (s)) {
					if (isSnake == 2 && !c.equal(MainProgram.snake1.body[0]))
						MainProgram.snake1.score += 20;
					return true;
				}
			}
			foreach (Cell s in MainProgram.snake2.body) {
				if (c.equal (s)) {
					if (isSnake == 1 && !c.equal(MainProgram.snake2.body[0]))
						MainProgram.snake2.score += 20;
					return true;
				}
			}
			return false;
		}
	}
}
