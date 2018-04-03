using UnityEngine;
using UnityEngine.UI;
using System;

public class PawnPromoter : MonoBehaviour 
{

	public static PawnPromoter Instance { get; private set; }


	[SerializeField] private Image queenButtonImage;
	[SerializeField] private Image rookButtonImage;
	[SerializeField] private Image knightButtonImage;
	[SerializeField] private Image bishopButtonImage;
	[SerializeField] private PieceTypeAndSprites[] piecesSprites;
	[SerializeField] private GameObject promotionScreen;
	private InputController inputController;
	private Player playerForPromotedPiece;
	private Coordinates coordinatesForPromotedPiece;


	public void RequestPromotion(Player holdingPlayer, Coordinates pawnCoordinates)
	{
		GameController.Instance.PawnPromotionStarted();
		inputController.enabled = false;
		playerForPromotedPiece = holdingPlayer;
		coordinatesForPromotedPiece = pawnCoordinates;

		ChangeButtonsSpritesToMatchPlayerColor();
		RotateTowardsPlayerForPromotedPiece();
		promotionScreen.SetActive(true);
	}


	public void Promote(PieceType type)
	{
		promotionScreen.SetActive(false);
		playerForPromotedPiece.AddPiece(type, coordinatesForPromotedPiece);
		GameController.Instance.PawnPromotionCompleted();
		playerForPromotedPiece = null;
		inputController.enabled = true;
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

		inputController = FindObjectOfType<InputController>();
	}

	//player for the piece must be set before calling this function
	private Sprite FindPieceSprite(PieceType type)
	{
		for (int i = 0; i < piecesSprites.Length; i++)
		{
			if (piecesSprites[i].type == type)
			{
				return ((playerForPromotedPiece.Color == PieceColor.White) ? 
					piecesSprites[i].whiteSprite : piecesSprites[i].blackSprite);
			}
		}

		return null;
	}


	private void ChangeButtonsSpritesToMatchPlayerColor()
	{
		queenButtonImage.sprite = FindPieceSprite(PieceType.Queen);
		rookButtonImage.sprite = FindPieceSprite(PieceType.Rook);
		knightButtonImage.sprite = FindPieceSprite(PieceType.Knight);
		bishopButtonImage.sprite = FindPieceSprite(PieceType.Bishop);
	}


	private void RotateTowardsPlayerForPromotedPiece()
	{
		if (playerForPromotedPiece.Color == PieceColor.White)
		{
			promotionScreen.transform.eulerAngles = Vector3.zero;
		}
		else
		{
			promotionScreen.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
		}
	}
}
