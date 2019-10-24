using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SwipeDirection {
	NONE,
	UP,
	DOWN,
	LEFT,
	RIGHT
};

public class SwipeController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Vector2 startPoint {get; set;}
	SwipeDirection swipeDir = SwipeDirection.NONE;
	public float minimumSwipe = 1f;

    public void OnEndDrag(PointerEventData eventData)
    {
       
		
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = eventData.position;
		Debug.Log("PointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
 		Vector2 endPoint = eventData.position;
		Vector2 dir = endPoint - startPoint;
		float absX = Mathf.Abs(dir.x);
		float absY = Mathf.Abs(dir.y);
		float distance = Vector2.Distance(dir, Vector3.zero);
		Debug.Log(distance);
		if (distance >= minimumSwipe) {
			if (absX >= absY) {
				// left/right
				swipeDir = dir.x >= 0 ? SwipeDirection.RIGHT : SwipeDirection.LEFT;
			} else {
				swipeDir = dir.y >= 0 ? SwipeDirection.UP : SwipeDirection.DOWN;

			}
		} else {
			swipeDir = SwipeDirection.NONE;
		}
		Debug.Log("Swipe Direction: " + swipeDir);

    }
}
