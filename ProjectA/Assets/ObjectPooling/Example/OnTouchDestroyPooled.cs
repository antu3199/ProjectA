using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchDestroyPooled : PooledObject {

	public void OnCollisionEnter (Collision collider){
		ReturnToPool ();
	}
}
