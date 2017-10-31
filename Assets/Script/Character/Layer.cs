using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour {
    public int CurrentLayer;
    public Transform UpPosition;
    public Transform DownPosition;

	// Use this for initialization
	void Awake () {
        gameObject.name = "Layer" + CurrentLayer;
	}

}
