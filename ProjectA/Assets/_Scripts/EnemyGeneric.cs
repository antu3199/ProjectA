using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(EnemyHealth))]
public class EnemyGeneric : MonoBehaviour {

  public EnemyHealth enemyHealth {get; set;}
  public SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		Managers.controller.enemies.Add(this);
    this.enemyHealth = GetComponent<EnemyHealth>();
	}
	
	// Update is called once per frame
	void Update () {
	}

  public List<Vector2> spriteCorners() {
    Vector3 min = rend.bounds.min;
    Vector3 max = rend.bounds.max;

    Vector2 BL = min;
    Vector2 TL = new Vector2(min.x, max.y);
    Vector2 TR = max;
    Vector2 BR = new Vector2(max.x, min.y);

    List<Vector2> list = new List<Vector2>();
    list.Add(BL);
    list.Add(TL);
    list.Add(TR);
    list.Add(BR);

    return list;
  }
}
