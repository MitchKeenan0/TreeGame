using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPhysics : MonoBehaviour
{
	public float comfortableAngle = 5f;
	public float rigidity = 5f;
	public float spring = 1f;

	private Rigidbody2D rb;
	private Vector3 naturalDirection = Vector3.zero;
	private Vector3 forceDirection = Vector3.zero;

	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }
	
    void FixedUpdate()
    {
		float angleToNatural = Mathf.Abs(Vector3.Angle(transform.up, naturalDirection));
		if (angleToNatural >= comfortableAngle)
		{
			float forceX = Mathf.Clamp((naturalDirection.x - transform.up.x), -1f, 1f);
			Vector3 torqueVector = (transform.right * forceX) * rigidity;
			forceDirection = Vector3.Lerp(forceDirection, torqueVector, Time.fixedDeltaTime * spring);
			rb.AddForce(forceDirection);
			Debug.DrawLine(transform.position, (transform.position + torqueVector), Color.blue);
		}
    }

	public void SetNaturalDirection(Vector3 value)
	{
		naturalDirection = value.normalized;
	}
}
