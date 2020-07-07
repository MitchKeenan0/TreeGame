using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbUnit : MonoBehaviour
{
	public TreeLimb limbPrefab;
	public bool bActive = false;
	public bool bTargeting = false;

	private TreeLimb limbPrototype;
	private Camera cam;

    void Start()
    {
		cam = Camera.main;
		limbPrototype = Instantiate(limbPrefab, transform);
		limbPrototype.SetVisible(false);
    }

    void Update()
    {
		if (bActive)
		{
			CheckOverlap();
			if (bTargeting)
				TargetLimb();
		}
	}

	void CheckOverlap()
	{
		Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
		Debug.DrawRay(ray.origin, ray.direction * 11f, Color.yellow, 1f);

		bTargeting = false;
		RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction * 12f);
		if (hits.Length > 0)
		{
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider.gameObject.GetComponent<TreeLimb>())
				{
					bTargeting = true;
					break;
				}
			}
		}
	}

	void TargetLimb()
	{
		limbPrototype.SetVisible(true);
		limbPrototype.SetLimbLine(Vector3.up);
	}

	public void SetActive(bool value)
	{
		bActive = value;
	}
}
