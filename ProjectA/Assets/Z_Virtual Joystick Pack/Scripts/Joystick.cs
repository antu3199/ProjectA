using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickState {
  public Joystick.DPadDirection direction = Joystick.DPadDirection.NEUTRAL;
  public Joystick.DPadState state = Joystick.DPadState.NEUTRAL;

  public JoystickState() {}
  public JoystickState(JoystickState state) {
    this.direction = state.direction;
    this.state  = state.state;
  }
};

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Options")]
    [Range(0f, 2f)] public float handleLimit = 1f;

    [HideInInspector] public Vector2 inputVector = Vector2.zero;

    [Header("Components")]
    public RectTransform background;
    public RectTransform handle;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }

    Vector2 joystickPosition = Vector2.zero;
    private Camera cam = new Camera();
    public static List<Vector2> dPadDirections = new List<Vector2>();
    private bool isPressingUI = false;

    public enum DPadDirection {
        NEUTRAL = 0,
        UP = 1,
        DOWN = 2,
        LEFT = 3,
        RIGHT = 4,
        TOP_LEFT = 5,
        TOP_RIGHT = 6,
        BOTTOM_LEFT = 7,
        BOTTOM_RIGHT = 8
    };
    
    public enum DPadState {
      NEUTRAL,
      ON_KEY_DOWN,
      ON_KEY,
      ON_KEY_UP
    };

    public JoystickState joystickState = new JoystickState();


    private DPadState currentDPadState;

    void Start()
    {
      joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);

      dPadDirections = new List<Vector2>();
      float diagonalMag = 1f / Mathf.Sqrt(2);

      dPadDirections.Clear();
      Vector2 up = Vector2.up;
      Vector2 down = Vector2.down;
      Vector2 left = Vector2.left;
      Vector2 right = Vector2.right;
      Vector2 topLeft = new Vector2(-diagonalMag,diagonalMag).normalized;
      Vector2 topRight = new Vector2(diagonalMag,diagonalMag).normalized;
      Vector2 bottomLeft = new Vector2(-diagonalMag,-diagonalMag).normalized;
      Vector2 bottomRight = new Vector2(diagonalMag, -diagonalMag).normalized;
      dPadDirections.Add(up);
      dPadDirections.Add(down);
      dPadDirections.Add(left);
      dPadDirections.Add(right);
      dPadDirections.Add(topLeft);
      dPadDirections.Add(topRight);
      dPadDirections.Add(bottomLeft);
      dPadDirections.Add(bottomRight);
      this.currentDPadState = DPadState.NEUTRAL;
    }

    void Update() {
      #if UNITY_EDITOR

      Vector2 newInputVector = Vector2.zero;
      
      if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) {
        this.currentDPadState = DPadState.ON_KEY_DOWN;
      } else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {
        this.currentDPadState = DPadState.ON_KEY_UP;
      }

      if (Input.GetKey(KeyCode.W)) {
        newInputVector.y += 1;
      }
      if (Input.GetKey(KeyCode.S)) {
        newInputVector.y += -1;  
      }
      if (Input.GetKey(KeyCode.D)) {
        newInputVector.x += 1;
      }
      if (Input.GetKey(KeyCode.A)) {
        newInputVector.x -=  1;
      }

      if (!this.isPressingUI) {
        this.inputVector = newInputVector;
        this.UpdateHandle();
      }
      #endif

      joystickState.direction = GetDPadDirection();
      joystickState.state = this.currentDPadState;
    }

    void LateUpdate() {
      if (this.currentDPadState == DPadState.ON_KEY_DOWN) {
        this.currentDPadState = DPadState.ON_KEY;
      } else if (this.currentDPadState == DPadState.ON_KEY_UP) {
        this.currentDPadState = DPadState.NEUTRAL;
      }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        this.UpdateHandle();
        this.currentDPadState = DPadState.ON_KEY;
        this.isPressingUI = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        this.currentDPadState = DPadState.ON_KEY_DOWN;
        this.isPressingUI = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        this.currentDPadState = DPadState.ON_KEY_UP;
        this.isPressingUI = false;
    }

    public DPadDirection GetDPadDirection() {
      if (inputVector == Vector2.zero) return DPadDirection.NEUTRAL;
      
      DPadDirection curDPadDirection = (DPadDirection)1;
      float maxDot = Vector2.Dot(dPadDirections[0], this.inputVector);
      for (int i = 1; i < dPadDirections.Count; i++) {
          float dot = Vector2.Dot(dPadDirections[i], this.inputVector);
          if (dot > maxDot){
              curDPadDirection = (DPadDirection)i+1;
              maxDot = dot;
          }
      }
      return curDPadDirection;
    }

    public static Vector2 GetMovementDirection(JoystickState other) {
      int direction = (int)other.direction;
      if (direction == 0) {
        return Vector2.zero;
      }

      return dPadDirections[direction-1];
    }

    public static bool isNeutral (DPadDirection dir) {
      return dir == DPadDirection.NEUTRAL;
    }

    public static bool isLeft (DPadDirection direction) {
      return direction == DPadDirection.LEFT || direction == DPadDirection.TOP_LEFT || direction == DPadDirection.BOTTOM_LEFT;
    }

    public static bool isRight (DPadDirection direction) {
      return direction == DPadDirection.RIGHT || direction == DPadDirection.TOP_RIGHT || direction == DPadDirection.BOTTOM_RIGHT;
    }

    public static bool isUp (DPadDirection direction) {
      return direction == DPadDirection.UP || direction == DPadDirection.TOP_LEFT || direction == DPadDirection.TOP_RIGHT;
    }

    public static bool isDown (DPadDirection direction) {
      return direction == DPadDirection.DOWN || direction == DPadDirection.BOTTOM_LEFT || direction == DPadDirection.BOTTOM_RIGHT;
    }

    public bool Neutral() {
      return Joystick.isNeutral(this.GetDPadDirection());
    }

    public bool Left() {
        return Joystick.isLeft(this.GetDPadDirection());
    }

    public bool Right() {
        return Joystick.isRight(this.GetDPadDirection());
    }

    public bool Up() {
        return Joystick.isUp(this.GetDPadDirection());
    }

    public bool Down() {
        return Joystick.isDown(this.GetDPadDirection());
    }
    
    public bool TopLeft() {
      return this.GetDPadDirection() == DPadDirection.TOP_LEFT;
    }

    public bool TopRight() {
      return this.GetDPadDirection() == DPadDirection.TOP_RIGHT;
    }

    public bool BottomLeft() {
      return this.GetDPadDirection() == DPadDirection.BOTTOM_LEFT;
    }

    public bool BottomRight() {
      return this.GetDPadDirection() == DPadDirection.BOTTOM_RIGHT;
    }

    public bool OnKeyDown() {
      return this.currentDPadState == DPadState.ON_KEY_DOWN;
    }

    public bool OnKeyUp() {
      return this.currentDPadState == DPadState.ON_KEY_UP;
    }

    public bool OnKey() {
      return this.currentDPadState == DPadState.ON_KEY;
    }

    private void UpdateHandle() {
      handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }
}
