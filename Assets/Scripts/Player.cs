using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour 
{

	public static event Action<PieceColor?> onPlayerDefeated;


	public PieceColor Color { get { return color; }}
	public AttackMap PlayerAttackMap { get { return playerAttackMap; } }
	public AttackMap EnemyAttackMap { get { return enemyAttackMap; } }


	[SerializeField] private PieceColor color;
	[SerializeField] private AttackMap playerAttackMap;
	[SerializeField] private AttackMap enemyAttackMap;
	[SerializeField] private PiecesCreator piecesCreator;
	private List<Piece> pieces;
	private KingBehavior king;
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
		GameController.Instance.onGameRestart += CreatePieces;
		onPlayerDefeated += ClearPieces;
	}


	private void OnEnable () 
	{
		CreatePieces();
	}


	private void UpdateAvailableMoves()
	{
		if (MovesInCurrentTurn())
		{
			bool kingWasCheckedLastTurn = enemyAttackMap.KingIsChecked;

			if (king.IsInCheck())
			{
				enemyAttackMap.CheckKing(king.Coordinates);
			}
			else if (kingWasCheckedLastTurn)
			{
				enemyAttackMap.UncheckKing();
			}

			bool atLeastOnePieceCanMove = false;
			for (int i = 0; i < pieces.Count; i++)
			{
				pieces[i].CalculateAvailableMoves();

				if (pieces[i].IsInteractive())
				{
					atLeastOnePieceCanMove = true;
				}
			}

			if (!atLeastOnePieceCanMove)
			{
				if (king.IsInCheck())
				{
					onPlayerDefeated(color);
				}
				else
				{
					onPlayerDefeated(null); //a draw
				}
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
		int y = (color == PieceColor.White) ? 1 : 6;

		for (int x = 0; x < CheckBoard.Instance.Size; x++)
		{
			Piece pawn = piecesCreator.CreatePiece(PieceType.Pawn);
			SetPieceInfo(pawn, new Coordinates(x, y));
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
				if (piece.type == PieceType.King)
				{
					king = piece.GetComponent<KingBehavior>();
				}
				castlingController.AddPiece(piece);
			}
		}
	}


	private void SetPieceInfo(Piece piece, Coordinates coord)
	{
		piece.HoldingPlayer = this;
		piece.transform.SetParent(transform);
		piece.Coordinates = coord;
		piece.transform.position = CheckBoard.Instance.BoardToWorldCoordinates(coord);
		piece.Initialize();
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
		else
		{
			return PieceType.King;
		}
	}


	private void ClearPieces(PieceColor? defeatedPlayer)
	{
		while (pieces.Count > 0)
		{
			pieces[0].Remove();
		}
	}


	private void OnDestroy()
	{
		GameController.Instance.onTurnEnd -= UpdateAttackMap;
		GameController.Instance.onTurnStart -= UpdateAvailableMoves;
		GameController.Instance.onGameRestart -= CreatePieces;
		onPlayerDefeated -= ClearPieces;
	}
}
