using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello.ConsoleInterface
{
	class ConsolePlayer : IPlayer
	{
        public int Score { get; set; }

		public BoardPosition GetMove()
		{
            bool parsingSuceeded = false;
            BoardPosition newPosition = new BoardPosition();

            while (!parsingSuceeded)
            {
                Console.WriteLine("Please enter your move (eg. A5):");
                String strMove = Console.ReadLine();
                parsingSuceeded = BoardPosition.TryParse(strMove, out newPosition);
                if (!parsingSuceeded)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }

            return newPosition;
        }
		
	}
}
