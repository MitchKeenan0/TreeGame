using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbUnit : MonoBehaviour
{
	public TreeLimb limbPrefab;
	public bool bActive = false;
	public bool bTargeting = false;

	private TreeLimb limbPrototype = null;
	private TreeLimb targetLimb = null;
	private Camera cam = null;

    void Start()
    {
		cam = Camera.main;
		limbPrototype = Instantiate(limbPrefab, null);
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
		if (Input.touchCount > 0)
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
						targetLimb = hit.collider.gameObject.GetComponent<TreeLimb>();
						bTargeting = true;
						break;
					}
				}
			}
			else
			{
				targetLimb = null;
			}
		}
	}

	void TargetLimb()
	{
		limbPrototype.SetVisible(true);
		Vector3 limbPosition = cam.ScreenToWorldPoint(transform.position);
		limbPosition.z = 0;
		Vector3 limbDirection = limbPosition + (limbPosition - (targetLimb.transform.position));
		limbPrototype.transform.rotation = Quaternion.LookRotation(limbDirection);
		limbPrototype.SetLimbLine(limbPosition, Vector3.up);
	}

	public void SetActive(bool value)
	{
		bActive = value;
	}
}
