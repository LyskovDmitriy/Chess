using UnityEngine;
using System;

public class KnightBehavior : BaseBehavior 
{

	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		CalculateMoves(SquareIsEmptyOrOccupiedByEnemy, AddToAvailableMoves);
	}


	public override void CalculateMovesForAttackMap()
	{
		CalculateMoves(SquareIsWithinBoard, AddToAttackMap);
	}


	private void CalculateMoves(Func<Coordinates, bool> squareCheckBeforeAdding, Action<Coordinates> addMove)
	{
		Coordinates currentCoordinates = piece.Coordinates;

		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(-1, 2), squareCheckBeforeAdding, addMove);
		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(1, 2), squareCheckBeforeAdding, addMove);
		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(-2, 1), squareCheckBeforeAdding, addMove);
		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(2, 1), squareCheckBeforeAdding, addMove);
		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(-2, -1), squareCheckBeforeAdding, addMove);
		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(2, -1), squareCheckBeforeAdding, addMove);
		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(-1, -2), squareCheckBeforeAdding, addMove);
		AddToMovesIfValidCoordinates(currentCoordinates + new Coordinates(1, -2), squareCheckBeforeAdding, addMove);
	}


	private void AddToMovesIfValidCoordinates(Coordinates movementCoordinates, 
		Func<Coordinates, bool> squareCheckBeforeAdding, Action<Coordinates> addMove)
	{
		if (squareCheckBeforeAdding(movementCoordinates))
		{
			addMove(movementCoordinates);
		}
	}
}
