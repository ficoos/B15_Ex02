using System;

namespace Ex02.Othello
{
	public enum eIterationResult
	{
		Success,
		MoveOutOfBounds,
		IllegalMove,
		GameOver,
		GameQuit,
		NoPossibleMoves
	}
}
