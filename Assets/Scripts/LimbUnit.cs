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
		limbPrototype.SetColliderEnabled(false);
    }

    void FixedUpdate()
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
			bTargeting = false;
			Vector2 closestVector = new Vector2(999f, 999f);
			GameObject closestObj = null;
			RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction * 12f);
			if (hits.Length > 0)
			{
				foreach (RaycastHit2D hit in hits)
				{
					if (hit.collider.gameObject.GetComponent<TreeLimb>())
					{
						bTargeting = true;
						Vector2 myPosition = cam.ScreenToWorldPoint(transform.position);
						Vector2 toHit = hit.collider.ClosestPoint(myPosition) - myPosition;
						if (toHit.magnitude < closestVector.magnitude)
						{
							closestVector = toHit;
							closestObj = hit.collider.gameObject;
						}
					}
				}

				if (bTargeting && closestObj != null)
					targetLimb = closestObj.GetComponent<TreeLimb>();
			}
			else
			{
				targetLimb = null;
			}
		}
	}

	void TargetLimb()
	{
		Vector3 limbPosition = cam.ScreenToWorldPoint(transform.position);
		limbPosition.z = 0;
		Vector3 sproutPosition = targetLimb.GetClosestBoundsPoints(limbPosition);
		Vector3 limbDirection = (limbPosition - targetLimb.GetLimbCentre()).normalized;

		limbPrototype.transform.rotation = Quaternion.Lerp(limbPrototype.transform.rotation, Quaternion.LookRotation(Vector3.forward, limbDirection), 10*Time.deltaTime);
		limbPrototype.SetLimbLine(true, sproutPosition, limbPrototype.transform.up * 0.1f);
		limbPrototype.SetVisible(true);
	}

	public void SetActive(bool value)
	{
		bActive = value;
	}

	public void Spend()
	{
		limbPrototype.transform.SetParent(targetLimb.transform);
		limbPrototype.AttachToRb(targetLimb.GetComponent<Rigidbody2D>());
		limbPrototype.SetColliderEnabled(true);

		limbPrototype.SetGrowTargetLength(0.6f);
		targetLimb.SetGrowTargetLength(1.6f);

		PlantPhysics pPhysics = limbPrototype.GetComponent<PlantPhysics>();
		pPhysics.SetNaturalDirection(limbPrototype.transform.up);
		limbPrototype.ImbueLeaf();

		bTargeting = false;
		Destroy(gameObject);
	}
}
