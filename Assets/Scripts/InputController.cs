using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour 
{

	private Camera mainCamera;
	private Piece highlightedPiece;


	private void Awake()
	{
		mainCamera = Camera.main;
		GameController.Instance.onTurnEnd += OnTurnEnd;
		GameController.Instance.onGameRestart += OnGameRestart;
		Player.onPlayerDefeated += OnGameOver;
		#if UNITY_ANDROID
		Input.multiTouchEnabled = false;
		Input.simulateMouseWithTouches = false;
		#endif
	}


	private void Update () 
	{
		#if UNITY_STANDALONE || UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			HandleInput(Input.mousePosition);
		}
		#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			HandleInput(Input.GetTouch(0).position);
		}
		#endif
	}


	private void HandleInput(Vector3 inputPosition)
	{
		Coordinates? coord = GetCoordinatesAtScreenInput(inputPosition);

		if (coord.HasValue)
		{
			Coordinates boardCoordinates = coord.Value;

			if (highlightedPiece == null)
			{
				Case_NoHighlightedPiece(boardCoordinates);
			}
			else
			{
				Case_ThereIsHighlightedPiece(boardCoordinates);
			}
		}
		else
		{
			UnhighlightPiece();
		}
	}


	private void Case_NoHighlightedPiece(Coordinates inputCoordinates)
	{
		Piece selectedPiece = CheckBoard.Instance[inputCoordinates];
		if ((selectedPiece != null) && selectedPiece.HoldingPlayer.MovesInCurrentTurn() && selectedPiece.IsInteractive())
		{
			HighlightPiece(selectedPiece);
		}
	}


	private void Case_ThereIsHighlightedPiece(Coordinates inputCoordinates)
	{
		if (CheckBoard.Instance.IsSquareEmpty(inputCoordinates))
		{
			Action_SelectEmptySquare(inputCoordinates);
		}
		else
		{
			Action_SelectOccupiedSquare(inputCoordinates);
		}
	}


	private void Action_SelectEmptySquare(Coordinates inputCoordinates)
	{
		if (highlightedPiece.CanMove(inputCoordinates))
		{
			highlightedPiece.Move(inputCoordinates);
		}
		else
		{
			UnhighlightPiece();
		}
	}


	private void Action_SelectOccupiedSquare(Coordinates inputCoordinates)
	{
		Piece selectedPiece = CheckBoard.Instance[inputCoordinates];

		if (selectedPiece.HoldingPlayer.MovesInCurrentTurn() && (selectedPiece != highlightedPiece) && selectedPiece.IsInteractive())
		{
			UnhighlightPiece();
			HighlightPiece(selectedPiece);
		}
		else if ((highlightedPiece != null) && (highlightedPiece.IsEnemy(selectedPiece)) 
			&& highlightedPiece.CanMove(inputCoordinates)) 
		{
			highlightedPiece.MoveAndAttack(inputCoordinates);
		}
	}


	private void HighlightPiece(Piece piece)
	{
		highlightedPiece = piece;
		highlightedPiece.Highlight();
	}


	private void UnhighlightPiece()
	{
		if (highlightedPiece != null)
		{
			highlightedPiece.Unhighlight();
			highlightedPiece = null;
		}
	}


	private void OnTurnEnd()
	{
		highlightedPiece = null;
	}


	private void OnGameOver(PieceColor? defeatedPlayer)
	{
		enabled = false;
	}


	private void OnGameRestart()
	{
		enabled = true;
	}


	private Coordinates? GetCoordinatesAtScreenInput(Vector3 screenInputPosition)
	{
		Vector3 screenPosition = screenInputPosition;
		screenPosition.z = mainCamera.transform.position.y;
		Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
		return CheckBoard.Instance.WorldToBoardCoordinates(worldPosition);
	}


	private void OnDestroy()
	{
		GameController.Instance.onTurnEnd -= OnTurnEnd;
		GameController.Instance.onGameRestart -= OnGameRestart;
		Player.onPlayerDefeated -= OnGameOver;
	}
}
