using UnityEngine;
using System;

public class GameController : MonoBehaviour 
{

	public static GameController Instance { get; private set; }


	public PieceColor ColorToMove { get; private set; }


	public event Action onTurnStart;
	public event Action onTurnEnd;
	public event Action onGameRestart;


	private bool pawnIsBeingPromoted;


	public void PawnPromotionStarted()
	{
		pawnIsBeingPromoted = true;
	}


	public void PawnPromotionCompleted()
	{
		pawnIsBeingPromoted = false;
		EndTurn();
	}


	public void EndTurn()
	{
		if (pawnIsBeingPromoted)
		{
			return;
		}
		onTurnEnd();
		ColorToMove = (PieceColor)( ((int)ColorToMove + 1) % 2 );
		onTurnStart();
	}


	public void RestartGame()
	{
		onGameRestart();
		Start();
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
	}


	private void Start () 
	{
		pawnIsBeingPromoted = false;
		ColorToMove = PieceColor.White;
		onTurnStart();
	}
}
