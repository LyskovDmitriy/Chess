using System.Collections.Generic;
using UnityEngine;

public class BaseBehavior : MonoBehaviour 
{

	public List<Coordinates> availableMoves { get; protected set; }
	public bool SendOnMoveNotification = false;


	protected Piece piece;


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
			if (CheckBoard.Instance.IsSquareEmpty(availableMoves[i]))
			{
				CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.CanMove);
			}
			else
			{
				CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.CanAttack);
			}
		}
	}


	public void UnhighlightAvailableMoves()
	{
		for (int i = 0; i < availableMoves.Count; i++)
		{
			CheckBoard.Instance.HighlightSquare(availableMoves[i], SquareHighlightType.Unhighlight);
		}
	}


	protected void Awake()
	{
		piece = GetComponent<Piece>();
		availableMoves = new List<Coordinates>();
	}


	protected void OnDestroy()
	{
	}
}
