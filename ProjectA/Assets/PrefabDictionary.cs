

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabKVP {
    public GameObject sprite;
    public string key;
}

public class PrefabDictionary : Singleton<PrefabDictionary> {
    public List<PrefabKVP> prefabs;

    public GameObject findPrefab(string key) {
        for (int i = 0; i < prefabs.Count; i++) {
            if (prefabs[i].key == key) {
                return prefabs[i].sprite;
            }
        }

        return null;
    }

    public GameObject InstPrefab(string key) {
      return InstPrefab(key, Vector3.zero);
    }  

    public GameObject InstPrefab(string key, Vector3 pos) {
      GameObject obj = findPrefab(key);
      GameObject inst = Instantiate(obj, pos, Quaternion.identity) as GameObject;
      return inst;
    }
}