using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour 
{

	public PieceColor Color { get { return color; }}
	public AttackMap PlayerAttackMap { get { return playerAttackMap; } }
	public AttackMap EnemyAttackMap { get { return enemyAttackMap; } }


	[SerializeField] private PieceColor color;
	[SerializeField] private AttackMap playerAttackMap;
	[SerializeField] private AttackMap enemyAttackMap;
	[SerializeField] private PiecesCreator piecesCreator;
	private List<Piece> pieces;
	private CastlingController castlingController;


	public void AddPiece(PieceType pieceToAdd, Coordinates coord)
	{
		SetPieceInfo(piecesCreator.CreatePiece(pieceToAdd), coord);
	}


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
		castlingController = GetComponent<CastlingController>();
		GameController.Instance.onTurnEnd += UpdateAttackMap;
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

			castlingController.UpdateCastlingAvailability();
		}
	}


	private void UpdateAttackMap()
	{
		if (MovesInCurrentTurn())
		{
			playerAttackMap.Clear();

			for (int i = 0; i < pieces.Count; i++)
			{
				pieces[i].CalculateMovesForAttackMap();
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

			if (piece.type == PieceType.King || piece.type == PieceType.Rook)
			{
				castlingController.AddPiece(piece);
			}
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
		GameController.Instance.onTurnEnd -= UpdateAttackMap;
		GameController.Instance.onTurnStart -= UpdateAvailableMoves;
	}
}
