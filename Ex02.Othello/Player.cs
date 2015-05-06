using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello
{
	public class Player
	{
		public uint Score { get; internal set; }
		public ePlayerColor Color { get; private set; }
		public string Name { get; private set; }
		private readonly IPlayerController r_Controller;

		internal Player(string i_Name, IPlayerController i_Controller, ePlayerColor i_Color)
		{
			r_Controller = i_Controller;
			Color = i_Color;
			Name = i_Name;
		}

		public PlayerControllerAction GetAction(RankedMove[] i_LegalMoves)
		{
			return r_Controller.GetAction(i_LegalMoves);
		}
	}
}
