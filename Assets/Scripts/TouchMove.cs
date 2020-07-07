using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMove : MonoBehaviour
{
	private Camera cameraMain;
	private RectTransform rt;
	private Transform originalTransform;
	private Transform rangeTransform;
	private bool bMoving = false;
	private bool bReturning = false;

    void Start()
    {
		cameraMain = Camera.main;
		rt = GetComponent<RectTransform>();
		originalTransform = transform.parent;
    }

    void Update()
    {
        if (bMoving)
		{
			FollowTouch();
		}
		else if (bReturning)
		{
			SmoothReturn();
		}
    }

	void FollowTouch()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Vector3 touchWorldPosition = touch.position;
			transform.position = Vector3.Lerp(transform.position, touchWorldPosition, Time.deltaTime * 10f);
		}
	}

	void SmoothReturn()
	{
		transform.SetParent(originalTransform);
		transform.localPosition = Vector3.zero;
		bReturning = false;
	}

	public void InitMover(Transform value)
	{
		rangeTransform = value;
	}

	public void StartTouch()
	{
		bMoving = true;
		transform.SetParent(rangeTransform, true);
		if (GetComponent<LimbUnit>())
			GetComponent<LimbUnit>().SetActive(true);
	}

	public void EndTouch()
	{
		bMoving = false;

		bool bLanded = false;
		if (GetComponent<LimbUnit>())
			GetComponent<LimbUnit>().SetActive(false);
		if (GetComponent<LimbUnit>().bTargeting)
			bLanded = true;
		if (!bLanded)
			bReturning = true;
	}
}
