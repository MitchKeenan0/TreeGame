using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	public float maxWindSpeed = 1f;
	public float minWindSpeed = 0f;

	public float windChangeInterval = 1f;
	public float intervalDeviation = 0.5f;

	private List<Rigidbody2D> rbList;
	private IEnumerator changeCoroutine;
	private Vector2 windDirection = Vector3.zero;

    void Start()
    {
		rbList = new List<Rigidbody2D>();
		changeCoroutine = WindChange(windChangeInterval);
		StartCoroutine(changeCoroutine);
    }

    void FixedUpdate()
    {
        if (rbList != null && (rbList.Count > 0))
		{
			foreach (Rigidbody2D rb in rbList)
			{
				if (rb != null)
				{
					float massScale = rb.mass;
					rb.AddForce(windDirection * massScale);
					Debug.DrawLine(rb.transform.position, rb.transform.position + new Vector3(windDirection.x, windDirection.y, 0f));
				}
			}
		}
    }

	void GenerateWindDirection()
	{
		windDirection.x = Random.Range(-1f, 1f) * Random.Range(minWindSpeed, maxWindSpeed);
		windDirection.y = Random.Range(-0.1f, 0.1f) * Random.Range(minWindSpeed, maxWindSpeed);
	}

	private IEnumerator WindChange(float interval)
	{
		while (true)
		{
			GenerateWindDirection();
			float deviatedTime = windChangeInterval + Random.Range(-intervalDeviation, intervalDeviation);
			yield return new WaitForSeconds(deviatedTime);
		}
	}

	public void AddRb(Rigidbody2D rb)
	{
		if (rbList == null)
			rbList = new List<Rigidbody2D>();
		if (rbList != null)
			rbList.Add(rb);
	}
}
