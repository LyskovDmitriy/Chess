using UnityEngine;

[CreateAssetMenu]
public class PiecesCreator : ScriptableObject 
{

	[SerializeField] private PiecePrefabAndSprite[] pieces;


	public Piece CreatePiece(PieceType type)
	{
		PiecePrefabAndSprite pieceInfo = FindPieceInfo(type);
		Piece piece = Instantiate(pieceInfo.prefab);
		piece.GetComponent<SpriteRenderer>().sprite = pieceInfo.sprite;
		return piece;
	}


	private PiecePrefabAndSprite FindPieceInfo(PieceType type)
	{
		for (int i = 0; i < pieces.Length; i++)
		{
			if (pieces[i].prefab.type == type)
			{
				return pieces[i];
			}
		}

		return null;
	}
}
