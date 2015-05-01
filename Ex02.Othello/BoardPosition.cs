using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello
{
    public struct BoardPosition
    {
		int X { get; set; }
		int Y { get; set; }

		public BoardPosition(int i_X, int i_Y) : this()
		{
			X = i_X;
			Y = i_Y;
		}
    }
}
