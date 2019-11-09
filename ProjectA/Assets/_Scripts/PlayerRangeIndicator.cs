using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SwipeDirection
{
  NONE = 0,
  UP = 1,
  DOWN = 2,
  LEFT = 3,
  RIGHT = 4
};


public class PlayerRangeIndicator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public SpriteRenderer rangeSprite;
	public float range = 1f;
	Vector3 ogSize;

  public List<EnemyGeneric> enemiesInRange {get; set;}

  public Vector2 startPoint { get; set; }
  SwipeDirection swipeDir = SwipeDirection.NONE;
  public float minimumSwipe = 1f;

	void Start () {
		ogSize = rangeSprite.bounds.size;
    this.enemiesInRange = new List<EnemyGeneric>();
	}
	
	// Update is called once per frame
	void Update () {
		Bounds bounds = rangeSprite.bounds;
		rangeSprite.transform.localScale = new Vector3(range / ogSize.x, range / ogSize.y, rangeSprite.transform.localScale.z);

    enemiesInRange.Clear();

		foreach (EnemyGeneric enemy in Managers.controller.enemies) {
			if (isInRange(enemy.spriteCorners())) {
				enemiesInRange.Add(enemy);
			}
		}

    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      this.OnSwipeDirection(SwipeDirection.LEFT);
    }
    
    if (Input.GetKeyDown(KeyCode.RightArrow)) {
      this.OnSwipeDirection(SwipeDirection.RIGHT);
    }
    
    if (Input.GetKeyDown(KeyCode.UpArrow)) {
      this.OnSwipeDirection(SwipeDirection.UP);
    } 
    
    if (Input.GetKeyDown(KeyCode.DownArrow)) {
      this.OnSwipeDirection(SwipeDirection.DOWN);
    }

	}
	
	public bool isInRange(Vector2 pos) {
		Vector2 rangePos = new Vector2(rangeSprite.transform.position.x, rangeSprite.transform.position.y);
		return Vector2.Distance(rangeSprite.transform.position, pos) <= range / 2; 
	}

	public bool isInRange(Vector3 pos) {
		Vector2 pos2 = new Vector2(pos.x, pos.y);
		return isInRange(pos2); 
	}

  public bool isInRange(List<Vector2> posList) {
    foreach (Vector2 pos in posList) {
      if (isInRange(pos)) {
        Debug.DrawLine(transform.position, pos, Color.blue, 1f);
        return true;
      }
    }
    return false;
  }


  public void OnPointerDown(PointerEventData eventData)
  {
    startPoint = eventData.position;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    Vector2 endPoint = eventData.position;
    Vector2 dir = endPoint - startPoint;
    float absX = Mathf.Abs(dir.x);
    float absY = Mathf.Abs(dir.y);
    float distance = Vector2.Distance(dir, Vector3.zero);
    Debug.Log(distance);
    if (distance >= minimumSwipe)
    {
      if (absX >= absY)
      {
        // left/right
        swipeDir = dir.x >= 0 ? SwipeDirection.RIGHT : SwipeDirection.LEFT;
      }
      else
      {
        swipeDir = dir.y >= 0 ? SwipeDirection.UP : SwipeDirection.DOWN;
      }

      this.OnSwipeDirection(swipeDir);
    }
    else
    {
      swipeDir = SwipeDirection.NONE;
    }
    Debug.Log("Swipe Direction: " + swipeDir);

  }

  public void OnSwipeDirection(SwipeDirection dir) {
    foreach (EnemyGeneric enemy in this.enemiesInRange) {
      if (enemy.enemyHealth.getNextDirection() == dir) {
        enemy.enemyHealth.removeNext();
        break;
      }
    }
  }


}

