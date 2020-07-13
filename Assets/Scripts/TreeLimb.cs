using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLimb : MonoBehaviour
{
	private LineRenderer lineRenderer;
	private BoxCollider2D[] boxColliders;
	private float boxLength = 0f;

    void Awake()
    {
		lineRenderer = GetComponent<LineRenderer>();
		boxColliders = GetComponents<BoxCollider2D>();
		boxLength = boxColliders[0].size.x;
    }

	public void SetLimbLine(Vector3 origin, Vector3 extent)
	{
		transform.position = origin;
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, (origin + extent) - transform.position);

		foreach (BoxCollider2D box in boxColliders)
		{
			float lineLength = Vector3.Distance(origin, origin + extent);
			box.offset = new Vector2(0f, (lineLength / 2f));
			box.size = new Vector2(boxLength, lineLength);
		}
	}

	public void SetVisible(bool value)
	{
		lineRenderer.enabled = value;
	}

	public void SetColliderEnabled(bool value)
	{
		foreach (BoxCollider2D box in boxColliders)
			box.enabled = value;
	}

	public Vector3 GetLimbCentre()
	{
		Vector3 centre = Vector3.zero;
		BoxCollider2D box = boxColliders[0];
		centre = box.bounds.center;
		return centre;
	}

	public Vector3 GetClosestBoundsPoints(Vector3 refVector)
	{
		BoxCollider2D box = boxColliders[0];
		Vector3 closePoint = Vector3.zero;
		Vector2 closer = box.ClosestPoint(refVector);
		closePoint.x = closer.x;
		closePoint.y = closer.y;
		return closePoint;
	}
}
