using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffsetSetter : MonoBehaviour {
  [SerializeField] private CameraSection cameraSection;
  [SerializeField] private Vector3 offset;
  
	void OnTriggerEnter2D(Collider2D other) {
     if (other.tag == "Player") {
       cameraSection.SetOffset(this.offset);
     }
  }
}
