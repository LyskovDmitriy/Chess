using System.Collections.Generic;
using UnityEngine;

public class AttackMap : MonoBehaviour 
{
	
	public SquareAttackInfo this[Coordinates squareCoord]
	{ 
		get { return attackInfoMap[squareCoord.x, squareCoord.y]; }	
	}


	public bool KingIsChecked { get; private set; }


	private SquareAttackInfo[,] attackInfoMap;
	private List<Coordinates> coordinatesToCoverEnemyKing;
	private bool kingCanBeCoveredUp;


	public void CheckKing(Coordinates kingCoordinates)
	{
		KingIsChecked = true;

		SquareAttackInfo attackInfo = attackInfoMap[kingCoordinates.x, kingCoordinates.y];
		kingCanBeCoveredUp = (attackInfo.attackingPieces.Count == 1);

		if (kingCanBeCoveredUp)
		{
			CalculateCoordinatesToCoverEnemyKing(kingCoordinates);
		}
	}


	public void UncheckKing()
	{
		KingIsChecked = false;
		kingCanBeCoveredUp = true;
	}


	public bool CanCoverEnemyKing(Coordinates possibleMove)
	{
		if (kingCanBeCoveredUp)
		{
			return coordinatesToCoverEnemyKing.Contains(possibleMove);
		}

		return false;
	}


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

		coordinatesToCoverEnemyKing.Clear();
	}


	private void Awake() 
	{
		CreateAttackMap();
		coordinatesToCoverEnemyKing = new List<Coordinates>();
		GameController.Instance.onGameRestart += Start;
	}


	private void Start()
	{
		kingCanBeCoveredUp = true;
		KingIsChecked = false;
		Clear();
	}


	private void CreateAttackMap()
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


	private void CalculateCoordinatesToCoverEnemyKing(Coordinates kingCoordinates)
	{
		SquareAttackInfo attackInfo = attackInfoMap[kingCoordinates.x, kingCoordinates.y];
		Coordinates attackDirection = attackInfo.attackDirections[0];
		Coordinates attackerCoordinates = attackInfo.attackingPieces[0].Coordinates;

		if (attackDirection != Coordinates.Zero)
		{
			for (Coordinates currentCoordinates = kingCoordinates - attackDirection;
				currentCoordinates != attackerCoordinates; currentCoordinates -= attackDirection)
			{
				coordinatesToCoverEnemyKing.Add(currentCoordinates);
			}
		}
		coordinatesToCoverEnemyKing.Add(attackerCoordinates);
	}


	private void OnDestroy()
	{
		GameController.Instance.onGameRestart -= Start;
	}
}
