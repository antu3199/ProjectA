using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSectionResizer : MonoBehaviour {
  [SerializeField] private List<BoxCollider2D> sections;
	private BoxCollider2D boxCollider;

  #if UNITY_EDITOR

  void Update() {
    if (!Application.isPlaying) {
      this.boxCollider = GetComponent<BoxCollider2D>();
      float halfSizeX = boxCollider.size.x / 2f;
      float halfSizeY = boxCollider.size.y / 2f;
      foreach (var section in this.sections) {
        float maxPosX = halfSizeX - section.size.x / 2f;
        float minPosX = -halfSizeX + section.size.x / 2f;
        float maxPosY = halfSizeY - section.size.y / 2f;
        float minPosY = -halfSizeY + section.size.y / 2f;
        float clampedX = Mathf.Clamp(section.transform.localPosition.x, minPosX, maxPosX);
        float clampedY = Mathf.Clamp(section.transform.localPosition.y, minPosY, maxPosY);
        section.transform.localPosition = new Vector3(clampedX, clampedY, section.transform.localPosition.z);
      }
    }
  }
  #endif
}
