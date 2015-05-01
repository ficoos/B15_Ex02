using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello
{
	public class Game
	{
		public IPlayer BlackPlayer { get; private set; }
		public IPlayer WhitePlayer { get; private set; }
		public ePlayerColor CurrentPlayerColor {get; private set; }
		public IPlayer CurrentPlayer {
			get {
				if (CurrentPlayerColor == ePlayerColor.Black) {
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

		public Game(IPlayer i_BlackPlayer, IPlayer i_WhitePlayer, int i_BoardSize) 
		{
			BlackPlayer = i_BlackPlayer;
			WhitePlayer = i_WhitePlayer;
			CurrentPlayerColor = ePlayerColor.Black;
			r_board = new GameBoard(i_BoardSize);
			IsRunning = true;
		}

		private void switchPlayer() {
			if (CurrentPlayerColor == ePlayerColor.Black)
			{
				CurrentPlayerColor = ePlayerColor.White;
			} else {
				CurrentPlayerColor = ePlayerColor.Black;
			}
		}

		public void Iterate()
		{
			BoardPosition newTokenPosition = CurrentPlayer.GetMove();
		}

		public bool IsRunning { get; private set; }
	}
}
