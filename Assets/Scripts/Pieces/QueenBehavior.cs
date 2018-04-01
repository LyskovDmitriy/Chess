using System.Collections.Generic;
using UnityEngine;

public class QueenBehavior : LinearMovementBehavior 
{
	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		AddDiagonalLineMoves();
		AddStraightLineMoves();
	}
}
