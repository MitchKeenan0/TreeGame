using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTree : MonoBehaviour
{
	public GameObject limbPrefab;
	public GameObject leafPrefab;
	public float stemLength = 0.5f;
	public float sproutInterval = 3f;

	private Soil soil;
	private Energy energy;
	private List<TreeLimb> limbList;
	private List<Leaf> leafList;
	private TreeLimb tLimb;
	private IEnumerator sproutCoroutine;

    void Start()
    {
		limbList = new List<TreeLimb>();
		leafList = new List<Leaf>();
		soil = FindObjectOfType<Soil>();
		energy = GetComponent<Energy>();

		// create trunk
		GameObject stem = Instantiate(limbPrefab, transform);
		TreeLimb tLimb = stem.GetComponent<TreeLimb>();
		tLimb.SetLimbLine(false, transform.position, transform.up * 0.1f);
		tLimb.SetGrowTargetLength(0.3f);
		tLimb.SetGrowTargetWidth(0.02f);
		tLimb.SetColliderEnabled(true);
		tLimb.AttachToRb(soil.GetComponent<Rigidbody2D>());
		limbList.Add(tLimb);

		PlantPhysics pp = stem.GetComponent<PlantPhysics>();
		pp.SetComfortableAngle(3f);

		PlantPhysics pPhysics = tLimb.GetComponent<PlantPhysics>();
		pPhysics.SetNaturalDirection(Vector3.up);

		tLimb.ImbueLeaf();
		tLimb.bLifeCritical = true;

		sproutCoroutine = SproutInterval();
		StartCoroutine(sproutCoroutine);
	}

    private IEnumerator SproutInterval()
	{
		while (true)
		{
			float deviation = sproutInterval * 0.5f;
			float interval = sproutInterval + Random.Range(-deviation, deviation);
			yield return new WaitForSeconds(interval);

			if (energy.GetEnergy() >= 1)
			{
				SpawnNewLimb();
				energy.SpendEnergy(1);
			}
		}
	}

	void SpawnNewLimb()
	{
		bool bLimb = false;
		int tries = 0;
		while (!bLimb && (tries < (limbList.Count * limbList.Count)))
		{
			tries++;
			int parentLimbIndex = Random.Range(0, limbList.Count);
			if ((limbList[parentLimbIndex] != null))
			{
				TreeLimb parentLimb = limbList[parentLimbIndex];
				float parentLength = parentLimb.GetLimbLength();
				float minimumLength = Mathf.Clamp(Mathf.Sqrt(parentLength), 0f, parentLength * 0.9f);
				float sproutLimbHeight = Random.Range(minimumLength, parentLength);
				Vector3 sproutPosition = parentLimb.transform.position + (parentLimb.transform.up * sproutLimbHeight);
				Quaternion spawnRotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-50f, 50f)));

				GameObject stem = Instantiate(limbPrefab, Vector3.zero, spawnRotation);
				stem.transform.SetParent(parentLimb.transform, false);
				stem.transform.position = sproutPosition;

				TreeLimb stemLimb = stem.GetComponent<TreeLimb>();
				stemLimb.SetLimbLine(true, sproutPosition, stemLimb.transform.up * 0.1f);
				stemLimb.AttachToRb(parentLimb.GetComponent<Rigidbody2D>());
				stemLimb.SetColliderEnabled(true);
				stemLimb.SetGrowTargetLength(parentLimb.GetLimbLength() * 0.36f);
				stemLimb.SetGrowTargetWidth(0.02f);
				stemLimb.ParentalGrowth(1.1f, 1.02f);

				PlantPhysics pPhysics = stemLimb.GetComponent<PlantPhysics>();
				pPhysics.SetNaturalDirection(stemLimb.transform.up);
				stemLimb.ImbueLeaf();

				limbList.Add(stemLimb);

				bLimb = true;
			}
		}
	}
}
