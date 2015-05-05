using System;
using System.Collections.Generic;
using System.Text;


namespace Ex02.Othello
{
	public class Game
	{
		public IPlayer BlackPlayer { get; private set; }
		public IPlayer WhitePlayer { get; private set; }
		public ePlayerColor CurrentPlayerColor { get; private set; }
		public IPlayer CurrentPlayer
		{
			get
			{
				if (CurrentPlayerColor == ePlayerColor.Black)
				{
					return BlackPlayer;
				}
				else
				{
					return WhitePlayer;
				}
			}
		}
		private readonly GameBoard r_board;

		public GameBoard Board
		{
			get
			{
				return r_board;
			}
		}

		private static readonly BoardPosition[] sr_CardinalDirections = {
			new BoardPosition(0, 1), // North
			new BoardPosition(1, 1), // NorthEast
			new BoardPosition(1, 0), // East
			new BoardPosition(1, -1), // SouthEast
			new BoardPosition(0, -1), // South
			new BoardPosition(-1, -1), // SouthWest
			new BoardPosition(-1, 0), // West
			new BoardPosition(-1, 1) // NorthWest
		};

		public Game(IPlayer i_BlackPlayer, IPlayer i_WhitePlayer, int i_BoardSize)
		{
			BlackPlayer = i_BlackPlayer;
			WhitePlayer = i_WhitePlayer;
			CurrentPlayerColor = ePlayerColor.Black;
			r_board = new GameBoard(i_BoardSize);
			IsRunning = true;
		}

		private void switchPlayer()
		{
			if (CurrentPlayerColor == ePlayerColor.Black)
			{
				CurrentPlayerColor = ePlayerColor.White;
			}
			else
			{
				CurrentPlayerColor = ePlayerColor.Black;
			}
		}

		public eIterationResult Iterate()
		{
			eIterationResult result = eIterationResult.Success;

			BoardPosition newTokenPosition = CurrentPlayer.GetMove();
			if (!r_board.IsValidPosition(newTokenPosition))
			{
				result = eIterationResult.MoveOutOfBounds;
			}
			else if (!r_board[newTokenPosition].IsEmpty)
			{
				result = eIterationResult.IllegalMove;
			}
			else if (performMove(newTokenPosition) == 0)
			{
				result = eIterationResult.IllegalMove;
			}
			else
			{
				switchPlayer();
			}

			return result;
		}

		private int performMove(BoardPosition i_Position, bool i_FlipTiles = true)
		{
			int totalFilpped = 0;

			foreach (BoardPosition offset in sr_CardinalDirections)
			{
				int numFilpped;
				if (checkDirection(CurrentPlayerColor, i_Position + offset, offset, i_FlipTiles, out numFilpped))
				{
					totalFilpped += numFilpped;
				}
			}

			if (i_FlipTiles && totalFilpped > 0)
			{
				r_board[i_Position] = new GameBoard.Cell(CurrentPlayerColor);
			}


			return totalFilpped;
		}

		public bool checkDirection(ePlayerColor i_PlayerColor, BoardPosition i_CurrentPosition, BoardPosition i_Offset, bool i_FilpTiles, out int o_NumFlipped)
		{
			bool isValid = true;
			o_NumFlipped = -1;

			if (!r_board.IsValidPosition(i_CurrentPosition))
			{
				isValid = false;
			}
			else
			{
				GameBoard.Cell currentCell = r_board[i_CurrentPosition];
				if (currentCell.IsEmpty)
				{
					isValid = false;
				}
				else if (currentCell.ContainsColor(i_PlayerColor))
				{
					o_NumFlipped = 0;
				}
				else if (checkDirection(i_PlayerColor, i_CurrentPosition + i_Offset, i_Offset, i_FilpTiles, out o_NumFlipped))
				{
					if (i_FilpTiles)
					{
						currentCell.Flip();
						r_board[i_CurrentPosition] = currentCell;
					}

					o_NumFlipped++;
				}
				else
				{
					isValid = false;
				}
			}

			return isValid;
		}

		public bool HasValidMove(ePlayerColor i_Player)
		{
			return true;
		}

		public void AddPointsToPlayer(int i_Score, String i_PlayerType)
		{
			CurrentPlayer.Score += i_Score;
		}




		public bool IsRunning { get; private set; }
	}

}
