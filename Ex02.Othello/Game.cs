using System;
using System.Collections.Generic;

namespace Ex02.Othello
{
	public class Game
	{
		private readonly Player r_BlackPlayer;
		public Player BlackPlayer
		{
			get
			{
				return r_BlackPlayer;
			}
		}

		private readonly Player r_WhitePlayer;
		public Player WhitePlayer
		{
			get
			{
				return r_WhitePlayer;
			}
		}

		public Player CurrentPlayer { get; private set; }

		public Player OtherPlayer
		{
			get
			{
				return CurrentPlayer == BlackPlayer ? WhitePlayer : BlackPlayer;
			}
		}
		
		private readonly GameBoard r_Board;
		public GameBoard Board
		{
			get
			{
				return r_Board;
			}
		}

		public Player Winner 
		{
			get
			{
				Player winner;

				if (IsRunning)
				{
					throw new InvalidOperationException("There is now winner, Game is still in progress.");
				}

				if (BlackPlayer.Score > WhitePlayer.Score)
				{
					winner = BlackPlayer;
				}
				else if (WhitePlayer.Score > BlackPlayer.Score)
				{
					winner = WhitePlayer;
				}
				else
				{
					winner = null;
				}

				return winner;
			}
		}

		public bool IsRunning { get; private set; }
		private eIterationResult m_LastIterationResult = eIterationResult.Success;

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

		public Game(PlayerInfo i_BlackPlayer, PlayerInfo i_WhitePlayer, int i_BoardSize)
		{
			r_BlackPlayer = new Player(i_BlackPlayer.Name, i_BlackPlayer.Controller, ePlayerColor.Black);
			r_WhitePlayer = new Player(i_WhitePlayer.Name, i_WhitePlayer.Controller, ePlayerColor.White);
			r_BlackPlayer.Score = r_WhitePlayer.Score = 2;
			CurrentPlayer = r_BlackPlayer;
			r_Board = new GameBoard(i_BoardSize);
			IsRunning = true;
		}

		private void switchPlayer()
		{
			if (CurrentPlayer == r_BlackPlayer)
			{
				CurrentPlayer = r_WhitePlayer;
			}
			else
			{
				CurrentPlayer = r_BlackPlayer;
			}
		}

		private eIterationResult handleQuitAction(QuitAction i_Action)
		{
			IsRunning = false;
			return eIterationResult.GameQuit;
		}

		private eIterationResult handleAction(PlayerControllerAction i_Action)
		{
			eIterationResult result;

			if (i_Action is QuitAction)
			{
				result = handleQuitAction((QuitAction) i_Action);
			}
			else if (i_Action is PerformMoveAction)
			{
				result = handlePerfromMoveAction((PerformMoveAction) i_Action);
			}
			else
			{
				throw new ArgumentException("Got unknown action from user");
			}

			return result;
		}

		private eIterationResult handlePerfromMoveAction(PerformMoveAction i_Action)
		{
			eIterationResult result;
			BoardPosition newTokenPosition = i_Action.Position;

			if (!r_Board.IsValidPosition(newTokenPosition))
			{
				result = eIterationResult.MoveOutOfBounds;
			}
			else if (!r_Board[newTokenPosition].IsEmpty)
			{
				result = eIterationResult.IllegalMove;
			}
			else
			{
				uint cellsFlipped = performMove(newTokenPosition);
				if (cellsFlipped == 0)
				{
					result = eIterationResult.IllegalMove;
				}
				else
				{
					OtherPlayer.Score -= cellsFlipped;
					CurrentPlayer.Score += cellsFlipped + 1;
					switchPlayer();
					result = eIterationResult.Success;
				}
			}

			return result;
		}

		public eIterationResult Iterate()
		{
			eIterationResult result;
			RankedMove[] rankedMoves = GetLegalMoves();

			if (rankedMoves.Length == 0)
			{
				if (m_LastIterationResult == eIterationResult.NoPossibleMoves)
				{
					IsRunning = false;
					result = eIterationResult.GameOver;
				}
				else
				{
					result = eIterationResult.NoPossibleMoves;
				}
			}
			else
			{
				result = handleAction(CurrentPlayer.GetAction(rankedMoves));
			}

			m_LastIterationResult = result;
			return result;
		}

		private uint performMove(BoardPosition i_Position, bool i_FlipTiles = true)
		{
			uint totalFilpped = 0;

			foreach (BoardPosition offset in sr_CardinalDirections)
			{
				uint numFilpped;
				if (checkDirection(CurrentPlayer.Color, i_Position + offset, offset, i_FlipTiles, out numFilpped))
				{
					totalFilpped += numFilpped;
				}
			}

			if (i_FlipTiles && totalFilpped > 0)
			{
				r_Board[i_Position] = new GameBoard.Cell(CurrentPlayer.Color);
			}

			return totalFilpped;
		}

		private bool checkDirection(ePlayerColor i_PlayerColor, BoardPosition i_CurrentPosition, BoardPosition i_Offset, bool i_FilpTiles, out uint o_NumFlipped)
		{
			bool isValid = true;
			o_NumFlipped = 0;

			if (!r_Board.IsValidPosition(i_CurrentPosition))
			{
				isValid = false;
			}
			else
			{
				GameBoard.Cell currentCell = r_Board[i_CurrentPosition];
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
						r_Board[i_CurrentPosition] = currentCell;
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

		public RankedMove[] GetLegalMoves()
		{
			List<RankedMove> legalMoves = new List<RankedMove>();

			for (int x = 0; x < Board.Size; x++)
			{
				for (int y = 0; y < Board.Size; y++)
				{
					if (!Board[x, y].IsEmpty)
					{
						continue;
					}

					BoardPosition position = new BoardPosition(x, y);
					const bool v_FlipTiles = true;
					uint score = performMove(position, !v_FlipTiles);
					if (score > 0)
					{
						legalMoves.Add(new RankedMove(position, score));
					}
				}
			}

			return legalMoves.ToArray();
		}
	}
}
