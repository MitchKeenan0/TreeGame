using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLimb : MonoBehaviour
{
	public float length = 2.1f;
	private LineRenderer lineRenderer;

    void Awake()
    {
		lineRenderer = GetComponent<LineRenderer>();
    }

	public void SetLimbLine(Vector3 origin, Vector3 extent)
	{
		lineRenderer.SetPosition(0, origin);
		lineRenderer.SetPosition(1, extent.normalized * length);
	}

	public void SetVisible(bool value)
	{
		lineRenderer.enabled = value;
	}
}
