using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
	private EnergyGameUI energyUI;
	private UpgradeUI upgradeUI;
	private int currentEnergy = 0;

	void Start()
	{
		energyUI = FindObjectOfType<EnergyGameUI>();
		upgradeUI = FindObjectOfType<UpgradeUI>();
		LeafEnergy(1, new Vector3(Screen.width * 2f, Screen.height * 2f, 0f));
	}

	public int GetEnergy() { return currentEnergy; }

	public void LeafEnergy(int value, Vector3 position)
	{
		currentEnergy += value;
		energyUI.PopEnergy(value, currentEnergy, position);
		upgradeUI.UpdateTotalEnergy(value);
	}

	public void SpendEnergy(int value)
	{
		currentEnergy -= value;
		energyUI.UpdateScoreText(currentEnergy);
	}
}
