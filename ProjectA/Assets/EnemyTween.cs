using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTween : MonoBehaviour {
  [SerializeField] private LeanTweenPath tweenPath;
  [SerializeField] private Transform moveObject;

	// Use this for initialization
	void Start () {
		this.DoPath();
	}

  void DoPath() {
    LeanTween.moveSpline(moveObject.gameObject, tweenPath.vec3, 2.5f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate(this.onUpdate).setOnComplete(this.onCompletePath).setLoopClamp();
  }
	// Update is called once per frame
	void onUpdate(float a) {
  }

  void onCompletePath() {

  }
}
