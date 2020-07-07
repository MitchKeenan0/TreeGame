using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private int totalEnergy = 0;
	private int limbEnergy = 0;
	private int leafEnergy = 0;
	private int limbs = 0;
	private int leafs = 0;

    void Start()
    {
		limbEnergy = limbPrice;
		leafEnergy = leafPrice;
    }

	void AddBranchToken()
	{
		GameObject branchToken = Instantiate(limbUnitPrefab, limbPanel);
		branchToken.GetComponent<TouchMove>().InitMover(transform);
		limbEnergy += limbPrice;
		limbs++;
	}

	void AddLeafToken()
	{
		GameObject leafToken = Instantiate(leafUnitPrefab, leafPanel);
		leafToken.GetComponent<TouchMove>().InitMover(transform);
		leafEnergy += leafPrice;
		leafs++;
	}

	public void UpdateTotalEnergy(int value)
	{
		totalEnergy += value;
		if ((totalEnergy >= limbEnergy) && (limbs < limbCapacity))
			AddBranchToken();
		if ((totalEnergy >= leafEnergy) && (leafs < leafCapacity))
			AddLeafToken();
	}

	public void Branch()
	{

	}
}
