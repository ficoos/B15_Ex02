using System;
using System.Collections.Generic;
using Ex02.ConsoleUtils;

namespace Ex02.Othello.ConsoleInterface
{
	internal class ConsoleFrontEnd
	{
		private static readonly IDictionary<ePlayerColor, string> sr_ColorRepresentation = new Dictionary<ePlayerColor, string>
																								{
																									{ ePlayerColor.Black, "X" },
																									{ ePlayerColor.White, "O" }
																								};

		private Game m_CurrentGame;

		private bool m_IsRunning;

		public ConsoleFrontEnd()
		{
			m_IsRunning = true;
		}

		private bool shouldPlayAnotherGame()
		{
			bool shouldPlay = m_IsRunning;
			if (shouldPlay)
			{
				shouldPlay = UserInputHelper.AskYesNoQuestion("Would you like to play another game?");
			}

			return shouldPlay;
		}

		private PlayerInfo getBlackPlayerInformation()
		{
			string name = UserInputHelper.AskForNonEmptyString("Please enter the black player's name");
			return new PlayerInfo(name, new ConsolePlayerController());
		}

		private PlayerInfo getWhitePlayerInformation()
		{
			string name;
			IPlayerController controller;

			if (UserInputHelper.AskYesNoQuestion("Would you like to play against the AI"))
			{
				uint searchDepth = UserInputHelper.SelectFromList("Please select difficulty level", new uint[] { 1, 2, 3, 4, 5 });
				name = string.Format("AI Level {0}", searchDepth);
				controller = new MinMaxAiController(searchDepth - 1);
			}
			else
			{
				name = UserInputHelper.AskForNonEmptyString("Please enter the white player's name");
				controller = new ConsolePlayerController();
			}
			
			return new PlayerInfo(name, controller);
		}

		private void playGame()
		{
			int boardSize = UserInputHelper.SelectFromList("Please select a valid board size", GameBoard.ValidBoardSizes);
			PlayerInfo blackPlayerInfo = getBlackPlayerInformation();
			PlayerInfo whitePlayerInfo = getWhitePlayerInformation();
			m_CurrentGame = new Game(blackPlayerInfo, whitePlayerInfo, boardSize);
			drawGameState();
			while (m_CurrentGame.IsRunning)
			{
				eIterationResult lastIterationResult = this.m_CurrentGame.Iterate();
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
				}
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

			for (int i = 0; i < board.Size; i++)
			{
				Console.Write(" ");
				Console.Write(headerLetter);
				Console.Write("  ");
				headerLetter++;
			}

			Console.WriteLine();

			drawSeparatorLine();
			for (int y = 0; y < board.Size; y++ )
			{
				Console.Write(" {0} |", (char)(BoardPosition.FirstDigit + y));
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
				this.drawSeparatorLine();
			}
		}

		private void drawSeparatorLine()
		{
			Console.Write("   ");
			int lineLength = (m_CurrentGame.Board.Size * 4) + 1;
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
				m_CurrentGame.BlackPlayer.Score,
				m_CurrentGame.WhitePlayer.Score,
				m_CurrentGame.CurrentPlayer.Name,
				sr_ColorRepresentation[ePlayerColor.Black],
				sr_ColorRepresentation[ePlayerColor.White],
				m_CurrentGame.BlackPlayer.Name,
				m_CurrentGame.WhitePlayer.Name);
		}
		
		public void Run()
		{
			while (m_IsRunning)
			{
				playGame();
				m_IsRunning = shouldPlayAnotherGame();
			}
		}
	}
}
