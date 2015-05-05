using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello
{
	public interface IPlayer
	{
		BoardPosition GetMove();
		int Score { get; set; }
	}
}
