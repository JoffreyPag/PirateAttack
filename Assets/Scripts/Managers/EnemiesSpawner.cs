using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    private int timeToSpawn;
    private float timeToSpawnCount;
    bool canGenerate;
    [SerializeField] List<GameObject> Enemies;
    void Start()
    {
        canGenerate = true;
        timeToSpawn = GameManager.Instance.timeTospawnEnemies;
        timeToSpawnCount = timeToSpawn;
        Player.Instance.onDie.AddListener(()=>{
                canGenerate = false;
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject enemy in enemies){
                    enemy.GetComponent<IDamageable>().Die();
                }
            });
    }
    void FixedUpdate()
    {
        if(canGenerate){
            if(timeToSpawnCount <= 0){
                SpawnEnemie();
                timeToSpawnCount = timeToSpawn;
            }else{
                timeToSpawnCount -= Time.deltaTime;
            }
        }
    }
    private void SpawnEnemie(){
        GameObject enemie = ChooseEnemie();
        Vector2 spawnPosition = GetSpawnPosition();
        Instantiate(enemie, spawnPosition, Quaternion.identity);
    }
    private GameObject ChooseEnemie()
    {
        int position = UnityEngine.Random.Range(0, Enemies.Count);
        return Enemies[position];
    }
    private Vector2 GetSpawnPosition(){   
        int[,] map = MapGenerator.Instance.map;
        Vector2Int gridSize = GameManager.Instance.GridSize;
        int x;
        int y;
        do{
            x = UnityEngine.Random.Range(1, gridSize.x-1);
            y = UnityEngine.Random.Range(1, gridSize.y-1);
        }while(map[x,y] == ((int)MapGenerator.MapCode.land));
        return new Vector2(x,y);
    }
}
