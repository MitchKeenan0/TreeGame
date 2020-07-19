using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSystem : MonoBehaviour
{
	public GameObject limbPrefab;
	public GameObject leafPrefab;
	public float stemLength = 0.5f;

	private Soil soil;
	private List<TreeLimb> limbList;
	private List<Leaf> leafList;

    void Start()
    {
		InitTree();
    }

	void InitTree()
	{
		limbList = new List<TreeLimb>();
		leafList = new List<Leaf>();
		soil = FindObjectOfType<Soil>();

		// create trunk
		GameObject stem = Instantiate(limbPrefab, transform);
		TreeLimb tLimb = stem.GetComponent<TreeLimb>();
		tLimb.SetLimbLine(false, transform.position, transform.up * 0.1f);
		tLimb.SetGrowTargetLength(0.5f);
		tLimb.SetGrowTargetWidth(0.05f);
		limbList.Add(tLimb);
		tLimb.SetColliderEnabled(true);
		tLimb.AttachToRb(soil.GetComponent<Rigidbody2D>());

		PlantPhysics pPhysics = tLimb.GetComponent<PlantPhysics>();
		pPhysics.SetNaturalDirection(Vector3.up);

		tLimb.ImbueLeaf();
		tLimb.bLifeCritical = true;
	}
}
