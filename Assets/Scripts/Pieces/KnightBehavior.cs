using System.Collections.Generic;
using UnityEngine;

public class KnightBehavior : BaseBehavior 
{

	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		Coordinates currentCoordinates = piece.Coordinates;
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(-1, 2));
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(1, 2));
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(-2, 1));
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(2, 1));
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(-2, -1));
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(2, -1));
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(-1, -2));
		CheckIfValidAndAddToMoves(currentCoordinates + new Coordinates(1, -2));
	}


	private void CheckIfValidAndAddToMoves(Coordinates coord)
	{
		if (CheckBoard.Instance.IsSquareWithinBoard(coord))
		{
			if (CheckBoard.Instance.IsSquareEmpty(coord) || CheckBoard.Instance[coord].IsEnemy(piece))
			{
				availableMoves.Add(coord);
			}
		}
	}
}
