using System.Collections.Generic;
using UnityEngine;

public class LinearMovementBehavior : BaseBehavior 
{
	protected void AddMovesInDirection(Coordinates delta)
	{
		for (Coordinates movementCoordinates = piece.Coordinates + delta;
			SquareIsWithinBoard(movementCoordinates);
			movementCoordinates += delta)
		{
			if (SquareIsEmpty(movementCoordinates))
			{
				availableMoves.Add(movementCoordinates);
			}
			else
			{
				if (SquareIsOccupiedByEnemy(movementCoordinates))
				{	
					availableMoves.Add(movementCoordinates);
					break;
				}
				else
				{
					break;
				}
			}

		}
	}


	protected void AddDiagonalLineMoves()
	{
		AddMovesInDirection(Coordinates.LeftUp);
		AddMovesInDirection(Coordinates.RightUp);
		AddMovesInDirection(Coordinates.RightDown);
		AddMovesInDirection(Coordinates.LeftDown);
	}


	protected void AddStraightLineMoves()
	{
		AddMovesInDirection(Coordinates.Left);
		AddMovesInDirection(Coordinates.Up);
		AddMovesInDirection(Coordinates.Right);
		AddMovesInDirection(Coordinates.Down);
	}
}
