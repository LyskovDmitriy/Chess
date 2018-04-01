using System.Collections.Generic;
using UnityEngine;

public class KnightBehavior : BaseBehavior 
{

	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		Coordinates currentCoordinates = piece.Coordinates;

		for (int deltaX = -2; deltaX < 3; deltaX++)
		{
			for (int deltaY = -2; deltaY < 3; deltaY++)
			{
				int absDeltaX = Mathf.Abs(deltaX);
				int absDeltaY = Mathf.Abs(deltaY);

				if (((absDeltaX == 2) && (absDeltaY == 1)) 
					|| ((absDeltaX == 1) && (absDeltaY == 2)))
				{
					Coordinates movementCoordinates = currentCoordinates + new Coordinates(deltaX, deltaY);

					if (SquareIsEmptyOrOccupiedByEnemy(movementCoordinates))
					{
						availableMoves.Add(movementCoordinates);
					}
				}
			}
		}
	}



}
