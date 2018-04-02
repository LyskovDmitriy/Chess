using System.Collections.Generic;
using UnityEngine;

public class AttackMap : MonoBehaviour 
{

	public SquareAttackInfo this[Coordinates squareCoord]
	{ 
		get { return attackInfoMap[squareCoord.x, squareCoord.y]; }	
	}


	private SquareAttackInfo[,] attackInfoMap;


	public void AttackSquare(Coordinates squareCoord, Piece attackingPiece, Coordinates attackDirection)
	{
		attackInfoMap[squareCoord.x, squareCoord.y].AddAttackInfo(attackingPiece, attackDirection);
	}


	public void Clear()
	{
		for (int x = 0; x < CheckBoard.Instance.Size; x++)
		{
			for (int y = 0; y < CheckBoard.Instance.Size; y++)
			{
				attackInfoMap[x, y].Reset();
			}
		}
	}


	private void Awake () 
	{
		int size = CheckBoard.Instance.Size;
		attackInfoMap = new SquareAttackInfo[size, size];
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				attackInfoMap[x, y] = new SquareAttackInfo();
			}
		}
	}
}
