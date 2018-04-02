using System.Collections.Generic;
using UnityEngine;

public class BaseBehavior : MonoBehaviour 
{

	public Coordinates Coordinates { get { return piece.Coordinates; } }
	public List<Coordinates> availableMoves { get; protected set; }
	public bool SendOnMoveNotification = false;


	protected Piece piece;
	protected AttackMap friendlyAttackMap; //to add moves to
	protected AttackMap enemyAttackMap; //to check enemy moves


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


	protected void Awake()
	{
		piece = GetComponent<Piece>();
		availableMoves = new List<Coordinates>();
		checkBoard = CheckBoard.Instance;
	}


	protected void Start()
	{
		friendlyAttackMap = piece.HoldingPlayer.PlayerAttackMap;
		enemyAttackMap = piece.HoldingPlayer.EnemyAttackMap;
	}


	protected void AddToAttackMap(Coordinates attackCoordinates)
	{
		friendlyAttackMap.AttackSquare(attackCoordinates, piece, (attackCoordinates - piece.Coordinates).NormalizedDirection);
	}


	protected void AddToAvailableMoves(Coordinates movementCoordinates)
	{
		availableMoves.Add(movementCoordinates);
	}


	protected virtual bool CheckBeforeAddingMoveToAvailable(Coordinates movementCoordinates)
	{
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
}
