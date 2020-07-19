using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public float targetDistance = 5f;
	public float targetOffset = 0f;
	public float moveSpeed = 1f;
	public float initialOffset = 5f;
	public float moveIntervalTime = 1f;
	public float distanceStep = 1.05f;
	public float offsetStep = 0.1f;

	private Camera cam = null;
	private float distance = 0f;
	private float offset = 0f;
	private float delta = 0f;
	private Vector3 camMovePosition = Vector3.zero;
	private IEnumerator moveCoroutine;
	private Leaf[] leafs;

    void Start()
    {
		cam = GetComponent<Camera>();
		offset = initialOffset;
		transform.position += Vector3.up * initialOffset;
		camMovePosition = transform.position;
		moveCoroutine = CamMoveInterval(1f);
		StartCoroutine(moveCoroutine);
    }

	void Update()
	{
		delta = Time.deltaTime * moveSpeed;
		distance = Mathf.Lerp(distance, targetDistance, delta);
		offset = Mathf.Lerp(offset, targetOffset, delta);
		camMovePosition.y = offset;
		cam.orthographicSize = distance;
		transform.position = camMovePosition;
	}

	private IEnumerator CamMoveInterval(float interval)
	{
		while(true)
		{
			yield return new WaitForSeconds(interval);

			float screenMidY = Screen.height / 2f;
			leafs = FindObjectsOfType<Leaf>();
			foreach(Leaf lf in leafs)
			{
				float leafScreenY = (cam.WorldToScreenPoint(lf.transform.position)).y;
				if (leafScreenY >= screenMidY)
				{
					ScaleTargetDistance(distanceStep);
					ScaleTargetOffset(offsetStep);
					break;
				}
			}
		}
	}

	public void ScaleTargetDistance(float value)
	{
		targetDistance *= value;
	}

	public void ScaleTargetOffset(float value)
	{
		targetOffset += value;
	}
}
