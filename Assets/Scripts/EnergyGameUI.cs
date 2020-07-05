using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGameUI : MonoBehaviour
{
	public Text energyValueText;
	public PopUI scorePopPrefab;

	private List<PopUI> scorePopList;

    void Start()
    {
		scorePopList = new List<PopUI>();
		for (int i = 0; i < 5; i++)
		{
			PopUI pop = Instantiate(scorePopPrefab, transform);
			scorePopList.Add(pop);
			pop.gameObject.SetActive(false);
		}
	}

	public void PopEnergy(int value, Vector3 position)
	{
		energyValueText.text = value.ToString();
		PopUI pop = GetListedPop();
		pop.gameObject.SetActive(true);
		pop.PopIt(1, position);
	}

	PopUI GetListedPop()
	{
		PopUI pop = null;
		foreach(PopUI pp in scorePopList)
		{
			if (!pp.bActive)
			{
				pop = pp;
				break;
			}
		}
		if (pop == null)
		{
			PopUI popper = Instantiate(scorePopPrefab, transform);
			pop = popper;
		}
		return pop;
	}
}
