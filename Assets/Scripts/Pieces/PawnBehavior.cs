using System.Collections.Generic;
using UnityEngine;

public class PawnBehavior : BaseBehavior 
{

	public bool MovedTwoSquaresLastTurn { get; private set; }


	private Coordinates movementDirection;
	private int startingY;
	private bool calculatedMovementData = false;


	public override void CalculateAvailableMoves()
	{
		base.CalculateAvailableMoves();

		if (!calculatedMovementData)
		{
			CalculateMovementData();
		}

		if (MovedTwoSquaresLastTurn)
		{
			MovedTwoSquaresLastTurn = false;
		}
			
		Coordinates currentCoordinates = piece.Coordinates;
		if (CheckBoard.Instance.IsSquareEmpty(currentCoordinates + movementDirection))
		{
			availableMoves.Add(currentCoordinates + movementDirection);
		}

		if (!HasMoved())
		{
			if (CheckBoard.Instance.IsSquareEmpty(currentCoordinates + movementDirection * 2))
			{
				availableMoves.Add(currentCoordinates + movementDirection * 2);
			}
		} 

		bool canAttackLeft = false;
		Coordinates attackLeftForward = currentCoordinates + movementDirection + Coordinates.Left;
		if (CanAttackDiagonally(attackLeftForward))
		{
			availableMoves.Add(attackLeftForward);
			canAttackLeft = true;
		}

		bool canAttackRight = false;
		Coordinates attackRightForward = currentCoordinates + movementDirection + Coordinates.Right;
		if (CanAttackDiagonally(attackRightForward))
		{
			availableMoves.Add(attackRightForward);
			canAttackRight = true;
		}

		//en passant
		if (!canAttackLeft)
		{
			Coordinates attackLeft = currentCoordinates + Coordinates.Left;

			if (CanAttackEnPassant(attackLeft))
			{
				availableMoves.Add(attackLeftForward);
			}
		}

		if (!canAttackRight)
		{
			Coordinates attackRight = currentCoordinates + Coordinates.Right;

			if (CanAttackEnPassant(attackRight))
			{
				availableMoves.Add(attackRightForward);
			}
		}
	} 


	public override void ReactToMovement(Coordinates newCoordinates)
	{
		//if the piece moves diagonally, it attacks en passant
		Coordinates currentCoordinates = piece.Coordinates;
		if ((currentCoordinates.x != newCoordinates.x) && (currentCoordinates.y != newCoordinates.y))
		{
			if (CheckBoard.Instance.IsSquareEmpty(currentCoordinates + movementDirection + Coordinates.Left))
			{
				CheckBoard.Instance[currentCoordinates + Coordinates.Left].Remove();
			}
				
			if (CheckBoard.Instance.IsSquareEmpty(currentCoordinates + movementDirection + Coordinates.Right))
			{
				CheckBoard.Instance[currentCoordinates + Coordinates.Right].Remove();
			}
		}

		int movementDistance = Mathf.Abs((currentCoordinates.y - newCoordinates.y));
		if (movementDistance == 2)
		{
			MovedTwoSquaresLastTurn = true;
		}

		//check if the pawn reached last row to transform into another piece
	}


	public override void HighlightAvailableMoves()
	{
		for (int i = 0; i < availableMoves.Count; i++)
		{
			if (CheckBoard.Instance.IsSquareEmpty(availableMoves[i]))
			{
				if ((piece.Coordinates.x != availableMoves[i].x) && (piece.Coordinates.y != availableMoves[i].y))
				{
					CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.CanAttack);
				}
				else
				{
					CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.CanMove);
				}
			}
			else
			{
				CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.CanAttack);
			}
		}
	}


	private bool CanAttackDiagonally(Coordinates attackDirection)
	{
		if (CheckBoard.Instance.IsSquareWithinBoard(attackDirection))
		{
			if (!CheckBoard.Instance.IsSquareEmpty(attackDirection))
			{
				return true;
			}
		}

		return false;
	}


	private bool CanAttackEnPassant(Coordinates possiblePawnPosition)
	{
		if (CheckBoard.Instance.IsSquareWithinBoard(possiblePawnPosition))
		{
			Piece possibleEnemy = CheckBoard.Instance[possiblePawnPosition];

			if ((possibleEnemy != null) && possibleEnemy.IsEnemy(piece) && possibleEnemy.type == PieceType.Pawn)
			{
				if (possibleEnemy.GetComponent<PawnBehavior>().MovedTwoSquaresLastTurn)
				{
					return true;
				}
			}
		}

		return false;
	}


	private void CalculateMovementData()
	{
		movementDirection = (piece.HoldingPlayer.Color == PieceColor.White) ? Coordinates.Up : Coordinates.Down;
		startingY = (piece.HoldingPlayer.Color == PieceColor.White) ? 1 : 6;
		calculatedMovementData = true;
	}


	private bool HasMoved()
	{
		return piece.Coordinates.y != startingY;
	}
}
