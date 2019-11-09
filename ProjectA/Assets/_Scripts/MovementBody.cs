using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyEvent {
  None,
  OnKey,
  OnKeyDown,
  OnKeyUp
};

[RequireComponent (typeof(Rigidbody))]
public class MovementBody : MonoBehaviour {

  [SerializeField] private SpriteRenderer sprite;

  [SerializeField] private float wallJumpOffset = 0.5f;
	[SerializeField] private float characterHeight = 1f;
	[SerializeField] private float characterSpeed = 1f;
  [SerializeField] private float maxSpeed = 10f;
  [SerializeField] private float jumpHeight = 100f;
  private Rigidbody2D rigidBody;

  [SerializeField] private float movementSensitivity = 1f;
  [SerializeField] private float movementDragSpeed = 1f;
  float movement = 0;
	Vector3 characterMovementVelocity = Vector3.zero;

  private int groundLayerMask;

  [SerializeField] private float wallClimbUpForce;
  [SerializeField] private Vector2 wallJumpMultiplier = Vector2.one;
  [SerializeField] private Vector2 airDrag ;
    
  [SerializeField] private Transform ground1;
  [SerializeField] private Transform ground2;


  [SerializeField] private float jumpStrength = 8.0f;
   
  [SerializeField] private float extraJumpStrengthInitial;
  [SerializeField] private float extraJumpStrengthDrag = 10f;

  [SerializeField] private float dashSpeed;
  [SerializeField] private float dashTime;
  [SerializeField] private Color neutralColor;
  [SerializeField] private Color dashingColor;

  private float extraJumpStrength;

  private bool canLeftWallClimb = true;
  private bool canRightWallClimb = true;

  private Vector2 externalVelocity = Vector3.zero;

  private Vector2 boundingBox;

  private Vector2 dashDirection;

  public bool move = true;

  public bool isGrounded;

  public bool isDashing = false;

  private bool canDash = true;

  private bool canMove = true;
  
  public JoystickState movementState; // Make sure to set this when initializing
  KeyEvent jumpButton = KeyEvent.None;
  KeyEvent dashButton = KeyEvent.None;

  List<KeyEvent> keyEvents = new List<KeyEvent>();


	void Awake() {
	  this.rigidBody = GetComponent<Rigidbody2D>();
    this.groundLayerMask = LayerMask.GetMask("Ground");
    this.boundingBox = this.sprite.bounds.size;

    keyEvents.Add(jumpButton);
    keyEvents.Add(dashButton);

	}

  public void SetMovementState(JoystickState state) {
    this.movementState = state;
  }


  public void setJump(KeyEvent evt) {
    jumpButton = evt;
  }

  public void setDashButton(KeyEvent evt) {
    dashButton = evt;
  }

  public bool CanDoJump() {
    return jumpButton == KeyEvent.OnKeyDown && this.isGrounded;
  }
  
  public bool CanDoDash() {
    return this.canDash &&  this.dashButton == KeyEvent.OnKeyDown;
  }

  public void setCanMove(bool move) {
    this.canMove = move;
  }


