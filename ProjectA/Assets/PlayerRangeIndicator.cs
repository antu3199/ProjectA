using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeIndicator : MonoBehaviour {

	public SpriteRenderer rangeSprite;
	public float range = 1f;
	Vector3 ogSize;
	void Start () {
		Debug.Log("OGSize: " + rangeSprite.bounds.size);
		ogSize = rangeSprite.bounds.size;
		
	}
	
	// Update is called once per frame
	void Update () {
		Bounds bounds = rangeSprite.bounds;
		rangeSprite.transform.localScale = new Vector3(range / ogSize.x, range / ogSize.y, rangeSprite.transform.localScale.z);
		Debug.Log(rangeSprite.bounds.size);

		foreach (EnemyGeneric enemy in Managers.controller.enemies) {
			if (isInRange(enemy.transform.position)) {
				Debug.Log("Enemy in range!");
				Debug.DrawLine(rangeSprite.transform.position,enemy.transform.position, Color.blue, 1f);
			}
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
}
