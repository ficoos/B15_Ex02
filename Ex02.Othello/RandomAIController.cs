using System;

namespace Ex02.Othello
{
	public class RandomAiController : IPlayerController
	{
		private readonly Random r_Random;

		public RandomAiController()
		{
			r_Random = new Random();
		}

		public RandomAiController(int i_Seed)
		{
			r_Random = new Random(i_Seed);
		}

		public PlayerControllerAction GetAction(RankedMove[] i_LegalMoves)
		{
			RankedMove randomMove = i_LegalMoves[r_Random.Next(i_LegalMoves.Length)];
			return new PerformMoveAction(randomMove.Position);
		}
	}
}