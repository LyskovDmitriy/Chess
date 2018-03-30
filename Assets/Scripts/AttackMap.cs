using System.Collections.Generic;
using UnityEngine;

public class AttackMap : MonoBehaviour 
{

	private SquareAttackInfo[,] attackInfoMap;


	private void Awake () 
	{
		attackInfoMap = new SquareAttackInfo[CheckBoard.Instance.Size, CheckBoard.Instance.Size];
		GameController.Instance.onTurnEnd += OnTurnEnd;
	}


	private void OnTurnEnd()
	{
		for (int x = 0; x < CheckBoard.Instance.Size; x++)
		{
			for (int y = 0; y < CheckBoard.Instance.Size; y++)
			{
				attackInfoMap[x, y].Reset();
			}
		}
	}


	private void OnDestroy()
	{
		GameController.Instance.onTurnEnd += OnTurnEnd;
	}
}
