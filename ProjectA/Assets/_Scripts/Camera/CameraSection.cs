using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSection : MonoBehaviour {
  [SerializeField] private Vector3 offset;
  [SerializeField] private float moveTime = 0.25f;
  private BoxCollider2D boxCollider;
  private Vector2 cameraSize;
	Camera camera;
  float maxX;
  float maxY;
  float minX;
  float minY;
  float halfCameraSizeX;
  float halfCameraSizeY;
  float targetX;
  float targetY;

  private TimeManager.ScheduledTask task = new TimeManager.ScheduledTask(1, null);

  public void SetOffset(Vector3 offset) {
    this.offset = offset;
  }

	void Start () {
    this.boxCollider = GetComponent<BoxCollider2D>();
    camera = Camera.main;
		cameraSize = new Vector2((camera.aspect * 2f * camera.orthographicSize), (2f * camera.orthographicSize));
    this.halfCameraSizeX = cameraSize.x / 2f;
    this.halfCameraSizeY = cameraSize.y / 2f;
    maxX = this.transform.position.x + this.boxCollider.size.x / 2;
    maxY = this.transform.position.y + this.boxCollider.size.y / 2;
    minX = this.transform.position.x - this.boxCollider.size.x / 2;
    minY = this.transform.position.y - this.boxCollider.size.y / 2;

	}

  void Update() {

  }
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
      targetX = camera.transform.position.x;
      targetY = camera.transform.position.y;

      bool go = false;
      Vector3 reference = other.gameObject.transform.position - offset;

      if (reference.x + this.halfCameraSizeX < maxX && reference.x - this.halfCameraSizeX > minX) {
        targetX = other.gameObject.transform.position.x - offset.x;
        go = true;
      }
      
      if (reference.y + this.halfCameraSizeY < maxY && reference.y - this.halfCameraSizeY > minY) {
        targetY = other.gameObject.transform.position.y - offset.y;
        go = true;
      }
      
      if (go) {
       LeanTween.cancel(camera.gameObject);
       // float initialX = this.camera.transform.position.x;
       // float initialY = this.camera.transform.position.y;
      //  camera.transform.position = new Vector3(targetX, targetY, camera.transform.position.z);
     //   Managers.timeManager.RemoveTask(task);
     //   task =  Managers.timeManager.ScheduleTask(this.moveTime, null, a => camera.transform.SetPosition(Mathf.Lerp(initialX, targetX, a), Mathf.Lerp(initialY, targetY, a), camera.transform.position.z));
        LeanTween.move(camera.gameObject, new Vector3(targetX, targetY, camera.transform.position.z), this.moveTime );
      }
     
      
		}
	}

}
