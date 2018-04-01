using System.Collections.Generic;
using UnityEngine;

public class BaseBehavior : MonoBehaviour 
{

	public Coordinates Coordinates { get { return piece.Coordinates; } }
	public List<Coordinates> availableMoves { get; protected set; }
	public bool SendOnMoveNotification = false;


	protected Piece piece;


	private CheckBoard checkBoard;


	public virtual void CalculateAvailableMoves()
	{
		availableMoves.Clear();
	}


	public virtual void ReactToMovement(Coordinates newCoordinates)
	{ }


	public bool CanMove(Coordinates coord)
	{
		return availableMoves.Contains(coord);
	}


	public bool IsInteractive() { return availableMoves.Count > 0; }


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
