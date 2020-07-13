using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
	public float sunlightEnergyRate = 0.333f;
	public float circleCastScale = 0.2f;

	private ParticleSystem sunParticles;
	private Energy energy;
	private bool bSunlit = false;
	private float timeLastEnergy = 0f;

    void Start()
    {
		sunParticles = GetComponentInChildren<ParticleSystem>();
		SetParticles(false);
		energy = FindObjectOfType<Energy>();
    }
	
    void Update()
    {
		bSunlit = false;
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, circleCastScale, Vector3.forward);
		if (hits.Length > 0)
		{
			for(int i = 0; i < hits.Length; i++)
			{
				RaycastHit2D hit = hits[i];
				if (hit.collider.gameObject.GetComponent<Sunbeam>())
				{
					bSunlit = true;
					break;
				}
			}
		}

		SetParticles(bSunlit);

		if (bSunlit)
			UpdateSunlightEnergy();
    }

	void UpdateSunlightEnergy()
	{
		if (Time.timeSinceLevelLoad >= (timeLastEnergy + (1f / sunlightEnergyRate)))
		{
			Vector3 uiPosition = new Vector3(Random.Range(-circleCastScale, circleCastScale), Random.Range(-circleCastScale, circleCastScale), 0f);
			energy.LeafEnergy(10, transform.position + uiPosition);
			timeLastEnergy = Time.timeSinceLevelLoad;
		}
	}

	void SetParticles(bool bEnabled)
	{
		var em = sunParticles.emission;
		em.enabled = bEnabled;
	}
}
