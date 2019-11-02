using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSequence : MonoBehaviour {

  public List<Image> arrows;
  public List<Sprite> arrowImages;
  public RectTransform arrowImagesTransform;

  float curOffset = 0;
  public float offsetAdd = 1;
  public GameObject arrowPrefab;

  public float slideSpeed = 1f;

  public bool isAnimating = false;

  public float offsetStart = -1;
  private float targetOffset = 0;

  void Start() {
    curOffset = offsetStart;
    arrowImagesTransform.anchoredPosition = new Vector2(curOffset, arrowImagesTransform.anchoredPosition.y);
  }

  void Update() {
    if (isAnimating) {
      if (curOffset > targetOffset) {
        curOffset -= Time.deltaTime * slideSpeed;
        arrowImagesTransform.anchoredPosition = new Vector2(curOffset, arrowImagesTransform.anchoredPosition.y);
      } else {
        curOffset = targetOffset;
        isAnimating = false;
        arrowImagesTransform.anchoredPosition = new Vector2(curOffset, arrowImagesTransform.anchoredPosition.y);
      }
    }
  }

  public void doSequenceAnimation(SwipeDirection dir, bool anim) {
    if (isAnimating) {
      curOffset = targetOffset;
      arrowImagesTransform.anchoredPosition = new Vector2(curOffset, arrowImagesTransform.anchoredPosition.y);
    }

    targetOffset = curOffset - offsetAdd;
    isAnimating = true;
  }

  public void setArrow(int i, SwipeDirection dir) {
    arrows[i].sprite = arrowImages[(int)dir - 1];
  }

  public void addArrowToSequence(SwipeDirection dir) {
    GameObject arrow = Instantiate(arrowPrefab) as GameObject;
    arrow.transform.parent = arrowImagesTransform;
    RectTransform rect = arrow.GetComponent<RectTransform>();
    rect.anchoredPosition = new Vector2(curOffset, 0);
    curOffset += offsetAdd;

    Image arrowImage = arrow.GetComponent<Image>();
    arrowImage.sprite = arrowImages[(int)dir - 1];
    arrows.Add(arrowImage);
  }

  IEnumerator sequenceAnimation() {
    float targetOffset = curOffset + offsetAdd;
    while (curOffset < targetOffset ) {
      curOffset -= Time.deltaTime * slideSpeed;
      arrowImagesTransform.anchoredPosition = new Vector2(curOffset, arrowImagesTransform.anchoredPosition.y);
      yield return null;
    }
    arrowImagesTransform.anchoredPosition = new Vector2(curOffset, arrowImagesTransform.anchoredPosition.y);
  }
}
