using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPhysics : MonoBehaviour
{
	public float comfortableAngle = 5f;
	public float breakAngle = 30f;
	public float rigidity = 5f;
	public float spring = 1f;

	private Rigidbody2D rb;
	private Vector3 naturalDirection = Vector3.zero;
	private Vector3 forceDirection = Vector3.zero;
	private bool bConnectedToTree = true;
	private float forceScale = 1f;

	void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
    }
	
    void FixedUpdate()
    {
		float angleToNatural = Mathf.Abs(Vector3.Angle(transform.up, naturalDirection));
		if (bConnectedToTree && (angleToNatural >= comfortableAngle))
		{
			float forceX = Mathf.Clamp((naturalDirection.x - transform.up.x), -1f, 1f);
			Vector3 torqueVector = (transform.right * forceX) * rigidity * forceScale;
			forceDirection = Vector3.MoveTowards(forceDirection, torqueVector, Time.deltaTime * spring);
			rb.AddForce(forceDirection);
			Debug.DrawLine(transform.position, (transform.position + torqueVector), Color.blue);
		}
		if (angleToNatural >= breakAngle)
		{
			BreakOffTree();
		}
    }

	void BreakOffTree()
	{
		bool gg = false;
		TreeLimb tLimb = GetComponent<TreeLimb>();
		if (tLimb.bLifeCritical)
		{
			FindObjectOfType<Game>().GameOver();
			gg = true;
		}

		HingeJoint2D joint = GetComponent<HingeJoint2D>();
		joint.connectedBody = null;
		joint.enabled = false;
		transform.SetParent(null);
		bConnectedToTree = false;
		GetComponentInChildren<Leaf>().enabled = false;
		if (!gg)
			Destroy(gameObject, 0.5f);
	}

	public void SetNaturalDirection(Vector3 value)
	{
		naturalDirection = value.normalized;
	}

	public void SetForceScale(float value)
	{
		forceScale = value;
		rb.drag = value * 100f;
	}
}
