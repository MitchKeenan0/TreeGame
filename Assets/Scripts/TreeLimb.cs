using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLimb : MonoBehaviour
{
	public Leaf leafPrefab;
	public Transform limbRender;
	public float growSpeed = 0.3f;

	private Rigidbody2D rb;
	private HingeJoint2D joint;
	private BoxCollider2D[] boxColliders;
	private Vector3 limbExtent = Vector3.zero;
	private float sproutLength = 0.1f;
	private float matureLength = 1f;
	private float growDistance = 0.1f;
	private float growTime = 0f;
	private float boxLength = 0f;
	private bool bAlive = false;
	private List<Leaf> leafList;

    void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
		joint = GetComponent<HingeJoint2D>();
		boxColliders = GetComponents<BoxCollider2D>();
		boxLength = boxColliders[0].size.x;
		leafList = new List<Leaf>();
		FindObjectOfType<Wind>().AddRb(rb);
	}

	void Update()
	{
		if (bAlive)
		{
			if (growDistance < 1f)
			{
				float delta = Time.deltaTime * growSpeed;
				growTime += delta;
				growDistance = Mathf.Lerp(sproutLength, matureLength, growTime);
				SetLimbLine(false, transform.position, (transform.up * growDistance));
			}
		}
	}

	public void SetLimbLine(bool bSetLimbOrigin, Vector3 origin, Vector3 extent)
	{
		//if (bSetLimbOrigin)
			transform.position = origin;

		limbExtent = origin + extent;

		Vector2 lineSize = Vector2.zero;
		Vector2 lineOffset = Vector2.zero;
		float lineLength = Vector3.Distance(origin, limbExtent);
		lineOffset = new Vector2(0f, (lineLength / 2f));
		lineSize = new Vector2(boxLength, lineLength);
		foreach (BoxCollider2D box in boxColliders)
		{
			box.offset = lineOffset;
			box.size = lineSize;
		}

		limbRender.localPosition = new Vector3(0f, (lineLength / 2f), 0f);
		limbRender.localScale = lineSize;

		if (leafList != null && (leafList.Count > 0))
		{
			foreach (Leaf lf in leafList)
				lf.transform.position = limbExtent;
		}
	}

	public void AttachToRb(Rigidbody2D rb)
	{
		joint.connectedBody = rb;
	}

	public void ImbueLeaf()
	{
		Leaf leafObj = Instantiate(leafPrefab, transform);
		leafObj.transform.position = limbExtent;
		Vector3 leafEulers = new Vector3(0f, 0f, Random.Range(-50f, 50f));
		leafObj.transform.rotation = Quaternion.Euler(leafEulers);
		leafList.Add(leafObj);
		leafObj.ConnectToRb(rb);
		FindObjectOfType<Wind>().AddRb(leafObj.GetComponent<Rigidbody2D>());
		PlantPhysics pPhysics = leafObj.GetComponent<PlantPhysics>();
		pPhysics.SetNaturalDirection(leafObj.transform.up);
	}

	public void SetVisible(bool value)
	{
		limbRender.GetComponent<MeshRenderer>().enabled = value;
	}

	public void SetColliderEnabled(bool value)
	{
		foreach (BoxCollider2D box in boxColliders)
			box.enabled = value;
		GetComponent<PlantPhysics>().enabled = value;
		bAlive = value;
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
