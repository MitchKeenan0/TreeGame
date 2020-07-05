using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
	public float lifeTime = 5f;
	public bool bActive = false;

	private SpriteRenderer spriteRenderer;
	private IEnumerator lifetimeCoroutine;
	private Energy energy;

    void Awake()
    {
		energy = FindObjectOfType<Energy>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		lifetimeCoroutine = Lifetime();
		StartCoroutine(lifetimeCoroutine);
    }

	public void SetVisualEnabled(bool value)
	{
		spriteRenderer.enabled = value;
		if (value)
		{
			lifetimeCoroutine = Lifetime();
			StartCoroutine(lifetimeCoroutine);
		}
	}

	private IEnumerator Lifetime()
	{
		yield return new WaitForSeconds(lifeTime);
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

			StopAllCoroutines();
			bActive = false;
			SetVisualEnabled(false);
		}
	}
}
