using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSystem : MonoBehaviour
{
	public GameObject limbPrefab;
	public GameObject leafPrefab;
	public float stemLength = 0.5f;

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

		// new tree
		GameObject stem = Instantiate(limbPrefab, transform);
		TreeLimb tl = stem.GetComponent<TreeLimb>();
		Vector3 stemVector = (transform.up * stemLength);
		tl.SetLimbLine(stemVector);
		limbList.Add(tl);
		GameObject leaf = Instantiate(leafPrefab, stem.transform);
		leaf.transform.position = transform.position + stemVector;
		Leaf lf = leaf.GetComponent<Leaf>();
		leafList.Add(lf);
	}
}
