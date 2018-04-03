using System;

public class LinearMovementBehavior : BaseBehavior 
{
	protected void AddAvailableMovesInDirection(Coordinates delta, 
		Action<Coordinates> addMove, Action<Coordinates> actionAfterFindingOccupiedSquare)
	{
		for (Coordinates movementCoordinates = piece.Coordinates + delta;
			SquareIsWithinBoard(movementCoordinates);
			movementCoordinates += delta)
		{
			if (SquareIsEmpty(movementCoordinates))
			{
				addMove(movementCoordinates);
			}
			else
			{
				actionAfterFindingOccupiedSquare(movementCoordinates);
				break;
			}

		}
	}

	protected void AddToAvailableMovesIfSquareIsOccupiedByEnemy(Coordinates movementCoordinates)
	{
		if (SquareIsOccupiedByEnemy(movementCoordinates))
		{	
			AddToAvailableMoves(movementCoordinates);
		}
	}

	//Functions to fill available moves
	protected void AddAvailableDiagonalLineMoves()
	{
		AddAvailableMovesInDirection(Coordinates.LeftUp, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
		AddAvailableMovesInDirection(Coordinates.RightUp, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
		AddAvailableMovesInDirection(Coordinates.RightDown, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
		AddAvailableMovesInDirection(Coordinates.LeftDown, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
	}


	protected void AddAvailableStraightLineMoves()
	{
		AddAvailableMovesInDirection(Coordinates.Left, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
		AddAvailableMovesInDirection(Coordinates.Up, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
		AddAvailableMovesInDirection(Coordinates.Right, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
		AddAvailableMovesInDirection(Coordinates.Down, AddToAvailableMoves, AddToAvailableMovesIfSquareIsOccupiedByEnemy);
	}

	//Functions to fill attack map
	protected void AddDiagonalLineMovesToAttackMap()
	{
		AddAvailableMovesInDirection(Coordinates.LeftUp, AddToAttackMap, AddToAttackMap);
		AddAvailableMovesInDirection(Coordinates.RightUp, AddToAttackMap, AddToAttackMap);
		AddAvailableMovesInDirection(Coordinates.RightDown, AddToAttackMap, AddToAttackMap);
		AddAvailableMovesInDirection(Coordinates.LeftDown, AddToAttackMap, AddToAttackMap);
	}


	protected void AddStraightLineMovesToAttackMap()
	{
		AddAvailableMovesInDirection(Coordinates.Left, AddToAttackMap, AddToAttackMap);
		AddAvailableMovesInDirection(Coordinates.Up, AddToAttackMap, AddToAttackMap);
		AddAvailableMovesInDirection(Coordinates.Right, AddToAttackMap, AddToAttackMap);
		AddAvailableMovesInDirection(Coordinates.Down, AddToAttackMap, AddToAttackMap);
	}
}
