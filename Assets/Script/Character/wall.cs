using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class wall : MonoBehaviour {

    TilemapCollider2D tilemapCollider;
	// Use this for initialization
	void Start () {
        tilemapCollider = GetComponent<TilemapCollider2D>();
        tilemapCollider.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        tilemapCollider.enabled = true;
    }
}
