using System.Collections.Generic;
using UnityEngine;

public class BaseBehavior : MonoBehaviour 
{

	public Coordinates Coordinates { get { return piece.Coordinates; } }
	public List<Coordinates> availableMoves { get; protected set; }


	public bool SendOnMoveNotification = false;


	protected Piece piece;
	protected AttackMap enemyAttackMap; //to check squares that are attacked by enemy


	private AttackMap friendlyAttackMap; //to add moves to
	private CheckBoard checkBoard;


	public virtual void CalculateAvailableMoves()
	{
		availableMoves.Clear();
	}


	public virtual void CalculateMovesForAttackMap()
	{
	}


	public virtual void ReactToMovement(Coordinates newCoordinates)
	{
	}


	public bool CanMove(Coordinates coord)
	{
		return availableMoves.Contains(coord);
	}


	public bool IsInteractive() 
	{ 
		return availableMoves.Count > 0;
	}


	public virtual void HighlightAvailableMoves()
	{
		for (int i = 0; i < availableMoves.Count; i++)
		{
			if (SquareIsEmpty(availableMoves[i]))
			{
				checkBoard.HighlightSquare(availableMoves[i], SquareHighlightType.CanMove);
			}
			else
			{
				checkBoard.HighlightSquare(availableMoves[i], SquareHighlightType.CanAttack);
			}
		}
	}


	public void UnhighlightAvailableMoves()
	{
		for (int i = 0; i < availableMoves.Count; i++)
		{
			checkBoard.HighlightSquare(availableMoves[i], SquareHighlightType.Unhighlight);
		}
	}


	public virtual void Initialize()
	{
		friendlyAttackMap = piece.HoldingPlayer.PlayerAttackMap;
		enemyAttackMap = piece.HoldingPlayer.EnemyAttackMap;
	}


	protected void AddToAttackMap(Coordinates attackCoordinates)
	{
		Coordinates attackDirection = piece.IsNonLinearAttackType(piece.type) ? 
			Coordinates.Zero : (attackCoordinates - piece.Coordinates).NormalizedDirection;
		friendlyAttackMap.AttackSquare(attackCoordinates, piece, attackDirection);
	}


	protected void AddToAvailableMoves(Coordinates movementCoordinates)
	{
		if (CheckBeforeAddingMoveToAvailable(movementCoordinates))
		{
			availableMoves.Add(movementCoordinates);
		}
	}


	protected virtual bool CheckBeforeAddingMoveToAvailable(Coordinates movementCoordinates)
	{
		//when the king is in check, all moves that don't cover him up are unavailable
		if (enemyAttackMap.KingIsChecked)
		{
			if (!enemyAttackMap.CanCoverEnemyKing(movementCoordinates))
			{
				return false;
			}
		}

		return MoveDoesNotUncoverKing(movementCoordinates);
	}

	//king can't be exposed to check by his player's actions during his turn
	protected bool MoveDoesNotUncoverKing(Coordinates movementCoordinates)
	{
		Coordinates currentCoordinates = piece.Coordinates;
		//if square is not under attack, piece's movement can't uncover the king
		if (!enemyAttackMap[currentCoordinates].isUnderAttack)
		{
			return true;
		}

		SquareAttackInfo attackInfo = enemyAttackMap[currentCoordinates];

		for (int i = 0; i < attackInfo.attackingPieces.Count; i++)
		{
			//pieces with non-linear attack type (pawn, king and knight) always attack particular squares
			//and obstacles don't change their attack range
			if (piece.IsNonLinearAttackType(attackInfo.attackingPieces[i].type))
			{
				continue;
			}
			else
			{
				if (KingIsBehindPiece(attackInfo.attackDirections[i]))
				{
					Coordinates attackDirection = attackInfo.attackDirections[i];
					Coordinates movementDirection = (movementCoordinates - currentCoordinates).NormalizedDirection;
					//if the piece is covering the king, it can move only along attack direction or in the opposite direction
					if ((movementDirection == attackDirection) || (movementDirection == -attackDirection))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}
		//king can't be attacked if the piece moves
		return true;
	}


	protected bool SquareIsEmptyOrOccupiedByEnemy(Coordinates coord)
	{
		if (SquareIsWithinBoard(coord))
		{
			if (SquareIsEmpty(coord) || SquareIsOccupiedByEnemy(coord))
			{
				return true;
			}
		}

		return false;
	}


	protected bool SquareIsWithinBoard(Coordinates coord)
	{
		if (checkBoard.IsSquareWithinBoard(coord))
		{
			return true;
		}

		return false;
	}


	protected bool SquareIsEmpty(Coordinates coord)
	{
		if (checkBoard.IsSquareEmpty(coord))
		{
			return true;
		}

		return false;
	}


	protected bool SquareIsOccupiedByEnemy(Coordinates coord)
	{
		if (checkBoard[coord].IsEnemy(piece))
		{
			return true;
		}

		return false;
	}


	private void Awake()
	{
		piece = GetComponent<Piece>();
		availableMoves = new List<Coordinates>();
		checkBoard = CheckBoard.Instance;
	}


	private bool KingIsBehindPiece(Coordinates direction)
	{
		for (Coordinates currentCoordinates = piece.Coordinates + direction;
			SquareIsWithinBoard(currentCoordinates); currentCoordinates += direction)
		{
			if (SquareIsEmpty(currentCoordinates))
			{
				continue;
			}
			else
			{
				Piece nextPiece = checkBoard[currentCoordinates];
				if (nextPiece.type == PieceType.King && !nextPiece.IsEnemy(piece))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		return false;
	}
}
