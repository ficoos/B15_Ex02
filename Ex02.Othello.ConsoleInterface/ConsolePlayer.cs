using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello.ConsoleInterface
{
	class ConsolePlayer : IPlayer
	{
		public BoardPosition GetMove()
		{
			Console.WriteLine("Please enter your move (eg. A5):");
			Console.ReadLine();
			return new BoardPosition();
		}
	}
}
