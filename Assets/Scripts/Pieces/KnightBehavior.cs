using System.Collections.Generic;
using UnityEngine;
using System;

public class KnightBehavior : BaseBehavior 
{

	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		CalculateAvailableMoves(SquareIsEmptyOrOccupiedByEnemy, AddToAvailableMoves);
	}


	public override void CalculateMovesForAttackMap()
	{
		CalculateAvailableMoves(SquareIsWithinBoard, AddToAttackMap);
	}


	private void CalculateAvailableMoves(Func<Coordinates, bool> squareCheckBeforeAdding, Action<Coordinates> addMove)
	{
		Coordinates currentCoordinates = piece.Coordinates;

		for (int deltaX = -2; deltaX < 3; deltaX++)
		{
			if (deltaX == 0)
			{
				continue;
			}

			for (int deltaY = -2; deltaY < 3; deltaY++)
			{
				if (deltaY == 0)
				{
					continue;
				}

				int absDeltaX = Mathf.Abs(deltaX);
				int absDeltaY = Mathf.Abs(deltaY);

				if (((absDeltaX == 2) && (absDeltaY == 1)) 
					|| ((absDeltaX == 1) && (absDeltaY == 2)))
				{
					Coordinates movementCoordinates = currentCoordinates + new Coordinates(deltaX, deltaY);
					if (squareCheckBeforeAdding(movementCoordinates))
					{
						addMove(movementCoordinates);
					}
				}
			}
		}
	}
}
