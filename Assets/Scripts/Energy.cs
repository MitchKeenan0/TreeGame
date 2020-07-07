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
	}

	public void LeafEnergy(int value, Vector3 position)
	{
		currentEnergy += value;
		energyUI.PopEnergy(currentEnergy, position);
		upgradeUI.UpdateTotalEnergy(value);
	}
}
