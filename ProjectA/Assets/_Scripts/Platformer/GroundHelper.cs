using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHelper : MonoBehaviour {
  
  [SerializeField] private PlayerController controller;
  

  void Start() {
  }

  void OnTriggerStay2D(Collider2D other) {

    if (other.gameObject.tag == "Ground") {
      controller.isGrounded = true;
    }

  }
  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Ground") {
      controller.isGrounded = false;
    }
  }

}
