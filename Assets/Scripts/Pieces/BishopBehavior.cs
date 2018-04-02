using System.Collections.Generic;
using UnityEngine;

public class BishopBehavior : LinearMovementBehavior 
{
	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		AddAvailableDiagonalLineMoves();
	}


	public override void CalculateMovesForAttackMap()
	{
		AddDiagonalLineMovesToAttackMap();
	}
}