  private void doJump() {
        this.transform.position = new Vector3(this.transform.position.x, this.ground1.transform.position.y - this.ground1.transform.localPosition.y + 0.1f, transform.position.z);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + this.jumpStrength);
        this.isGrounded = false;
        this.externalVelocity = Vector2.zero;
        this.extraJumpStrength = this.extraJumpStrengthInitial;
  }
  private void doDash() {
      if (this.CanDoDash()) {
        this.isDashing = true;
        this.canDash = false;
        this.externalVelocity = Vector2.zero;
        this.dashDirection = Joystick.GetMovementDirection(movementState);
        this.sprite.color = this.neutralColor;
        Managers.timeManager.ScheduleTask(this.dashTime, this.EndDash, a => this.sprite.color = Color.Lerp(this.neutralColor, this.dashingColor, a));
      }
  }

  void Update() {
    if (movementState == null) {
      Debug.LogError("SET JOYSTICK STATE");
      return;
    }
    if (this.canMove) {
      doPlayerMovement();
      if (Input.GetKeyDown(KeyCode.T)) {
        this.Die();
      }
    }
  }

	private void doPlayerMovement () {
      // left/right movement.
      if (Joystick.isLeft(movementState.direction)) {
        movement = -1;
      } else if (Joystick.isRight(movementState.direction)) {
        movement = 1;
      } else {
        movement = 0;
      }

      movement = Mathf.Clamp(movement, -1, 1);

      if (Mathf.Sign(this.externalVelocity.x) != Mathf.Sign(movement) && Mathf.Abs(this.externalVelocity.x) > Mathf.Abs(movement * maxSpeed)) {
        movement = 0;
      } else if (Mathf.Sign(this.externalVelocity.x) != Mathf.Sign(movement)){ 
        this.externalVelocity.x = 0;
      }

      this.ApplyGravity();

      if (this.CanDoJump()) {
        this.doJump();
      } else if (Joystick.isRight(movementState.direction) && movementState.state == Joystick.DPadState.ON_KEY && RightSideWallHit() && this.canRightWallClimb && !this.isGrounded) {
        this.DoWallClimb(false);
        
      } else if (Joystick.isLeft(movementState.direction) && movementState.state == Joystick.DPadState.ON_KEY  && LeftSideWallHit() && this.canLeftWallClimb && !this.isGrounded) {
        this.DoWallClimb(true);
      }

      this.doJumpFloat();

     if (this.CanDoDash()) {
        this.doDash();
     }
   
      // Apply movement
      if (!this.isDashing) {
        rigidBody.velocity = externalVelocity + new Vector2(movement * maxSpeed, rigidBody.velocity.y);
      } else {
        rigidBody.velocity = externalVelocity + dashDirection * dashSpeed;
      }
	}

  private void doJumpFloat() {
    if ( this.extraJumpStrength > 0 && this.jumpButton == KeyEvent.OnKey && !this.isGrounded) {
        this.extraJumpStrength = Mathf.Max(0, this.extraJumpStrength - this.extraJumpStrengthDrag);
        this.rigidBody.AddForce(Vector2.up * this.extraJumpStrength);
    }
  }

  private void ApplyGravity() {
      if (!this.isGrounded) {
        this.externalVelocity = this.ApplyDrag(this.externalVelocity, airDrag);
      } else {
        this.canLeftWallClimb = true;
        this.canRightWallClimb = true;
        this.extraJumpStrength = 0;
        if (this.isDashing == false && !this.canDash) {
          this.canDash = true;
          this.sprite.color = this.neutralColor;
        }
      }
  }

  public void Die() {
    this.sprite.color = Color.black;
    this.rigidBody.isKinematic = true;
    this.canMove = false;
    this.rigidBody.Sleep();
    Managers.timeManager.ScheduleTask(1, Managers.controller.RestartLevel);
  }

  private void EndDash() {
    this.isDashing = false;
    this.rigidBody.velocity = new Vector2(0, rigidBody.gravityScale);
  }

  void DoWallClimb(bool left) {
    float xDir = left ? -1 : 1;
    if (rigidBody.velocity.y < 0) {
      rigidBody.AddForce(Vector3.up * this.wallClimbUpForce);
    }

    if (this.jumpButton == KeyEvent.OnKeyDown) {
      rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
      rigidBody.AddForce(new Vector3(0, wallJumpMultiplier.y, 0) * jumpHeight);
      this.canLeftWallClimb = !left;
      this.canRightWallClimb = left;
    }
  }

  private bool IsGrounded() {
    Debug.DrawLine(this.ground1.position, this.ground2.position);
    Vector3 corner = this.transform.position + new Vector3( this.boundingBox.x/2, -this.boundingBox.y/2, 0);
    RaycastHit2D hit = Physics2D.Raycast(corner, this.transform.TransformDirection(Vector3.up), this.boundingBox.y/2 - 1.0f, this.groundLayerMask);
	  return hit.collider != null;
  }

  private bool LeftSideWallHit() {
    Vector3 corner = this.transform.position + new Vector3( -this.boundingBox.x/2 - this.wallJumpOffset, -this.boundingBox.y/2, 0);
    RaycastHit2D hit = Physics2D.Raycast(corner, Vector3.up, this.boundingBox.y , this.groundLayerMask);
    Debug.DrawLine(this.transform.position, this.transform.position + this.transform.TransformDirection(Vector3.left) * this.characterHeight);
	  return hit.collider != null;
  }

  private bool RightSideWallHit() {
     
   // RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.right), this.characterHeight, this.groundLayerMask);
    Vector3 corner = this.transform.position + new Vector3( this.boundingBox.x/2 + this.wallJumpOffset, -this.boundingBox.y/2, 0);
    this.ground1.transform.position= corner;
    RaycastHit2D hit = Physics2D.Raycast(corner, this.transform.TransformDirection(Vector3.up), this.boundingBox.y, this.groundLayerMask);
    Debug.DrawLine(this.transform.position, this.transform.position + this.transform.TransformDirection(Vector3.right) * this.characterHeight);
	  return hit.collider != null;
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Bullet") {
      if (this.isDashing) {
        other.GetComponent<PooledObject>().ReturnToPool();
      } else {
        this.Die();
      }
    }
  }

  private void OnCollisionStay2D (Collision2D other) {
    if (other.rigidbody != null && other.rigidbody.isKinematic) {
       this.externalVelocity.x = other.rigidbody.velocity.x;
       this.externalVelocity.y = 0;
    }
  }

  private Vector2 ApplyDrag(Vector2 velocity, Vector2 drag) {
    float x = Mathf.Clamp01(1f - drag.x * Time.fixedDeltaTime);
    float y = Mathf.Clamp01(1f - drag.y * Time.fixedDeltaTime);
    velocity = new Vector2(velocity.x * x, velocity.y * y);
    return velocity;
  }
}
