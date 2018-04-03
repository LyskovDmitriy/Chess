using UnityEngine;

public class CastlingController : MonoBehaviour 
{

	private KingBehavior king;
	private RookBehavior leftRook;
	private RookBehavior rightRook;
	private AttackMap enemyAttackMap;
	private bool castlingIsImpossible;


	public void AddPiece(Piece pieceToAdd)
	{
		if (pieceToAdd.type == PieceType.King)
		{
			king = pieceToAdd.GetComponent<KingBehavior>();
			king.castlingController = this;
			enemyAttackMap = pieceToAdd.HoldingPlayer.EnemyAttackMap;
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

		if (CastlingIsAvailable(leftRook, Coordinates.Left))
		{
			king.AllowCastling(Coordinates.Left);
		}
		if (CastlingIsAvailable(rightRook, Coordinates.Right))
		{
			king.AllowCastling(Coordinates.Right);
		}
	}


	public bool CastlingIsAvailable(RookBehavior rook, Coordinates directionFromKingToRook)
	{
		if ((rook == null) || rook.HasMoved)
		{
			return false;
		}
		//check if all squares between the king and the rook are empty
		for (Coordinates coordinatesToCheck = king.Coordinates + directionFromKingToRook;
			coordinatesToCheck != rook.Coordinates; coordinatesToCheck += directionFromKingToRook)
		{
			if (!CheckBoard.Instance.IsSquareEmpty(coordinatesToCheck))
			{
				return false;
			}
		}

		for (Coordinates coordinatesToCheck = king.Coordinates, 
			coordinatesAfterLastCheck = coordinatesToCheck + directionFromKingToRook * 3;
				coordinatesToCheck != coordinatesAfterLastCheck; coordinatesToCheck += directionFromKingToRook)
		{
			if (enemyAttackMap[coordinatesToCheck].isUnderAttack)
			{
				return false;
			}
		}
		return true;
	}


	private void Awake()
	{
		GameController.Instance.onGameRestart += Start;
	}


	private void Start()
	{
		castlingIsImpossible = false;
	}


	private void OnDestroy()
	{
		GameController.Instance.onGameRestart -= Start;
	}
}
