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
				Piece selectedPiece = CheckBoard.Instance[boardCoordinates];

				if ((selectedPiece != null) && selectedPiece.HoldingPlayer.MovesInCurrentTurn() && selectedPiece.IsInteractive())
				{
					HighlightPiece(selectedPiece);
				}
			}
			else
			{
				if (CheckBoard.Instance.IsSquareEmpty(boardCoordinates))
				{
					if (highlightedPiece.CanMove(boardCoordinates))
					{
						Debug.Log("Move");
						highlightedPiece.Move(boardCoordinates);
					}
					else
					{
						UnhighlightPiece();
					}
				}
				else
				{
					Piece selectedPiece = CheckBoard.Instance[boardCoordinates];

					if (selectedPiece.HoldingPlayer.MovesInCurrentTurn() && (selectedPiece != highlightedPiece) && selectedPiece.IsInteractive())
					{
						UnhighlightPiece();
						HighlightPiece(selectedPiece);
					}
					else if ((highlightedPiece != null) && (highlightedPiece.IsEnemy(selectedPiece)) 
						&& highlightedPiece.CanMove(boardCoordinates)) 
					{
						Debug.Log("Attack");
						highlightedPiece.MoveAndAttack(boardCoordinates);
					}
				}
			}
		}
		else
		{
			UnhighlightPiece();
		}
	}


	private void HighlightPiece(Piece piece)
	{
		highlightedPiece = piece;
		highlightedPiece.Highlight();
		//highlight possible moves
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
	}
}
