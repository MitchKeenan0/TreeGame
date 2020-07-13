using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
	public float lifeTime = 5f;
	public bool bLifetime = false;
	public bool bActive = false;

	private SpriteRenderer sprite;
	private TrailRenderer trail;
	private Collider2D col;
	private IEnumerator lifetimeCoroutine;
	private IEnumerator resetCoroutine;
	private Energy energy;
	private bool bResetting = false;

    void Awake()
    {
		energy = FindObjectOfType<Energy>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		trail = GetComponentInChildren<TrailRenderer>();
		col = GetComponent<Collider2D>();
		col.enabled = false;
		if (bLifetime)
		{
			lifetimeCoroutine = Lifetime();
			StartCoroutine(lifetimeCoroutine);
		}
    }

	public void SetDropletEnabled(bool value)
	{
		if (sprite != null)
			sprite.enabled = value;
		if (trail != null)
			trail.Clear();
		if (value)
		{
			if (bLifetime)
			{
				lifetimeCoroutine = Lifetime();
				StartCoroutine(lifetimeCoroutine);
			}
			bResetting = false;
		}
		if (GetComponent<Sunbeam>())
			GetComponent<Sunbeam>().SetActive(value);
		col.enabled = value;
	}

	public void Cancel()
	{
		bActive = false;
		SetDropletEnabled(false);
		StopAllCoroutines();
	}

	private IEnumerator Lifetime()
	{
		yield return new WaitForSeconds(lifeTime);
		bActive = false;
		SetDropletEnabled(false);
	}

	private IEnumerator ResetDelay()
	{
		bResetting = true;
		yield return new WaitForSeconds(0.5f);
		bActive = false;
		SetDropletEnabled(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (bLifetime && !collision.gameObject.GetComponent<Droplet>())
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
