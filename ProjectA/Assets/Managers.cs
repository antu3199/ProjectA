using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour  {


	public static TimeManager timeManager {get; private set;}
  public static GameStateManager gameState{get; private set;}
	public static GameController controller = null;
  
	private static List<IGameManager> _startSequence;

	public string firstSceneName;

	void Awake(){
    if (Managers._startSequence != null) {
      Destroy(this.gameObject);
      return;
    }

		_startSequence = new List<IGameManager> ();
		timeManager = GetComponent<TimeManager>();
    gameState = GetComponent<GameStateManager>();

		_startSequence.Add (timeManager);
    _startSequence.Add(gameState);

		foreach (IGameManager manager in _startSequence) {
			manager.Initialize();
		}

    if (firstSceneName != string.Empty) {
		  SceneManager.LoadScene (firstSceneName);
    }

		DontDestroyOnLoad (this);
	}
}
