using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneric : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Managers.controller.enemies.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
