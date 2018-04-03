using System.Collections.Generic;
using UnityEngine;

public class PawnBehavior : BaseBehavior 
{

	public bool MovedTwoSquaresLastTurn { get; private set; }
	public bool HasMoved { get; private set; }


	private Coordinates movementDirection;
	private int lastRowIndex;


	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		if (MovedTwoSquaresLastTurn)
		{
			MovedTwoSquaresLastTurn = false;
		}
			
		TryMoveForward();
		TryAttackInDirection(Coordinates.Left);
		TryAttackInDirection(Coordinates.Right);
	} 


	public override void CalculateMovesForAttackMap()
	{
		AddMoveToAttackMapIfValid(Coordinates.Left);
		AddMoveToAttackMapIfValid(Coordinates.Right);
	}


	public override void ReactToMovement(Coordinates newCoordinates)
	{
		HasMoved = true;
		//if the piece moves diagonally, it may be attacking en passant
		Coordinates currentCoordinates = piece.Coordinates;
		if ((currentCoordinates.x != newCoordinates.x) && (currentCoordinates.y != newCoordinates.y))
		{
			RemoveEnemyPawnIfAttackedEnPassant(Coordinates.Left);
			RemoveEnemyPawnIfAttackedEnPassant(Coordinates.Right);
		}

		int movementDistance = Mathf.Abs((newCoordinates.y - currentCoordinates.y));
		if (movementDistance == 2)
		{
			MovedTwoSquaresLastTurn = true;
		}
		//pawn must be promoted if it reached last row
		if (newCoordinates.y == lastRowIndex)
		{
			PawnPromoter.Instance.RequestPromotion(piece.HoldingPlayer, newCoordinates);
			piece.Remove();
		}
	}


	public override void HighlightAvailableMoves()
	{
		for (int i = 0; i < availableMoves.Count; i++)
		{
			if (piece.Coordinates.x != availableMoves[i].x)
			{
				CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.CanAttack);
			}
			else
			{
				CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.CanMove);
			}
		}
	}		


	public override void Initialize()
	{
		base.Initialize();

		HasMoved = false;
		if (piece.HoldingPlayer.Color == PieceColor.White)
		{
			movementDirection = Coordinates.Up;
			lastRowIndex = 7;
		}
		else
		{
			movementDirection = Coordinates.Down;
			lastRowIndex = 0;
		}
	}


	private void TryMoveForward()
	{
		Coordinates currentCoordinates = piece.Coordinates;

		if (SquareIsEmpty(currentCoordinates + movementDirection))
		{
			AddToAvailableMoves(currentCoordinates + movementDirection);

			if (!HasMoved)
			{
				if (SquareIsEmpty(currentCoordinates + movementDirection * 2))
				{
					AddToAvailableMoves(currentCoordinates + movementDirection * 2);
				}
			}
		}
	}


	private void TryAttackInDirection(Coordinates attackDirection)
	{
		Coordinates attackForwardInDirection = piece.Coordinates + movementDirection + attackDirection;
		if (CanAttackDiagonally(attackForwardInDirection))
		{
			AddToAvailableMoves(attackForwardInDirection);
		}
		else //try attack en passant
		{
			Coordinates possiblePawnPosition = piece.Coordinates + attackDirection;
			if (CanAttackEnPassant(possiblePawnPosition))
			{
				AddToAvailableMoves(attackForwardInDirection);
			}
		}
	}


	private bool CanAttackDiagonally(Coordinates attackDirection)
	{
		if (SquareIsWithinBoard(attackDirection))
		{
			if (!SquareIsEmpty(attackDirection) && SquareIsOccupiedByEnemy(attackDirection))
			{
				return true;
			}
		}

		return false;
	}


	private void AddMoveToAttackMapIfValid(Coordinates attackDirection)
	{
		Coordinates attackForwardInDirection = piece.Coordinates + movementDirection + attackDirection;
		if (SquareIsWithinBoard(attackForwardInDirection))
		{
			AddToAttackMap(attackForwardInDirection);
		}
	}


	private bool CanAttackEnPassant(Coordinates possiblePawnPosition)
	{
		if (SquareIsWithinBoard(possiblePawnPosition) && !SquareIsEmpty(possiblePawnPosition))
		{
			Piece possibleEnemy = CheckBoard.Instance[possiblePawnPosition];
			if (possibleEnemy.IsEnemy(piece) && (possibleEnemy.type == PieceType.Pawn))
			{
				if (possibleEnemy.GetComponent<PawnBehavior>().MovedTwoSquaresLastTurn)
				{
					return true;
				}
			}
		}

		return false;
	}


	private void RemoveEnemyPawnIfAttackedEnPassant(Coordinates possiblePawnDirection)
	{
		Coordinates enemyPawnPosition = piece.Coordinates + possiblePawnDirection;
		if (CanAttackEnPassant(enemyPawnPosition))
		{
			CheckBoard.Instance[enemyPawnPosition].Remove();
		}
	}
}
