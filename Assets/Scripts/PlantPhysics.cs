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
	private bool bActive = false;
	private float forceScale = 1f;
	private float forceScaleScalar = 1f;

	void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
		naturalDirection = transform.up;
    }
	
    void FixedUpdate()
    {
		if (bActive)
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
			if (bConnectedToTree && (angleToNatural >= breakAngle))
			{
				BreakOffTree();
			}
		}
    }

	void BreakOffTree()
	{
		bool gg = false;
		TreeLimb tLimb = GetComponent<TreeLimb>();
		if (tLimb.bLifeCritical)
		{
			if (transform.root.GetComponent<Player>())
			{
				FindObjectOfType<Game>().GameOver();
				gg = true;
			}
		}

		HingeJoint2D joint = GetComponent<HingeJoint2D>();
		rb.velocity = joint.connectedBody.velocity;
		joint.connectedBody = null;
		joint.enabled = false;
		transform.SetParent(null);
		bConnectedToTree = false;
		GetComponentInChildren<Leaf>().enabled = false;

		PlantPhysics[] childrenPhys = GetComponentsInChildren<PlantPhysics>();
		foreach (PlantPhysics phys in childrenPhys)
			phys.SetActive(false);
		SetActive(false);

		if (!gg)
			Destroy(gameObject, 5f);
	}

	public void SetActive(bool value)
	{
		bActive = value;
		if (!bActive)
		{
			rb.drag = 0f;
			rb.gravityScale = 1f;
		}
	}

	public void SetNaturalDirection(Vector3 value)
	{
		naturalDirection = value.normalized;
		SetActive(true);
	}

	public void SetForceScale(float value)
	{
		if (bConnectedToTree)
		{
			forceScale = value * forceScaleScalar;
			rb.drag = value * 10000f;
		}
	}

	public void SetComfortableAngle(float value)
	{
		comfortableAngle = value;
	}

	public void ScaleForceValue(float value)
	{
		forceScaleScalar *= value;
	}
}
