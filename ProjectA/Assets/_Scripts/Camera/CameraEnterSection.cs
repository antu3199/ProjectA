using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnterSection : MonoBehaviour {
  public enum MovementDirection {
    LEFT,
    RIGHT,
    UP,
    DOWN
  }

   [System.Serializable]
   public class RespawnPoint {
     public Transform playerRespawnPoint;
     public Vector3 cameraTransform{get; set;}
   }

  [SerializeField] RespawnPoint respawnPoint;

  [SerializeField] private CameraInitialSection cameraInitialSection;
  [SerializeField] private MovementDirection movementDirection;
  [SerializeField] private float moveTime = 0.25f;
	private BoxCollider2D boxCollider;
	Camera referenceCamera;
  private Vector3 offset = Vector3.zero;
  private Vector3 location;
  

  public void Initialize() {
		this.boxCollider = GetComponent<BoxCollider2D>();
    offset = Vector3.zero;
    referenceCamera = Camera.main;
    this.respawnPoint.cameraTransform = new Vector3(cameraInitialSection.transform.position.x, cameraInitialSection.transform.position.y, Camera.main.transform.position.z);
  }

  public RespawnPoint GetRespawnPoint() {
    return this.respawnPoint;
  }


	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
      bool rightSide = false;
      
      switch (this.movementDirection) {
        case MovementDirection.LEFT:
          rightSide = (other.gameObject.transform.position.x < this.gameObject.transform.position.x);
        break;
        case MovementDirection.RIGHT:
          rightSide = (other.gameObject.transform.position.x > this.gameObject.transform.position.x);
        break;
        case MovementDirection.UP:
          rightSide = (other.gameObject.transform.position.y > this.gameObject.transform.position.y);
        break;
        case MovementDirection.DOWN:
          rightSide = (other.gameObject.transform.position.y < this.gameObject.transform.position.y);
        break;
      }
      
      if (rightSide) {
        location = new Vector3(this.cameraInitialSection.transform.position.x, this.cameraInitialSection.transform.position.y, referenceCamera.transform.position.z);
        LeanTween.cancel(referenceCamera.gameObject);
        LeanTween.move(referenceCamera.gameObject, location, moveTime);
        Managers.gameState.savePointIndex = Managers.controller.respawnPoints.IndexOf(this.respawnPoint);
      }

		}
	}

}
