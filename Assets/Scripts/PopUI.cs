using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUI : MonoBehaviour
{
	public Text valueText;
	public Text backingText;
	public float lifetime = 1f;
	public bool bActive = false;

	private IEnumerator lifetimeCoroutine;
	private Camera cameraMain;
	private CanvasGroup canvasGroup;

    void Awake()
    {
		canvasGroup = GetComponent<CanvasGroup>();
		cameraMain = Camera.main;
    }

	public void PopIt(int value, Vector3 position)
	{
		bActive = true;
		valueText.text = "+" + value.ToString();
		backingText.text = valueText.text;
		Vector3 popScreenPos = cameraMain.WorldToScreenPoint(position);
		popScreenPos.z = 0;
		transform.position = popScreenPos;
		canvasGroup.alpha = 1f;

		lifetimeCoroutine = Lifetime();
		StartCoroutine(lifetimeCoroutine);
	}

	private IEnumerator Lifetime()
	{
		yield return new WaitForSeconds(lifetime);
		bActive = false;
		canvasGroup.alpha = 0f;
	}
}
