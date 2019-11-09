using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

  [SerializeField] private Joystick joystick;
  [SerializeField] private UIButton jumpButton;
  [SerializeField] private UIButton dashButton;

  [SerializeField] private SpriteRenderer sprite;

  public MovementBody movementBody;

  private Rigidbody2D rigidBody;
  private bool alive = true;
  

	void Awake() {
	  this.rigidBody = GetComponent<Rigidbody2D>();
    movementBody.movementState = joystick.joystickState;
    JoystickState state = joystick.joystickState;
    movementBody.SetMovementState(state);
	}

  void Update() {
    if (this.alive) {
      movementBody.setJump(uiButtonToKeyEvent(jumpButton));
      movementBody.setDashButton(uiButtonToKeyEvent(dashButton));
    }
  }


  KeyEvent uiButtonToKeyEvent(UIButton button) {
    if (button.OnKey()) {
      return KeyEvent.OnKey;
    } else if (button.OnKeyDown()) {
      return KeyEvent.OnKeyDown;
    } else if (button.OnKeyUp()) {
      return KeyEvent.OnKeyUp;
    } else {
      return KeyEvent.None;
    }
  }

  public void Die() {
    this.sprite.color = Color.black;
    this.rigidBody.isKinematic = true;
    this.alive = false;
    this.rigidBody.Sleep();
    Managers.timeManager.ScheduleTask(1, Managers.controller.RestartLevel);
  }

  
  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Bullet") {
      if (this.movementBody.isDashing) {
        other.GetComponent<PooledObject>().ReturnToPool();
      } else {
        this.Die();
      }
    }
  }

}
