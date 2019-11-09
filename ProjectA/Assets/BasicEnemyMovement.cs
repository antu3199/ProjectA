using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour {

  public float minWalkTime = 1f;
  public float maxWalkTime = 3f;
  public MovementBody movementBody;


  JoystickState movementState = new JoystickState();

  float nextWalkTime = 0;
  float walkCounter = 0;
  int movementDirection = 1;

  void Start() {
    movementBody.SetMovementState(movementState);
    GetNextWalkTime();
  }

  void GetNextWalkTime() {
    walkCounter = 0;
    nextWalkTime = Random.Range(minWalkTime, maxWalkTime);

    int randInt = Random.Range(0, 3);
    if (randInt == 0) {
      movementDirection = 1;
    } else if (randInt == 1) {
      movementDirection = -1;
    } else {
      movementDirection = 0;
    }
  }
	// Update is called once per frame
	void Update () {
		walkCounter += Time.deltaTime;
    if (walkCounter >= nextWalkTime) {
      GetNextWalkTime();
    }

    if (movementDirection == 1) {
      movementState.direction = Joystick.DPadDirection.RIGHT;
    } else if (movementDirection == -1) {
      movementState.direction = Joystick.DPadDirection.LEFT;
    } else {
      movementState.direction = Joystick.DPadDirection.NEUTRAL;
    }

	}
}
