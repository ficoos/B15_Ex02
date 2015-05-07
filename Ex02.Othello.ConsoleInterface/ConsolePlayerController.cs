using System;

namespace Ex02.Othello.ConsoleInterface
{
	internal class ConsolePlayerController : IPlayerController
	{
		public PlayerControllerAction GetAction(GameState i_GameStateCopy)
		{
			BoardPosition[] legalMoves = i_GameStateCopy.GetLegalMoves();
			const string v_QuitString = "Q";
            PlayerControllerAction action = null;
			while (action == null)
			{
				Console.Write(
					"Please enter your move, possible moves are [{0}]: ",
					string.Join(", ", Array.ConvertAll(legalMoves, i_Input => i_Input.ToString())));
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
