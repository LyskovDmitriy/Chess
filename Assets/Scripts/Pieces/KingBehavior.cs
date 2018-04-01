using System.Collections.Generic;
using UnityEngine;

public class KingBehavior : BaseBehavior 
{

	public bool HasMoved { get { return hasMoved; } }
	public CastlingController castlingController;


	private bool hasMoved = false;


	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		Coordinates currentCoordinates = piece.Coordinates;

		for (int deltaX = -1; deltaX < 2; deltaX++)
		{
			for (int deltaY = -1; deltaY < 2; deltaY++)
			{
				if ((deltaX == 0) && (deltaY == 0))
				{
					continue;
				}

				Coordinates movementCoordinates = currentCoordinates + new Coordinates(deltaX, deltaY);
				if (SquareIsEmptyOrOccupiedByEnemy(movementCoordinates))
				{
					availableMoves.Add(movementCoordinates);
				}
			}
		}
	}


	public void AllowCastling(Coordinates direction)
	{
		availableMoves.Add(piece.Coordinates + direction * 2);
	}


	public override void ReactToMovement(Coordinates newCoordinates)
	{
		if (isPerformingCastling(newCoordinates)) 
		{
			castlingController.MoveCorrespondingRookInCastling(newCoordinates - piece.Coordinates);
		}
		hasMoved = true;
	}


	private bool isPerformingCastling(Coordinates newCoordinates)
	{
		if (Mathf.Abs(piece.Coordinates.x - newCoordinates.x) == 2) 
		{
			return true;
		}

		return false;
	}
}
