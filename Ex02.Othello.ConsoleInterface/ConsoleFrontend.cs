using System;
using System.Collections.Generic;
using Ex02.ConsoleUtils;

namespace Ex02.Othello.ConsoleInterface
{
	class ConsoleFrontend
	{
		private Game m_CurrentGame;
		private static readonly IDictionary<ePlayerColor, string> sr_ColorRepresentation = new Dictionary<ePlayerColor, string> {
			{ePlayerColor.Black, "X"},
			{ePlayerColor.White, "O"}
		};

		private bool m_IsRunning;

		public ConsoleFrontend()
		{
			m_IsRunning = true;
		}

		private static bool askYesNoQuestion(string i_Question)
		{
			const string v_YesAnswer = "y";
			const string v_NoAnswer = "n";
			bool? result = null;

			while (!result.HasValue)
			{
				Console.Write("{2} ({0}/{1}): ", v_YesAnswer, v_NoAnswer, i_Question);
				string answer = Console.ReadLine().ToLower();
				switch (answer)
				{
					case v_YesAnswer:
						result = true;
						break;
					case v_NoAnswer:
						result = false;
						break;
				}

				if (!result.HasValue)
				{
					Console.WriteLine("Please answer in the from of '{0}' or '{1}", v_YesAnswer, v_NoAnswer);
				}
			}

			return result.Value;
		}

		private bool shouldPlayAnotherGame()
		{
			return askYesNoQuestion("Would you like to play another game?");
		}

		private static string askForNonEmptyString(string i_Question)
		{
			Console.Write("{0}: ", i_Question);
			string answer = Console.ReadLine().Trim();
			while (answer == string.Empty)
			{
				Console.WriteLine("Please enter a non empty string");
				Console.Write("{0}: ", i_Question);
				answer = Console.ReadLine().Trim();
			}

			return answer;
		}

		private PlayerInfo getBlackPlayerInformation()
		{
			string name = askForNonEmptyString("Please enter the black player's name");
			return new PlayerInfo(name, new ConsolePlayerController());
		}

		private PlayerInfo getWhitePlayerInformation()
		{
			string name;
			IPlayerController controller;

			if (askYesNoQuestion("Would you like to play against the AI"))
			{
				name = "Random AI";
				controller = new RandomAiController();
			}
			else
			{
				name = askForNonEmptyString("Please enter the white player's name");
				controller = new ConsolePlayerController();
			}
			
			return new PlayerInfo(name, controller);
		}

		private void playGame()
		{
			int boardSize = readBoardSizeFromUser();
			PlayerInfo blackPlayerInfo = getBlackPlayerInformation();
			PlayerInfo whitePlayerInfo = getWhitePlayerInformation();
			m_CurrentGame = new Game(blackPlayerInfo, whitePlayerInfo, boardSize);
			eIterationResult lastIterationResult = eIterationResult.Success;
			while (m_CurrentGame.IsRunning)
			{
				drawGameState();
				switch(lastIterationResult)
				{
					case eIterationResult.IllegalMove:
						Console.WriteLine("The move was illegal, please try again.");
						break;
					case eIterationResult.MoveOutOfBounds:
						Console.WriteLine("The move was out of bounds, please try again.");
						break;
					case eIterationResult.GameOver:
						Player winner = m_CurrentGame.Winner;
						if (winner == null)
						{
							Console.WriteLine("Game over, it's a tie");
						}
						else
						{
							Console.WriteLine("Game over, {0} wins!!!", winner.Name);	
						}
						break;
					case eIterationResult.GameQuit:
						m_IsRunning = false;
						break;
					case eIterationResult.NoPossibleMoves:
						Console.WriteLine("Skipped player because there were no possible moves");
						break;
				}

				lastIterationResult = m_CurrentGame.Iterate();
			}
		}

		private void drawGameState()
		{
			Screen.Clear();
			drawHeader();
			drawBoard();
		}

		private void drawBoard()
		{
			GameBoard board = m_CurrentGame.Board;
			Console.Write("    ");
			char headerLetter = BoardPosition.FirstLetter;

			for (int i = 0; i <  board.Size; i++)
			{
				Console.Write(" ");
				Console.Write(headerLetter);
				Console.Write("  ");
				headerLetter++;
			}
			Console.WriteLine();

			drawSperatorLine(board.Size);
			for (int y = 0; y < board.Size; y++ )
			{
				Console.Write(" {0} |", (char) (BoardPosition.FirstDigit + y));
				for (int x = 0; x < board.Size; x++)
				{
					Console.Write(" ");
					switch(board[x, y].Content)
					{
						case eCellContent.None:
							Console.Write(" ");
							break;
						case eCellContent.Black:
							Console.Write(sr_ColorRepresentation[ePlayerColor.Black]);
						break;
						case eCellContent.White:
							Console.Write(sr_ColorRepresentation[ePlayerColor.White]);
						break;
					}

					Console.Write(" |");
				}

				Console.WriteLine();
				drawSperatorLine(board.Size);
			}
		}

		private static void drawSperatorLine(int i_Length)
		{
			Console.Write("   ");
			int lineLength = (i_Length * 4) + 1;
			for (int i = 0; i < lineLength; i++)
			{
				Console.Write("=");
			}

			Console.WriteLine();
		}

		private void drawHeader()
		{
			Console.WriteLine(
@"Amazing Othello | {5}[{3}] {0} - {1} {6}[{4}] | Current Player: {2}
",
				m_CurrentGame.BlackPlayer.Score, m_CurrentGame.WhitePlayer.Score, m_CurrentGame.CurrentPlayer.Name,
				sr_ColorRepresentation[ePlayerColor.Black], sr_ColorRepresentation[ePlayerColor.White],
				m_CurrentGame.BlackPlayer.Name, m_CurrentGame.WhitePlayer.Name);
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
					bool isFirstItem = size == GameBoard.ValidBoardSizes[0];
					if (!isFirstItem)
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
			while (m_IsRunning)
			{
				playGame();
				if (m_IsRunning)
				{
					m_IsRunning = shouldPlayAnotherGame();
				}
			}
		}
	}
}
