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
	private float trailTime = 0f;
	private float trailWidth = 0f;

    void Awake()
    {
		energy = FindObjectOfType<Energy>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		trail = GetComponentInChildren<TrailRenderer>();
		if (trail != null)
		{
			trailTime = trail.time;
			trailWidth = trail.widthMultiplier;
		}
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
		{
			trail.enabled = value;
			trail.Clear();
			trail.time = trailTime;
			trail.widthMultiplier = trailWidth;
		}

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
		StopAllCoroutines();
		SetDropletEnabled(false);
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
		bResetting = false;
	}

	void Update()
	{
		if (bLifetime && bResetting)
		{
			trail.time = Mathf.Lerp(trail.time, 0f, Time.deltaTime * 5f);
			trail.widthMultiplier = Mathf.Lerp(trail.widthMultiplier, 0f, Time.deltaTime * 5f);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.isTrigger && bLifetime && !collision.gameObject.GetComponent<Droplet>())
		{
			// energy transfer to leaf
			Leaf leaf = collision.gameObject.GetComponent<Leaf>();
			if (!leaf)
				leaf = collision.gameObject.GetComponentInParent<Leaf>();
			if (leaf != null)
			{
				energy.LeafEnergy(10, transform.position);
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			
			if (!bResetting)
			{
				StopAllCoroutines();
				resetCoroutine = ResetDelay();
				StartCoroutine(resetCoroutine);
			}
		}
	}
}
