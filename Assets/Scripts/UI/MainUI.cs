using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour 
{

	public Text[] activePlayerTexts;


	private void Awake()
	{
		GameController.Instance.onTurnStart += ChangeActivePlayer;
	}


	private void ChangeActivePlayer()
	{
		for (int i = 0; i < activePlayerTexts.Length; i++)
		{
			activePlayerTexts[i].text = GameController.Instance.ColorToMove.ToString();
		}
	}


	private void OnDestroy()
	{
		GameController.Instance.onTurnStart -= ChangeActivePlayer;
	}
}
