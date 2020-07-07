using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLimb : MonoBehaviour
{
	private LineRenderer lineRenderer;

    void Awake()
    {
		lineRenderer = GetComponent<LineRenderer>();
    }

	public void SetLimbLine(Vector3 extent)
	{
		lineRenderer.SetPosition(1, extent);
	}

	public void SetVisible(bool value)
	{
		lineRenderer.enabled = value;
	}
}
