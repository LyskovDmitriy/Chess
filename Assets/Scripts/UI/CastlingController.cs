using System.Collections.Generic;
using UnityEngine;

public class CastlingController : MonoBehaviour 
{

	private KingBehavior king;
	private RookBehavior leftRook;
	private RookBehavior rightRook;


	private bool castlingIsImpossible = false;


	public void AddPiece(Piece pieceToAdd)
	{
		if (pieceToAdd.type == PieceType.King)
		{
			king = pieceToAdd.GetComponent<KingBehavior>();
			king.castlingController = this;
		}
		else if (pieceToAdd.type == PieceType.Rook)
		{
			if (pieceToAdd.Coordinates.x == 0)
			{
				leftRook = pieceToAdd.GetComponent<RookBehavior>();
			}
			else
			{
				rightRook = pieceToAdd.GetComponent<RookBehavior>();
			}
		}
		else
		{
			Debug.Log("Trying to add wrong piece for castling"); 
		}
	}


	public void MoveCorrespondingRookInCastling(Coordinates castlingDirection)
	{
		if (castlingDirection.x > 0)
		{
			rightRook.GetComponent<Piece>().MoveWithoutEndTurn(rightRook.Coordinates + Coordinates.Left * 2);
		}
		else
		{
			leftRook.GetComponent<Piece>().MoveWithoutEndTurn(leftRook.Coordinates + Coordinates.Right * 3);
		}
		castlingIsImpossible = true;
	}


	public void UpdateCastlingAvailability()
	{
		if (castlingIsImpossible)
		{
			return;
		}
			
		if (king.HasMoved 
			|| ((rightRook == null) && rightRook.HasMoved 
				&& ((leftRook == null) && leftRook.HasMoved)))
		{
			castlingIsImpossible = true;
			return;
		}

		if (CastlingIsAvailable(leftRook, Coordinates.Right))
		{
			king.AllowCastling(Coordinates.Left);
		}
		if (CastlingIsAvailable(rightRook, Coordinates.Left))
		{
			king.AllowCastling(Coordinates.Right);
		}
	}


	public bool CastlingIsAvailable(RookBehavior rook, Coordinates directionFromRookToKing)
	{
		if ((rook == null) || rook.HasMoved)
		{
			return false;
		}
			
		for (Coordinates coordinatesToCheck = rook.Coordinates + directionFromRookToKing;
			coordinatesToCheck != king.Coordinates; coordinatesToCheck += directionFromRookToKing)
		{
			if (!CheckBoard.Instance.IsSquareEmpty(coordinatesToCheck))
			{
				return false;
			}
		}
		return true;
	}
}
