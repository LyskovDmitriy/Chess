using UnityEngine;
using System.Collections.Generic;

public class CheckBoard : MonoBehaviour 
{

	public static CheckBoard Instance { get; private set; }

	public Piece this [int x, int y] { get { return piecesBoard[x, y]; } }
	public Piece this [Coordinates coord] { get { return piecesBoard[coord.x, coord.y]; } }
	public int Size { get { return size; } }


	public Material beigeMaterial;
	public Material brownMaterial;
	public GameObject boardSquarePrefab;


	private const int size = 8;

	private Vector3 startingPosition = new Vector3(-3.5f, -3.5f);
	private Piece[,] piecesBoard;
	private BoardSquare[,] squaresObjects;
	private List<BoardSquare> highlightedSquares;


	public Coordinates? WorldToBoardCoordinates(Vector3 position)
	{
		int x = Mathf.RoundToInt(position.x - startingPosition.x);
		int y = Mathf.RoundToInt(position.y - startingPosition.y);

		if (IsSquareWithinBoard(x, y))
		{
			return new Coordinates(x, y);
		}
		else
		{
			return null;
		}
	}


	public Vector3 BoardToWorldCoordinates(Coordinates coord)
	{
		return BoardToWorldCoordinates(coord.x, coord.y);
	}


	public Vector3 BoardToWorldCoordinates(int x, int y)
	{
		return startingPosition + new Vector3(x, y);
	}


	public bool IsSquareEmpty(int x, int y)
	{
		return (piecesBoard[x, y] == null);
	}


	public bool IsSquareEmpty(Coordinates coord)
	{
		return (piecesBoard[coord.x, coord.y] == null);
	}


	public bool IsSquareWithinBoard(Coordinates coord)
	{
		return IsSquareWithinBoard(coord.x, coord.y);
	}


	public bool IsSquareWithinBoard(int x, int y)
	{
		if ((0 <= x) && (x < size) 
			&& (0 <= y)  && (y < size))
		{
			return true;
		}

		return false;
	}


	public void AddPieceToBoard(Piece pieceToAdd)
	{
		Coordinates pieceCoordinates = pieceToAdd.Coordinates;
		piecesBoard[pieceCoordinates.x, pieceCoordinates.y] = pieceToAdd;
	}


	public void Move(Coordinates startingPosition, Coordinates endPosition)
	{
		piecesBoard[endPosition.x, endPosition.y] = piecesBoard[startingPosition.x, startingPosition.y];
		piecesBoard[startingPosition.x, startingPosition.y] = null;
	}


	public void RemoveFromBoard(Coordinates coord)
	{
		piecesBoard[coord.x, coord.y] = null;
	}	


	public void HighlightSquare(Coordinates coord, SquareHighlightType highlightType)
	{
		if (highlightType != SquareHighlightType.Unhighlight)
		{
			if (highlightedSquares.Contains(squaresObjects[coord.x, coord.y]))
			{
				Debug.Log("Trying to highlight already highlighted square " + coord);
			}
			highlightedSquares.Add(squaresObjects[coord.x, coord.y]);
		}
		else
		{
			if (highlightedSquares.Contains(squaresObjects[coord.x, coord.y]))
			{
				highlightedSquares.Remove(squaresObjects[coord.x, coord.y]);
			}
			else
			{
				Debug.Log("Trying to unhighlight square that wasn't highlighted " + coord);
			}
		}

		squaresObjects[coord.x, coord.y].Highlight(highlightType);
	}


	public void CreateBoard () 
	{
		squaresObjects = new BoardSquare[size, size];

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				Vector3 position = new Vector3(startingPosition.x + x, startingPosition.y + y);
				GameObject square = Instantiate(boardSquarePrefab, position, Quaternion.identity);
				square.transform.SetParent(transform);

				BoardSquare squareInfo = square.GetComponent<BoardSquare>();
				Material standartMaterial = (((x + y) % 2) == 0) ? brownMaterial : beigeMaterial;
				squareInfo.SetAndApplyStandartMaterial(standartMaterial);

				squaresObjects[x, y] = squareInfo;
			}
		}

		piecesBoard = new Piece[size, size];
	}


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		CreateBoard();
		GameController.Instance.onTurnEnd += UnhighlightAllSquares;
		Player.onPlayerDefeated += OnGameOver;
		highlightedSquares = new List<BoardSquare>();
	}


	private void UnhighlightAllSquares()
	{
		for (int i = highlightedSquares.Count - 1; i >= 0; i--)
		{
			highlightedSquares[i].Highlight(SquareHighlightType.Unhighlight);
			highlightedSquares.RemoveAt(i);
		}
	}


	private void OnGameOver(PieceColor? defeatedPlayer)
	{
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				piecesBoard[x, y] = null;
			}
		}
	}


	private void OnDestroy()
	{
		GameController.Instance.onTurnEnd -= UnhighlightAllSquares;
		Player.onPlayerDefeated -= OnGameOver;
	}
}
