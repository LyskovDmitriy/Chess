using System.Collections.Generic;
using UnityEngine;
using System;

public class KingBehavior : BaseBehavior 
{

	public bool HasMoved { get { return hasMoved; } }
	public CastlingController castlingController;


	private bool hasMoved = false;


	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		CalculateAvailableMoves(CheckBeforeAddingMoveToAvailable, AddToAvailableMoves);
	}


	public override void CalculateMovesForAttackMap()
	{
		CalculateAvailableMoves(SquareIsWithinBoard, AddToAttackMap);
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


	protected override bool CheckBeforeAddingMoveToAvailable(Coordinates movementCoordinates)
	{
		if (SquareIsEmptyOrOccupiedByEnemy(movementCoordinates) )
		{
			if (enemyAttackMap[movementCoordinates].isUnderAttack)
			{
				return false;
			}

			Coordinates currentCoordinates = piece.Coordinates;
			//square is not under attack won't be after movement
			if (!enemyAttackMap[currentCoordinates].isUnderAttack)
			{
				return true;
			}
			//square is not under attack but can be after movement
			SquareAttackInfo squareAttack = enemyAttackMap[currentCoordinates];

			for (int i = 0; i < squareAttack.attackDirections.Count; i++)
			{
				PieceType attackingPiece = squareAttack.attackingPieces[i].type;

				if (attackingPiece == PieceType.King || attackingPiece == PieceType.Knight || attackingPiece == PieceType.Pawn)
				{
					//for these pieces attack direction doesn't matter, 
					//because after attacked piece's movement they don't change attacked squares, 
					//unlike pieces that attack linearly
					continue;
				}
				else
				{
					Coordinates movementDirection = (movementCoordinates - piece.Coordinates).NormalizedDirection;
					if (squareAttack.attackDirections[i] == movementDirection)
					{
						return false;
					}
				}
			}

			return true;
		}
		return false;
	}


	private void CalculateAvailableMoves(Func<Coordinates, bool> squareCheckBeforeAdding, Action<Coordinates> addMove)
	{
		Coordinates currentCoordinates = piece.Coordinates;

		for (int deltaX = -1; deltaX < 2; deltaX++)
		{
			for (int deltaY = -1; deltaY < 2; deltaY++)
			{
				if ((deltaX == 0) && (deltaY == 0))
				{
					continue;
				}

				Coordinates attackCoordinates = currentCoordinates + new Coordinates(deltaX, deltaY);
				if (squareCheckBeforeAdding(attackCoordinates))
				{
					addMove(attackCoordinates);
				}
			}
		}
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
