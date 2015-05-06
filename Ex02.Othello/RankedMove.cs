using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello
{
	public struct RankedMove
	{
		public BoardPosition Position { get; private set; }
		public uint Score { get; private set; }

		public RankedMove(BoardPosition i_Position, uint i_Score) : this()
		{
			Position = i_Position;
			Score = i_Score;
		}
	}
}
