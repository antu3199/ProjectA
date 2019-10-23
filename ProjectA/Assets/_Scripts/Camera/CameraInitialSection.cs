using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CameraInitialSection : MonoBehaviour {
  


  [SerializeField] private float leftRightOffset;
  [SerializeField] private float upDownOffset;
  [SerializeField] private CameraEnterSection leftWall;
  [SerializeField] private CameraEnterSection rightWall;
  [SerializeField] private CameraEnterSection upWall;
  [SerializeField] private CameraEnterSection downWall;
  [SerializeField] private float cameraOffset = 0.5f;
  public List<CameraEnterSection.RespawnPoint> playerRespawnPoints{get; set;}
  
	private BoxCollider2D boxCollider;
  private Camera camera;

  public void Initialize() {
    this.playerRespawnPoints = new List<CameraEnterSection.RespawnPoint>();
    
    if (leftWall) {
      leftWall.Initialize();
      playerRespawnPoints.Add(leftWall.GetRespawnPoint());
    }

    if (rightWall) {
      rightWall.Initialize();
      playerRespawnPoints.Add(rightWall.GetRespawnPoint());
    }

    if (upWall) {
      upWall.Initialize();
      playerRespawnPoints.Add(upWall.GetRespawnPoint());
    }

    if (downWall) {
      downWall.Initialize();
      playerRespawnPoints.Add(downWall.GetRespawnPoint());
    }
  }

  #if UNITY_EDITOR

  void Update() {
    if (!Application.isPlaying) {
      this.boxCollider = GetComponent<BoxCollider2D>();
		  camera = Camera.main;
      this.boxCollider.size = new Vector2((camera.aspect * 2f * camera.orthographicSize) + leftRightOffset, (2f * camera.orthographicSize) + upDownOffset);

      float halfSizeX = boxCollider.size.x / 2f;
      float halfSizeY = boxCollider.size.y / 2f;
      if (leftWall)
        this.leftWall.transform.localPosition = new Vector3(-halfSizeX + this.leftWall.GetComponent<BoxCollider2D>().size.x/2 + this.cameraOffset, 0, 0);
      if (rightWall)
        this.rightWall.transform.localPosition = new Vector3(halfSizeX - this.rightWall.GetComponent<BoxCollider2D>().size.x/2 - this.cameraOffset, 0, 0);
      if (upWall)
        this.upWall.transform.localPosition = new Vector3(0, halfSizeY - this.upWall.GetComponent<BoxCollider2D>().size.y/2 - this.cameraOffset, 0);
      if (downWall)
        this.downWall.transform.localPosition = new Vector3(0, -halfSizeY + this.downWall.GetComponent<BoxCollider2D>().size.y/2 + this.cameraOffset, 0);
    }
  }

  #endif
}
