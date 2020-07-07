using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
	public float lifeTime = 5f;
	public bool bActive = false;

	private SpriteRenderer sprite;
	private TrailRenderer trail;
	private IEnumerator lifetimeCoroutine;
	private IEnumerator resetCoroutine;
	private Energy energy;
	private bool bResetting = false;

    void Awake()
    {
		energy = FindObjectOfType<Energy>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		trail = GetComponentInChildren<TrailRenderer>();
		lifetimeCoroutine = Lifetime();
		StartCoroutine(lifetimeCoroutine);
    }

	public void SetVisualEnabled(bool value)
	{
		sprite.enabled = value;
		trail.Clear();
		if (value)
		{
			lifetimeCoroutine = Lifetime();
			StartCoroutine(lifetimeCoroutine);
			bResetting = false;
		}
	}

	private IEnumerator Lifetime()
	{
		yield return new WaitForSeconds(lifeTime);
		bActive = false;
		SetVisualEnabled(false);
	}

	private IEnumerator ResetDelay()
	{
		bResetting = true;
		yield return new WaitForSeconds(0.5f);
		bActive = false;
		SetVisualEnabled(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.GetComponent<Droplet>())
		{
			// energy transfer to leaf
			Leaf leaf = collision.gameObject.GetComponent<Leaf>();
			if (leaf != null)
				energy.LeafEnergy(1, transform.position);
			
			if (!bResetting)
			{
				StopAllCoroutines();
				resetCoroutine = ResetDelay();
				StartCoroutine(resetCoroutine);
			}
		}
	}
}
