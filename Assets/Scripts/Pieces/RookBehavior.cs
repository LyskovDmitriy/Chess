using System.Collections.Generic;
using UnityEngine;

public class RookBehavior : LinearMovementBehavior 
{

	public bool HasMoved { get { return hasMoved; } }


	private bool hasMoved = false;


	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		AddStraightLineMoves();
	}


	public override void ReactToMovement(Coordinates newCoordinates)
	{
		hasMoved = true;
	}
}
