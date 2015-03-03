using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SnakeConsoleApplication;

namespace SnakeConsoleApplication
{
	class Bonus
	{
		public Cell c;
		public Cell buf;
		public int[] dx = new int[8]{1, 1, -1, -1, 0, 0, 1, -1};
		public int[] dy = new int[8]{1, -1, 1, -1, 1, -1, 0, 0};
		public Bonus()
		{
			c = new Cell (1, 1);
			Random r;
			do {	
				r = new Random (DateTime.Now.Second);
				c = new Cell (r.Next (1, MainProgram.H - 1), r.Next (1, MainProgram.W - 1));
			} while (Snake.onSnake (c) || MainProgram.food.c.equal(c));

		}
			
		public void Move() {

			buf = new Cell (this.c.x, this.c.y);
			Random rnd = new Random (DateTime.Now.Second);
			dx = dx.OrderBy (x => rnd.Next()).ToArray();
			dy = dy.OrderBy (x => rnd.Next()).ToArray();
			MainProgram.EditCell (buf, ConsoleColor.Black, " ");

			for (int i = 0; i < 8; i++)
			{
				buf.x = this.c.x + dx[i];
				buf.y = this.c.y + dy[i];
				if (MainProgram.food.c.equal(buf) == false && MainProgram.onWall(buf) == false)
				if (Snake.onSnake(buf) == false)
				{
					this.c.x = buf.x;
					this.c.y = buf.y;
					break;
				}
			}
			MainProgram.EditCell (this.c, ConsoleColor.Blue, "0");
		}


		public void Draw() {
			MainProgram.EditCell (c, ConsoleColor.Blue, "0");
		}
	}
}

