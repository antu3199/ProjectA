using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private List<Vector2> dPadDirections;
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
    }
    
    public enum DPadState {
      NEUTRAL,
      ON_KEY_DOWN,
      ON_KEY,
      ON_KEY_UP
    }

    private DPadState currentDPadState;

    void Start()
    {
      joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);

      this.dPadDirections = new List<Vector2>();
      float diagonalMag = 1f / Mathf.Sqrt(2);
      Vector2 up = Vector2.up;
      Vector2 down = Vector2.down;
      Vector2 left = Vector2.left;
      Vector2 right = Vector2.right;
      Vector2 topLeft = new Vector2(-diagonalMag,diagonalMag).normalized;
      Vector2 topRight = new Vector2(diagonalMag,diagonalMag).normalized;
      Vector2 bottomLeft = new Vector2(-diagonalMag,-diagonalMag).normalized;
      Vector2 bottomRight = new Vector2(diagonalMag, -diagonalMag).normalized;
      this.dPadDirections.Add(up);
      this.dPadDirections.Add(down);
      this.dPadDirections.Add(left);
      this.dPadDirections.Add(right);
      this.dPadDirections.Add(topLeft);
      this.dPadDirections.Add(topRight);
      this.dPadDirections.Add(bottomLeft);
      this.dPadDirections.Add(bottomRight);
      this.currentDPadState = DPadState.NEUTRAL;
    }
 
    void Update() {
      #if UNITY_EDITOR

      Vector2 newInputVector = Vector2.zero;
      
      if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) {
        this.currentDPadState = DPadState.ON_KEY_DOWN;
      } else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
        this.currentDPadState = DPadState.ON_KEY_UP;
      }

      if (Input.GetKey(KeyCode.UpArrow)) {
        newInputVector.y += 1;
      }
      if (Input.GetKey(KeyCode.DownArrow)) {
        newInputVector.y += -1;  
      }
      if (Input.GetKey(KeyCode.RightArrow)) {
        newInputVector.x += 1;
      }
      if (Input.GetKey(KeyCode.LeftArrow)) {
        newInputVector.x -=  1;
      }

      if (!this.isPressingUI) {
        this.inputVector = newInputVector;
        this.UpdateHandle();
      }
      #endif

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
      float maxDot = Vector2.Dot(this.dPadDirections[0], this.inputVector);
      for (int i = 1; i < this.dPadDirections.Count; i++) {
          float dot = Vector2.Dot(this.dPadDirections[i], this.inputVector);
          if (dot > maxDot){
              curDPadDirection = (DPadDirection)i+1;
              maxDot = dot;
          }
      }
      return curDPadDirection;
    }

    public Vector2 GetMovementDirection() {
      int direction = (int)this.GetDPadDirection();
      if (direction == 0) {
        return Vector2.zero;
      }

      return this.dPadDirections[direction-1];
    }

    public bool Neutral() {
      return this.GetDPadDirection() == DPadDirection.NEUTRAL;
    }

    public bool Left() {
      DPadDirection direction =  this.GetDPadDirection();
      return direction == DPadDirection.LEFT || direction == DPadDirection.TOP_LEFT || direction == DPadDirection.BOTTOM_LEFT;
    }

    public bool Right() {
      DPadDirection direction =  this.GetDPadDirection();
      return direction == DPadDirection.RIGHT || direction == DPadDirection.TOP_RIGHT || direction == DPadDirection.BOTTOM_RIGHT;
    }

    public bool Up() {
      DPadDirection direction =  this.GetDPadDirection();
      return direction == DPadDirection.UP || direction == DPadDirection.TOP_LEFT || direction == DPadDirection.TOP_RIGHT;
    }

    public bool Down() {
      DPadDirection direction =  this.GetDPadDirection();
      return direction == DPadDirection.DOWN || direction == DPadDirection.BOTTOM_LEFT || direction == DPadDirection.BOTTOM_RIGHT;
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
