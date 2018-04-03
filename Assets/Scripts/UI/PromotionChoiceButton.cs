using UnityEngine;

public class PromotionChoiceButton : MonoBehaviour 
{
		
	[SerializeField] private PieceType promotionType;


	public void Promote()
	{
		PawnPromoter.Instance.Promote(promotionType);
	}
}
