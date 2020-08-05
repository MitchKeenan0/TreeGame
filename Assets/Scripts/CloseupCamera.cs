using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseupCamera : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void SetPosition(Vector3 value)
	{
		transform.position = value;
	}
}
