using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance {get;private set;}
    public enum MapCode{
        ocean = 0,
        land = 1
    }
    private Vector2Int gridSize;
    public int[,] map;
    private float perlinNoiseOffsetX;
    private float perlinNoiseOffsetY;
    [SerializeField] private RuleTile islandRuleTile;
    [SerializeField] private Tile seaTile, seaLimitTile;
    [SerializeField] private Tilemap oceanTileMap, oceanLimitsMap, islandTileMap;
    [SerializeField] private float perlinNoiseScale;
    [SerializeField, Range(0f, .3f)] float seaTolerance;
    private void Awake(){
        MapGenerator.Instance = this;
    }
    void Start(){
        gridSize = GameManager.Instance.GridSize;
        perlinNoiseOffsetX = UnityEngine.Random.Range(0f, 99999f);
        perlinNoiseOffsetY = UnityEngine.Random.Range(0f, 99999f);
        map = new int[gridSize.x,gridSize.y];
        GenerateMap();
        GenerateOcean();
        GenerateIsland();
        PositioningPlayer();
    }
    private void GenerateMap()
    {
        for (int x = 0; x < gridSize.x; x++){
            for (int y = 0; y < gridSize.y; y++){
                map[x, y] = PerlinNoiseValue(x, y);
            }
        }
        RemoveSingleTilesFromMap();
    }
    private void RemoveSingleTilesFromMap()
    {
        for (int x = 1; x < gridSize.x-1; x++){
            for (int y = 1; y < gridSize.y-1; y++){
                if (map[x, y] == ((int)MapCode.land)){
                    if (map[x - 1, y + 1] + map[x, y + 1] + map[x - 1, y] < 3
                     && map[x, y + 1] + map[x + 1, y + 1] + map[x + 1, y] < 3
                      && map[x - 1, y] + map[x - 1, y - 1] + map[x, y - 1] < 3
                       && map[x + 1, y] + map[x, y - 1] + map[x + 1, y - 1] < 3){
                        map[x, y] = (int)MapCode.ocean;
                    }
                }
            }
        }
    }
    private int PerlinNoiseValue(int x, int y)
    {
        float xCoord = (float)x / gridSize.x*perlinNoiseScale+perlinNoiseOffsetX;
        float yCoord = (float)y / gridSize.y*perlinNoiseScale+perlinNoiseOffsetY;;
        float value = Mathf.PerlinNoise(xCoord,yCoord);
        return Mathf.RoundToInt(value-seaTolerance);
    }
    void GenerateOcean(){
        GenerateOceanLimits();
        oceanTileMap.size = new Vector3Int(gridSize.x, gridSize.y);
        for(int x=0; x<gridSize.x; x++){
            for(int y=0; y<gridSize.y; y++){
                oceanTileMap.SetTile(new Vector3Int(x,y,0), seaTile);
            }
        }
    }
    private void GenerateOceanLimits()
    {
        for(int x=-gridSize.x/2; x<gridSize.x*1.5f; x++){
            for(int y=-gridSize.y/2; y<gridSize.y*1.5f; y++){
                oceanLimitsMap.SetTile(new Vector3Int(x,y,0), seaLimitTile);
            }
        }
    }
    void GenerateIsland(){
        islandTileMap.size = new Vector3Int(gridSize.x, gridSize.y);
        for(int x=0; x<gridSize.x; x++){
            for(int y=0; y<gridSize.y; y++){
                if(map[x,y] == ((int)MapCode.land)){
                    islandTileMap.SetTile(new Vector3Int(x,y) ,islandRuleTile);
                }
            }
        }
    }
    private void PositioningPlayer(){
        int x;
        int y;
        do{
            x = UnityEngine.Random.Range(1, gridSize.x-1);
            y = UnityEngine.Random.Range(1, gridSize.y-1);
        }while(map[x,y] == ((int)MapCode.land));
        Camera.main.transform.position = new Vector2(x,y);
        Player.Instance.transform.position = new Vector2(x,y);
    }
}
