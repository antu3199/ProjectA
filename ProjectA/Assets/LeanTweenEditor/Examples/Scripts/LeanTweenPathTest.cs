using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenPathTest : MonoBehaviour {

  [SerializeField] private LeanTweenPath tweenPath;
  [SerializeField] private SpriteRenderer moveObject;

	// Use this for initialization
	void Start () {
		this.DoPath();
	}

  void DoPath() {
    LeanTween.moveSpline(moveObject.gameObject, tweenPath.vec3, 2.5f).setOrientToPath2d(true).setDelay(1.0f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate(this.onUpdate).setOnComplete(this.onCompletePath);
  }

  void onUpdate(float a) {
    Debug.Log(tweenPath.getPoint(a));
  }

  void onCompletePath() {
   Debug.Log(tweenPath.getPoint(1));
   Debug.Log("Completed!");
  }
}
