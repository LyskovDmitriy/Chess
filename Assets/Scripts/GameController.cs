using UnityEngine;
using System;

public class GameController : MonoBehaviour 
{

	public static GameController Instance { get; private set; }

	public PieceColor ColorToMove { get; private set; }
	public bool IsGameOver { get; private set; }

	public event Action onTurnStart;
	public event Action onTurnEnd;


	public void EndTurn()
	{
		onTurnEnd();
		ColorToMove = (PieceColor)( ((int)ColorToMove + 1) % 2 );
		onTurnStart();
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
		IsGameOver = false;
		ColorToMove = PieceColor.White;
		onTurnStart();
	}


	private void OnGameOver(PieceColor? defeatedPlayer)
	{
		IsGameOver = true;
	}
}
