using System.Collections.Generic;

public class SquareAttackInfo
{

	public bool isUnderAttack;
	public List<Piece> attackingPieces;	


	public SquareAttackInfo()
	{
		isUnderAttack = false;
		attackingPieces = new List<Piece>();
	}


	public void Reset()
	{
		isUnderAttack = false;
		attackingPieces.Clear();
	}
}