﻿using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Piece : MonoBehaviour
{

	public PieceType type;
	public Coordinates Coordinates { get { return squareCoordinates; } set { squareCoordinates = value; } }
	public Player HoldingPlayer { get; set; }


	[SerializeField] private List<PieceType> nonLinearAttackTypes;
	private Coordinates squareCoordinates;
	private BaseBehavior behavior;


	public void Move(Coordinates newCoordinates)
	{
		MoveWithoutEndTurn(newCoordinates);
		GameController.Instance.EndTurn();
	}

	//is used for castling
	public void MoveWithoutEndTurn(Coordinates newCoordinates)
	{
		CheckBoard.Instance.Move(squareCoordinates, newCoordinates);
		if (behavior.SendOnMoveNotification)
		{
			behavior.ReactToMovement(newCoordinates);
		}
		squareCoordinates = newCoordinates;
		transform.position = CheckBoard.Instance.BoardToWorldCoordinates(squareCoordinates);
	}


	public void MoveAndAttack(Coordinates newCoordinates)
	{
		CheckBoard.Instance[newCoordinates].Remove();
		Move(newCoordinates);
	}


	public void Remove()
	{
		HoldingPlayer.RemovePiece(this);
		CheckBoard.Instance.RemoveFromBoard(squareCoordinates);
		Destroy(gameObject);
	}


	public bool IsEnemy(Piece possibleEnemy)
	{
		return (HoldingPlayer.Color != possibleEnemy.HoldingPlayer.Color);
	}


	public bool IsNonLinearAttackType(PieceType type)
	{
		return nonLinearAttackTypes.Contains(type);
	}


	public bool CanMove (Coordinates coord)
	{
		return behavior.CanMove(coord);
	}


	public void CalculateAvailableMoves()
	{
		behavior.CalculateAvailableMoves();
	}


	public void CalculateMovesForAttackMap()
	{
		behavior.CalculateMovesForAttackMap();
	}


	public bool IsInteractive()
	{
		return behavior.IsInteractive();
	}


	public void Highlight()
	{
		CheckBoard.Instance.HighlightSquare(squareCoordinates, SquareHighlightType.SelectedPiece);
		behavior.HighlightAvailableMoves();	
	}


	public void Unhighlight()
	{
		CheckBoard.Instance.HighlightSquare(squareCoordinates, SquareHighlightType.Unhighlight);
		behavior.UnhighlightAvailableMoves();
	}


	public void Initialize()
	{
		behavior.Initialize();
	}


	private void Awake()
	{
		behavior = GetComponent<BaseBehavior>();
	}
}
