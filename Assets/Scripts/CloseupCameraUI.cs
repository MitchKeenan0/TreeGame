using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseupCameraUI : MonoBehaviour
{
	private CanvasGroup canvasGroup = null;
	private bool bActive = false;

    void Start()
    {
		canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void SetActive(bool value)
	{
		bActive = value;
		canvasGroup.alpha = bActive ? 1f : 0f;
	}
}
