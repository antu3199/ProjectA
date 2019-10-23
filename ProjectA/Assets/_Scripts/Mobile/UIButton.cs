using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
  
  [SerializeField] private Color neutralColor;
  [SerializeField] private Color onPressedColor;
  [SerializeField] private KeyCode editorKey;

  private Image image;
  private ButtonState state;
  private bool isPressingUI = false;

  public enum ButtonState {
    NEUTRAL,
    ON_KEY_DOWN,
    ON_KEY,
    ON_KEY_UP
  }


  void Start() {
    this.image = GetComponent<Image>();
    this.image.color = this.neutralColor;
    this.state = ButtonState.NEUTRAL;
    this.isPressingUI = false;
  }

#if UNITY_EDITOR
  void Update() {
    if (!this.isPressingUI) {
      if (Input.GetKeyDown(this.editorKey)) {
        this.state = ButtonState.ON_KEY_DOWN;
        this.image.color = this.onPressedColor;
      } else if (Input.GetKey(this.editorKey)) {
        this.state = ButtonState.ON_KEY;
      } else if (Input.GetKeyUp(this.editorKey)) {
        this.state = ButtonState.NEUTRAL;
        this.image.color = this.neutralColor;
      }
    }
  }

#endif

  void LateUpdate() {
    if (this.state == ButtonState.ON_KEY_DOWN) {
      this.state = ButtonState.ON_KEY;
    } else if (this.state == ButtonState.ON_KEY_UP) {
      this.state = ButtonState.NEUTRAL;
    }
  }

  public void OnPointerDown(PointerEventData eventData)
  { 
    this.state = ButtonState.ON_KEY_DOWN;

    this.image.color = this.onPressedColor;
    this.isPressingUI = true;
    
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    this.state = ButtonState.ON_KEY_UP;
    this.image.color = this.neutralColor;
    this.isPressingUI = false;
  }

  public bool OnKeyDown() {
    return this.state == ButtonState.ON_KEY_DOWN;
  }

  public bool OnKey() {
    return this.state == ButtonState.ON_KEY;
  }

  public bool OnKeyUp() {
    return this.state == ButtonState.ON_KEY_UP;
  }
}
