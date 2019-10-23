using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMe : MonoBehaviour {
 
  [SerializeField] private Rigidbody2D rigidBody;
  [SerializeField] private float speed;
  [SerializeField] private Vector2 amplitude;

  [SerializeField] bool clampToZero = false;

	void Start () {
		this.rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
    float x = Mathf.Sin(Time.time * speed) * amplitude.x;
    float y = Mathf.Sin(Time.time * speed) * amplitude.y;
  
    this.rigidBody.velocity = new Vector2(x, y);
    
	}
}
