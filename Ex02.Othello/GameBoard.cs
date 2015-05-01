using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello
{
	public class GameBoard : ICloneable
	{
		private static readonly int[] sr_ValidBoardSizes = new int[]{6, 8};

		public static int[] ValidBoardSizes { 
			get {
				return sr_ValidBoardSizes;
			}
		}

		public struct Cell
		{
			public BoardPosition Position { get; private set; }
			public eCellContent Content { get; set; }

			public Cell(BoardPosition i_Position, eCellContent i_Content = eCellContent.None) : this()
			{
				Position = i_Position;
				Content = i_Content;
			}

			public static bool TryParse(string i_Input, out BoardPosition o_Position)
			{
				//TODO
				i_Input = i_Input.Trim();
				if (i_Input.Length != 2)
				{
					o_Position = new BoardPosition(0, 0);
					return false;
				}

				return true;
			}
		}

		private readonly Cell[,] r_BoardMatrix;
		public int Size { get; private set; }

		public static bool IsValidBoardSize(int i_Size)
		{
			const int v_ItemNotFound = -1;
			return Array.IndexOf(sr_ValidBoardSizes, i_Size) != v_ItemNotFound;
		}

		public GameBoard(int i_Size)
		{
			if (!IsValidBoardSize(i_Size))
			{
				throw new ArgumentOutOfRangeException("i_Size", string.Format("{0} is not a valid board size", i_Size));
			}

			r_BoardMatrix = new Cell[i_Size, i_Size];
			initializeBoardMatrix();
			Size = i_Size;
		}

		public Cell this[int x, int y]
		{
			get {
				return r_BoardMatrix[x, y];
			}
		}

		private void initializeBoardMatrix()
		{
			for (int x = 0; x < r_BoardMatrix.GetLength(0); x++) {
				for (int y = 0; y < r_BoardMatrix.GetLength(1); y++)
				{
					r_BoardMatrix[x, y] = new Cell(new BoardPosition(x, y));
				}
			}
		}

		public object Clone()
		{
			//TODO
			return this;
		}
	}
}
