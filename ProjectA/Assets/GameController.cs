using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	[SerializeField] private PlayerController player;
  [SerializeField] private List<CameraInitialSection> initialSections;

  public List<CameraEnterSection.RespawnPoint> respawnPoints;

  public List<EnemyGeneric> enemies = new List<EnemyGeneric>();

	// Use this for initialization
	void Awake () {
	  Managers.controller = this;
	}

  void Start() {
    this.respawnPoints = new List<CameraEnterSection.RespawnPoint>();
    foreach (var cameraSection in this.initialSections) {
      cameraSection.Initialize();
      this.respawnPoints.AddRange(cameraSection.playerRespawnPoints);
    }

    CameraEnterSection.RespawnPoint respawnPoint = this.respawnPoints[Managers.gameState.savePointIndex];
    player.transform.position = respawnPoint.playerRespawnPoint.position;
    Camera.main.transform.position = respawnPoint.cameraTransform;
  }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RestartLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
  
}
