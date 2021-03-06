﻿using UnityEngine;

public class BoardSquare : MonoBehaviour 
{

	[SerializeField] private Material canMoveSquareMaterial;
	[SerializeField] private Material canAttackSquareMaterial;
	[SerializeField] private Material pieceSelectedMaterial;
	[SerializeField] private Material transparentMaterial;
	[SerializeField] private SpriteRenderer overlappingSquareRenderer;
	private Material standartMaterial;
	private new SpriteRenderer renderer;


	public void SetAndApplyStandartMaterial(Material material)
	{
		standartMaterial = material;
		renderer.material = standartMaterial;
	}


	public void Highlight(SquareHighlightType type)
	{
		switch (type)
		{
			case SquareHighlightType.CanAttack:
				overlappingSquareRenderer.material = canAttackSquareMaterial;
				break;
			case SquareHighlightType.CanMove:
				overlappingSquareRenderer.material = canMoveSquareMaterial;
				break;
			case SquareHighlightType.SelectedPiece:
				overlappingSquareRenderer.material = pieceSelectedMaterial;
				break;
			case SquareHighlightType.Unhighlight:
				overlappingSquareRenderer.material = transparentMaterial;
				break;
		}
	}


	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
	}
}
