using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello
{
	public interface IPlayerController
	{
		PlayerControllerAction GetAction(RankedMove[] i_LegalMoves);
	}
}
