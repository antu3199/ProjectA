using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMe : MonoBehaviour {
    
    [SerializeField] private float amplitude = 1;
	[SerializeField] private float rotationSpeed = 1;
    private Vector3 initialPosition;


	// Use this for initialization
	void Start () {
		this.initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	  float sin = Mathf.Sin(Time.time * rotationSpeed);
	  float cos = Mathf.Cos(Time.time * rotationSpeed);
      this.transform.position = this.initialPosition + new Vector3(sin * amplitude, cos * amplitude, 0);
		
	}
}
