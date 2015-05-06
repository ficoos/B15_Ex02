namespace Ex02.Othello
{
	public sealed class PerformMoveAction : PlayerControllerAction
	{
		public BoardPosition Position { get; private set; }

		public PerformMoveAction(BoardPosition i_Position)
		{
			Position = i_Position;
		}
	}
}