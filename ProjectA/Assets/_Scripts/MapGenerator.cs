using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int randomFillPercent;

    int[,] map;

    List<Vector2> unOccupiedSpaces = new List<Vector2>();
    List<Vector2> occupiedSpaces = new List<Vector2>();

    public GameObject wallPrefab;

    public void Initialize() {
        GenerateMap();
        CreateMap();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GenerateMap();
        }
    }

    void GenerateMap() {
        map = new int[width,height];
        RandomFillMap();

        for (int i = 0; i < 5; i ++) {
            SmoothMap();
        }

        PostGenerateMap();
    }

    void PostGenerateMap() {
        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                if (map[x, y] == 0) {
                  unOccupiedSpaces.Add( new Vector2(x, y));
                } else {
                  occupiedSpaces.Add(new Vector2(x, y));
                }

            }
        }
    }

    public Vector2 getRandomUnOccupiedSpace() {
      int i = UnityEngine.Random.Range(0, unOccupiedSpaces.Count);
      return mapToPos(unOccupiedSpaces[i].x, unOccupiedSpaces[i].y);
    }


    void RandomFillMap() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                if (x == 0 || x == width-1 || y == 0 || y == height -1) {
                    map[x,y] = 1;
                }
                else {
                    map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0;
                }
            }
        }
    }

    void SmoothMap() {
        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                int neighbourWallTiles = GetSurroundingWallCount(x,y);

                if (neighbourWallTiles > 4)
                    map[x,y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x,y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX,neighbourY];
                    }
                }
                else {
                    wallCount ++;
                }
            }
        }

        return wallCount;
    }

    public Vector2 mapToPos(float x, float y) {
      return new Vector2(-width/2 + x + .5f, -height/2 + y+.5f);
    }
    
    void CreateMap() {
        if (map != null) {
            for (int x = 0; x < width; x ++) {
                for (int y = 0; y < height; y ++) {
                  if (this.map[x,y] == 1) {
                    Vector2 mapPos = mapToPos(x, y);
                    Vector3 pos = new Vector3(mapPos.x, mapPos.y, 0f);
                    GameObject wallInstance = Instantiate(wallPrefab, pos, Quaternion.identity) as GameObject;
                  }
                }
            }
        }
    }

}