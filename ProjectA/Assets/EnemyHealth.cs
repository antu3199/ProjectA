using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(ArrowSequence))]
public class EnemyHealth : MonoBehaviour {

	public int maxHealth = 10;
  public Queue<SwipeDirection> swipeList = new Queue<SwipeDirection>();

  public ArrowSequence sequence;


  void Start() {
    this.SetHealth(maxHealth);
  }

  public void SetHealth(int health) {
    swipeList.Clear();
    for (int i = 0; i < health; i++) {
      int randInt = Random.Range(1, 5);
      SwipeDirection dir = (SwipeDirection) randInt;
      swipeList.Enqueue(dir);
      sequence.addArrowToSequence(dir);
    }
  }
  
  public int getCurHealth() {
    return swipeList.Count;
  }
  

  public SwipeDirection getNextDirection() {
    if (this.getCurHealth() <= 0) {
      return SwipeDirection.NONE;
    }

    SwipeDirection res = swipeList.Peek();
    return res;
  }

  public SwipeDirection removeNext() {
    if (this.getCurHealth() <= 0) {
      return SwipeDirection.NONE;
    }
    
    SwipeDirection res = swipeList.Dequeue();
    sequence.doSequenceAnimation(res, true);
    this.getNextDirection();
    return res;
  }
}
