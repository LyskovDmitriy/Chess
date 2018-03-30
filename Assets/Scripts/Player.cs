﻿using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour 
{

	public PieceColor Color { get { return color; }}


	[SerializeField] private PiecesCreator piecesCreator;
	[SerializeField] private PieceColor color;
	private List<Piece> pieces;


	public bool MovesInCurrentTurn()
	{
		return (color == GameController.Instance.ColorToMove);
	}


	public void RemovePiece(Piece pieceToRemove)
	{
		pieces.Remove(pieceToRemove);
	}


	private void Awake()
	{
		pieces = new List<Piece>();
		GameController.Instance.onTurnStart += UpdateAvailableMoves;
	}


	private void OnEnable () 
	{
		CreatePieces();
	}


	private void UpdateAvailableMoves()
	{
		if (MovesInCurrentTurn())
		{
			for (int i = 0; i < pieces.Count; i++)
			{
				pieces[i].CalculateAvailableMoves();
			}
		}
	}


	private void CreatePieces()
	{
		CreatePawns();
		CreateMainPieces();
	}


	private void CreatePawns()
	{
		int pawnY = (color == PieceColor.White) ? 1 : 6;

		for (int x = 0; x < CheckBoard.Instance.Size; x++)
		{
			Piece pawn = piecesCreator.CreatePiece(PieceType.Pawn);
			SetPieceInfo(pawn, new Coordinates(x, pawnY));
		}
	}

	//King, Queen, Rook, Bishop, Rook
	private void CreateMainPieces()
	{
		int y = (color == PieceColor.White) ? 0 : 7;

		for (int x = 0; x < CheckBoard.Instance.Size; x++)
		{
			Piece piece = piecesCreator.CreatePiece(GetPieceType(x));
			SetPieceInfo(piece, new Coordinates(x, y));
		}
	}


	private void SetPieceInfo(Piece piece, Coordinates coord)
	{
		piece.transform.SetParent(transform);
		piece.Coordinates = coord;
		piece.transform.position = CheckBoard.Instance.BoardToWorldCoordinates(coord);
		piece.HoldingPlayer = this;
		pieces.Add(piece);
		CheckBoard.Instance.AddPieceToBoard(piece);
	}


	private PieceType GetPieceType(int x)
	{
		if (x == 0 || x == 7)
		{
			return PieceType.Rook;
		}
		else if (x == 1 || x == 6)
		{
			return PieceType.Knight;
		}
		else if (x == 2 || x == 5)
		{
			return PieceType.Bishop;
		}
		else if (x == 3)
		{
			return PieceType.Queen;
		}
		else if (x == 4)
		{
			return PieceType.King;
		}
		else
		{
			Debug.LogError("Wrong X coordinate: " + x);
			return PieceType.Pawn;
		}
	}


//	private void ClearPieces(PieceColor? defeatedPlayer)
//	{
//
//	}


	private void OnDestroy()
	{
		GameController.Instance.onTurnStart -= UpdateAvailableMoves;
	}
}