﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubesPooled : MonoBehaviour {

	public OnTouchDestroyPooled pooledObject;
	float time = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > 0.5) {
			OnTouchDestroyPooled newObject = pooledObject.GetPooledInstance<OnTouchDestroyPooled> (Vector3.zero);
			newObject.transform.position = transform.position;
			time = 0;
		}
	}
}
