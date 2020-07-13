using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunbeam : MonoBehaviour
{
	public float maxBeamWidth = 1f;
	public float minBeamWidth = 0.1f;
	public float scaleSpeed = 1f;
	public float scaleDeviation = 0.5f;
	public float raycastsPerUnit = 10f;
	public float startRotationZ = 23f;

	private float timer = 0f;
	private float actualScaleSpeed = 0f;
	private float boxColliderLength = 0f;
	private bool bExtant = true;
	private bool bActive = false;
	private LineRenderer lineRenderer = null;
	private BoxCollider2D boxCollider = null;
	private Droplet droplet = null;
	private IEnumerator lifetimeCoroutine;

    void Start()
    {
		lineRenderer = GetComponent<LineRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
		boxColliderLength = boxCollider.size.y;
		droplet = GetComponent<Droplet>();
		lineRenderer.widthMultiplier = 0f;
		actualScaleSpeed = Random.Range(scaleSpeed - scaleDeviation, scaleSpeed + scaleDeviation);
		AdjustRotation();
    }

	void Update()
	{
		if (bActive)
		{
			if (bExtant)
			{
				lineRenderer.widthMultiplier += Time.deltaTime * actualScaleSpeed;
				float lineWidth = lineRenderer.widthMultiplier;
				if (lineWidth >= maxBeamWidth)
					bExtant = false;
			}
			else
			{
				lineRenderer.widthMultiplier -= Time.deltaTime * actualScaleSpeed;
				float lineWidth = lineRenderer.widthMultiplier;
				if (lineWidth <= minBeamWidth)
				{
					lineRenderer.widthMultiplier = 0f;
					bExtant = true;
					droplet.Cancel();
				}
			}

			Vector2 boxSize = new Vector2(lineRenderer.widthMultiplier, boxColliderLength);
			boxCollider.size = boxSize;
		}
	}

	void AdjustRotation()
	{
		Quaternion rote = transform.rotation;
		float randomZ = Random.Range(startRotationZ * 0.75f, startRotationZ * 1.25f);
		rote.z = randomZ;
		transform.localRotation = rote;
	}

	public void SetActive(bool value)
	{
		bActive = value;
		if (!lineRenderer)
			lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.widthMultiplier = 0f;
		bExtant = value;
		if (!value)
			AdjustRotation();
	}
}
