﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	[SerializeField] private PlayerController player;
  [SerializeField] private List<CameraInitialSection> initialSections;

  public List<CameraEnterSection.RespawnPoint> respawnPoints;

  public List<EnemyGeneric> enemies = new List<EnemyGeneric>();

  public MapGenerator map;

  public bool spawnPlayerInFixedLocation;
  public Vector2 fixedLocation;

	// Use this for initialization
	void Awake () {
	  Managers.controller = this;
    map.Initialize();

    Vector2 playerSpawn = map.getRandomUnOccupiedSpace();
    if (spawnPlayerInFixedLocation) {
      playerSpawn = fixedLocation;
    }
    player.transform.position = new Vector3(playerSpawn.x, playerSpawn.y, player.transform.position.z);
	}

  void Start() {
    /* 
    this.respawnPoints = new List<CameraEnterSection.RespawnPoint>();
    foreach (var cameraSection in this.initialSections) {
      cameraSection.Initialize();
      this.respawnPoints.AddRange(cameraSection.playerRespawnPoints);
    }

    CameraEnterSection.RespawnPoint respawnPoint = this.respawnPoints[Managers.gameState.savePointIndex];
    player.transform.position = respawnPoint.playerRespawnPoint.position;
    Camera.main.transform.position = respawnPoint.cameraTransform;
    */
  }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RestartLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
  
}
