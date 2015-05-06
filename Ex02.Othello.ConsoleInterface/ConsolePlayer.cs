using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello.ConsoleInterface
{
	class ConsolePlayerController : IPlayerController
	{
		public PlayerControllerAction GetAction(RankedMove[] i_LegalMoves)
		{
			const string v_QuitString = "Q";

            PlayerControllerAction action = null;

			while (action == null)
            {
                Console.Write("Please enter your move, possible moves are [{0}]: ",
					string.Join(", ", Array.ConvertAll(i_LegalMoves, i_Input => i_Input.Position.ToString())));
                string strMove = Console.ReadLine();
	            if (strMove == v_QuitString)
	            {
		            action = new QuitAction();
	            }
				else
	            {
		            BoardPosition newPosition;
		            if (BoardPosition.TryParse(strMove, out newPosition))
		            {
			            action = new PerformMoveAction(newPosition);
		            }
		            else
		            {
			            Console.WriteLine("Invalid input. Please try again.");
		            }
	            }
            }

            return action;
        }
		
	}
}
