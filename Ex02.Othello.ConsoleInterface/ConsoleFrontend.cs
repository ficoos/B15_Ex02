using System;
using System.Collections.Generic;
using System.Text;
using Ex02.ConsoleUtils;

namespace Ex02.Othello.ConsoleInterface
{
	class ConsoleFrontend
	{
		private Game m_CurrentGame;

		public ConsoleFrontend()
		{
			bool isQuitting = false;
			while (!isQuitting)
			{
				playGame();
				Console.Write("Would you like to play another game (y/n): ");
			}
			
		}

		private void playGame()
		{
			int boardSize = readBoardSizeFromUser();
			m_CurrentGame = new Game(new ConsolePlayer(), new ConsolePlayer(), boardSize);
			eIterationResult lastIterationResult = eIterationResult.Success;
			while (m_CurrentGame.IsRunning)
			{
				drawGameState(m_CurrentGame);
				switch(lastIterationResult)
				{
					case eIterationResult.IllegalMove:
						Console.WriteLine("The move was illegal, please try again.");
						break;
					case eIterationResult.MoveOutOfBounds:
						Console.WriteLine("The move was out of bounds, please try again.");
						break;
				}
				
				lastIterationResult = m_CurrentGame.Iterate();
			}
		}

		private void drawGameState(Game r_Game)
		{
			Screen.Clear();
			drawHeader();
			drawBoard(r_Game.Board);
		}

		private void drawBoard(GameBoard gameBoard)
		{
			Console.Write("    ");
			char headerLetter = 'A';

			for (int i = 0; i < gameBoard.Size; i++)
			{
				Console.Write(" ");
				Console.Write(headerLetter);
				Console.Write("  ");
				headerLetter++;
			}
			Console.WriteLine();

			drawSperatorLine(gameBoard.Size);
			for (int y = 0; y < gameBoard.Size; y++ )
			{
				Console.Write(" {0} |", y + 1);
				for (int x = 0; x < gameBoard.Size; x++)
				{
					Console.Write(" ");
					switch(gameBoard[x, y].Content)
					{
						case eCellContent.None:
							Console.Write(" ");
							break;
						case eCellContent.Black:
							Console.Write("X");
						break;
						case eCellContent.White:
							Console.Write("O");
						break;
					}

					Console.Write(" |");
				}

				Console.WriteLine();
				drawSperatorLine(gameBoard.Size);
			}
		}

		private static void drawSperatorLine(int length)
		{
			Console.Write("   ");
			int lineLength = (length * 4) + 1;
			for (int i = 0; i < lineLength; i++)
			{
				Console.Write("=");
			}
			Console.WriteLine();
		}

		private void drawHeader()
		{
            
			Console.WriteLine("Amazing Othello. Stats, current player: {0}",m_CurrentGame.CurrentPlayerColor.ToString());
		}

		private int readBoardSizeFromUser()
		{
			const int v_InvalidBoardSize = -1;
			int boardSize = v_InvalidBoardSize;
			bool isValidInput = false;

			while (!isValidInput)
			{
				Console.Write("Please select a valid board size (");
				foreach (int size in GameBoard.ValidBoardSizes)
				{
					bool is_first_item = size == GameBoard.ValidBoardSizes[0];
					if (!is_first_item)
					{
						Console.Write(", ");
					}

					Console.Write(size);
				}

				Console.Write("):");
				string strSize = Console.ReadLine();
				if (!int.TryParse(strSize, out boardSize))
				{
					boardSize = v_InvalidBoardSize;
					Console.WriteLine("Input is not an integer, please try again");
					continue;
				}

				if (!GameBoard.IsValidBoardSize(boardSize))
				{
					boardSize = v_InvalidBoardSize;
					Console.WriteLine("Board size not supported");
					continue;
				}

				isValidInput = true;
			}

			return boardSize;
		}

		public void Run()
		{
			
		}
	}
}
