using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
	public Transform limbPanel;
	public Transform leafPanel;
	public GameObject limbUnitPrefab;
	public GameObject leafUnitPrefab;
	public int limbPrice = 20;
	public int leafPrice = 15;
	public int limbCapacity = 5;
	public int leafCapacity = 5;
	public int[] gridScaleCounts;
	public int lengthUpdatePrice = 1000;
	public int widthUpgradePrice = 100;

	private Energy energy;
	private GridLayoutStretch grid;
	private int gridCellSize = 100;
	private int scaleIndex = 0;
	private bool bScaleMax = false;
	private List<GameObject> branchTokenList;
	private int totalEnergy = 0;
	private int limbEnergy = 0;
	private int leafEnergy = 0;

    void Awake()
    {
		branchTokenList = new List<GameObject>();
		grid = GetComponentInChildren<GridLayoutStretch>();
		energy = FindObjectOfType<Energy>();
    }

	void AddBranchToken()
	{
		GameObject branchToken = Instantiate(limbUnitPrefab, limbPanel);
		branchTokenList.Add(branchToken);
		branchToken.GetComponent<TouchMove>().InitMover(transform);
	}

	void AddLeafToken()
	{
		GameObject leafToken = Instantiate(leafUnitPrefab, leafPanel);
		leafToken.GetComponent<TouchMove>().InitMover(transform);
		leafEnergy += leafPrice;
	}

	public void UpdateTotalEnergy(int value)
	{
		totalEnergy += value;
		if (totalEnergy >= (limbEnergy + limbPrice))
		{
			AddBranchToken();
			limbEnergy += limbPrice;
		}

		// scale upgrades
		if ((branchTokenList.Count > gridScaleCounts[scaleIndex]) && !bScaleMax)
		{
			gridCellSize = Mathf.RoundToInt(gridCellSize * 0.62f);
			float width = limbPanel.gameObject.GetComponent<RectTransform>().rect.width;
			Vector2 newSize = new Vector2(gridCellSize, gridCellSize);
			grid.cellSize = newSize;

			if (gridScaleCounts.Length > (scaleIndex + 1))
				scaleIndex++;
			else
				bScaleMax = true;
		}
	}

	public void BranchUpgrade()
	{
		if (energy.GetEnergy() >= lengthUpdatePrice)
		{
			TreeLimb[] limbs = FindObjectsOfType<TreeLimb>();
			foreach (TreeLimb tl in limbs)
				tl.SetGrowTargetLength(1.6f);
			energy.SpendEnergy(lengthUpdatePrice);
		}
	}

	public void WidthUpgrade()
	{
		if (energy.GetEnergy() >= widthUpgradePrice)
		{
			TreeLimb[] limbs = FindObjectsOfType<TreeLimb>();
			foreach (TreeLimb tl in limbs)
				tl.SetGrowTargetWidth(1.6f);
			energy.SpendEnergy(widthUpgradePrice);
		}
	}
}
