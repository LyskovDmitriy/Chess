using System.Collections.Generic;

public class SquareAttackInfo
{

	public bool isUnderAttack;
	public List<Piece> attackingPieces;	//Can be replaced with types
	public List<Coordinates> attackDirections;


	private static int initialCapacity = 5;


	public void AddAttackInfo(Piece attackingPiece, Coordinates direction)
	{
		isUnderAttack = true;
		attackingPieces.Add(attackingPiece);
		attackDirections.Add(direction);
	}


	public SquareAttackInfo()
	{
		isUnderAttack = false;
		attackingPieces = new List<Piece>(initialCapacity);
		attackDirections = new List<Coordinates>(initialCapacity);
	}


	public void Reset()
	{
		isUnderAttack = false;
		attackingPieces.Clear();
		attackDirections.Clear();
	}
}