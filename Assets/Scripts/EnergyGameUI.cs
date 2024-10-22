﻿using System.Collections;
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

	public void PopEnergy(int value, int total, Vector3 position)
	{
		UpdateScoreText(total);
		PopUI pop = GetListedPop();
		if (pop != null)
		{
			pop.gameObject.SetActive(true);
			pop.PopIt(value, position);
		}
	}

	public void UpdateScoreText(int value)
	{
		energyValueText.text = value.ToString();
	}

	PopUI GetListedPop()
	{
		PopUI pop = null;
		if (scorePopList != null)
		{
			foreach (PopUI pp in scorePopList)
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
		}
		return pop;
	}
}
