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
	private CloseupCameraUI closeupUI = null;
	private CloseupCamera closeupCamera = null;

    void Start()
    {
		cam = Camera.main;
		limbPrototype = Instantiate(limbPrefab, null);
		limbPrototype.SetVisible(false);
		limbPrototype.SetColliderEnabled(false);
		closeupUI = FindObjectOfType<CloseupCameraUI>();
		closeupUI.SetActive(false);
		closeupCamera = FindObjectOfType<CloseupCamera>();
    }

    void FixedUpdate()
    {
		if (bActive)
		{
			CheckOverlap();
			if (bTargeting && (targetLimb != null))
				TargetLimb();
		}
	}

	void CheckOverlap()
	{
		if (Input.touchCount > 0)
		{
			Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
			bTargeting = false;
			Vector2 closestVector = Vector2.positiveInfinity;
			GameObject closestObj = null;
			RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction * 1.1f);
			if (hits.Length > 0)
			{
				foreach (RaycastHit2D hit in hits)
				{
					TreeLimb tLimb = hit.collider.gameObject.GetComponent<TreeLimb>();
					if (tLimb != null)
					{
						bTargeting = true;
						Vector2 myPosition = cam.ScreenToWorldPoint(transform.position);
						Vector3 tLimbClosestPoint = tLimb.GetClosestBoundsPoints(hit.point);
						Vector2 limbHitPosition = new Vector2(tLimbClosestPoint.x, tLimbClosestPoint.y);
						Vector2 toHit = (limbHitPosition - myPosition);
						if (toHit.magnitude < closestVector.magnitude)
						{
							closestVector = toHit;
							closestObj = hit.collider.gameObject;
						}
					}
				}

				if (bTargeting && (closestObj != null))
				{
					if (closestObj.transform.root.GetComponent<Player>())
					{
						targetLimb = closestObj.GetComponent<TreeLimb>();
						closeupUI.SetActive(true);
					}
				}
			}
			else
			{
				targetLimb = null;
				closeupUI.SetActive(false);
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

		closeupCamera.SetPosition(limbPrototype.transform.position + (Vector3.forward * -0.5f));
	}

	public void SetActive(bool value)
	{
		bActive = value;
	}

	public void Spend()
	{
		if (targetLimb != null)
		{
			limbPrototype.transform.SetParent(targetLimb.transform);
			limbPrototype.AttachToRb(targetLimb.GetComponent<Rigidbody2D>());
			limbPrototype.SetColliderEnabled(true);

			limbPrototype.SetGrowTargetLength(targetLimb.GetLimbLength() * 0.36f);
			limbPrototype.SetGrowTargetWidth(0.02f);
			limbPrototype.ParentalGrowth(1.1f, 1.05f);

			PlantPhysics pPhysics = limbPrototype.GetComponent<PlantPhysics>();
			pPhysics.SetNaturalDirection(limbPrototype.transform.up);
			limbPrototype.ImbueLeaf();

			closeupUI.SetActive(false);

			bTargeting = false;
			Destroy(gameObject);
		}
	}
}
