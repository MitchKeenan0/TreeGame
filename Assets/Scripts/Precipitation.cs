using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precipitation : MonoBehaviour
{
	public Transform internalBeamTransform;
	public Droplet precipitPrefab;
	public float initialStartDelay = 1f;
	public float precipitFallRate = 5f;
	public float fallSpeed = 10f;

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
			timeLastDrop = Time.timeSinceLevelLoad;
		}
    }

	void ResetDroplet(Droplet dp)
	{
		Vector3 dropletPosition = internalBeamTransform.position;
		//dropletPosition += internalBeamTransform.right * -10f;
		dropletPosition += internalBeamTransform.up * Random.Range(-5f, 5f);
		dp.transform.position = dropletPosition;
		dp.GetComponent<Rigidbody2D>().velocity = internalBeamTransform.transform.right * fallSpeed;
		dp.bActive = true;
		dp.SetDropletEnabled(true);
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
