using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSprouter : MonoBehaviour
{
	public GameObject botSproutPrefab;
	public int initialSprouts = 3;
	public Vector3 spawnOffset = Vector3.zero;
	public float spawnInterval = 6f;
	public float intervalDeviation = 3f;

	private Soil soil;
	private IEnumerator spawnCoroutine;

	void Start()
    {
		soil = FindObjectOfType<Soil>();
		for(int i = 0; i < initialSprouts; i++)
			SpawnSprout();
		spawnCoroutine = SproutInterval();
		StartCoroutine(spawnCoroutine);
    }

	void SpawnSprout()
	{
		Vector3 spawnPos = Vector3.zero;
		spawnPos.y = spawnOffset.y;
		float offsetX = Random.Range(-spawnOffset.x, spawnOffset.x);
		if (Mathf.Abs(offsetX) < 1f)
			offsetX *= 3f;
		spawnPos.x = offsetX;
		spawnPos.z = 0f;
		GameObject sprout = Instantiate(botSproutPrefab);
		sprout.transform.position = spawnPos;
	}

    private IEnumerator SproutInterval()
	{
		float interval = spawnInterval + Random.Range(0f, intervalDeviation);
		yield return new WaitForSeconds(interval);
		SpawnSprout();
	}
}
