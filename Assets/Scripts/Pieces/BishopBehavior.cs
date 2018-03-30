using System.Collections.Generic;
using UnityEngine;

public class BishopBehavior : BaseBehavior 
{

	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		AddMovesInDirection(Coordinates.LeftUp);
		AddMovesInDirection(Coordinates.RightUp);
		AddMovesInDirection(Coordinates.RightDown);
		AddMovesInDirection(Coordinates.LeftDown);
	}


	public void AddMovesInDirection(Coordinates delta)
	{
		for (Coordinates currentCoordinates = piece.Coordinates + delta;
			CheckBoard.Instance.IsSquareWithinBoard(currentCoordinates);
				currentCoordinates += delta)
		{
			if (CheckBoard.Instance.IsSquareEmpty(currentCoordinates))
			{
				availableMoves.Add(currentCoordinates);
			}
			else
			{
				if (CheckBoard.Instance[currentCoordinates].IsEnemy(piece))
				{	
					availableMoves.Add(currentCoordinates);
					break;
				}
				else
				{
					break;
				}
			}

		}
	}
}
