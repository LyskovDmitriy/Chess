using UnityEngine;

public class PromotionChoiceButton : MonoBehaviour 
{
		
	[SerializeField] private PieceType promotionType;


	public void Promote()
	{
		PiecePromoter.Instance.Promote(promotionType);
	}
}
