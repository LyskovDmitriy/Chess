using UnityEngine;

public class RookBehavior : LinearMovementBehavior 
{

	public bool HasMoved { get { return hasMoved; } }


	private bool hasMoved = false;


	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		AddAvailableStraightLineMoves();
	}


	public override void CalculateMovesForAttackMap()
	{
		AddStraightLineMovesToAttackMap();
	}


	public override void ReactToMovement(Coordinates newCoordinates)
	{
		hasMoved = true;
	}
}
