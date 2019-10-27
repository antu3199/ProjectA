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
    this.getNextDirection();
  }

  public void SetHealth(int health) {
    swipeList.Clear();
    for (int i = 0; i < health; i++) {
      int randInt = Random.Range(1, 5);
      swipeList.Enqueue((SwipeDirection)randInt);
    }
  }

  public SwipeDirection getNextDirection() {
    SwipeDirection res = swipeList.Peek();
    sequence.doSequenceAnimation(res, true);
    return res;
  }

  public SwipeDirection removeNext() {
    SwipeDirection res = swipeList.Dequeue();
    this.getNextDirection();
    return res;
  }
}
