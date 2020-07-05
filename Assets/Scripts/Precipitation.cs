﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precipitation : MonoBehaviour
{
	public Transform internalBeamTransform;
	public Droplet precipitPrefab;
	public float initialStartDelay = 1f;
	public float precipitFallRate = 30f;
	public float fallSpeed = 10f;
	public Vector2 fallDirection = Vector2.zero;

	private List<Droplet> precipList;
	private float timeLastDrop = 0f;

    void Awake()
    {
		precipList = new List<Droplet>();
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > (timeLastDrop + (1f / precipitFallRate)))
		{
			Droplet nextDrop = GetListedDroplet();
			ResetDroplet(nextDrop);
		}
    }

	void ResetDroplet(Droplet dp)
	{
		Vector3 dropletPosition = Vector3.zero;
		dropletPosition += internalBeamTransform.right * -10f;
		dropletPosition += internalBeamTransform.up * Random.Range(-5f, 5f);
		dp.transform.position = dropletPosition;
		dp.GetComponent<Rigidbody2D>().velocity = internalBeamTransform.transform.right * precipitFallRate;
		dp.bActive = true;
		dp.SetVisualEnabled(true);
	}

	Droplet GetListedDroplet()
	{
		Droplet returned = null;
		if (precipList != null && precipList.Count > 0)
		{
			foreach(Droplet dp in precipList)
			{
				if (!dp.bActive)
				{
					returned = dp;
					break;
				}
			}
		}

		if (returned == null)
			returned = SpawnDroplet();
		return returned;
	}

	Droplet SpawnDroplet()
	{
		Droplet drop = Instantiate(precipitPrefab, transform);
		precipList.Add(drop);
		return drop;
	}
}