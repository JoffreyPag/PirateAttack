using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    [HideInInspector] public Vector2Int GridSize;
    [HideInInspector] public int gameTime;
    [HideInInspector] public int timeTospawnEnemies;
    [HideInInspector] public int points;
    public UnityEvent<int> onPointChange;
    private bool gamePaused;
    private void Awake() {
        if(GameManager.Instance != null && GameManager.Instance != this){
            Destroy(gameObject);
        }else{
            GameManager.Instance = this;
        }
        GridSize = new Vector2Int();
        RetrieveGameSettings();
        DontDestroyOnLoad(gameObject);
    }
    private void RetrieveGameSettings(){
        GridSize.x = PlayerPrefs.GetInt("GridX", 25);
        GridSize.y = PlayerPrefs.GetInt("GridY", 25);
        gameTime = PlayerPrefs.GetInt("gameTime", 120);
        timeTospawnEnemies = PlayerPrefs.GetInt("timeTospawnEnemies", 2);
    }
    public void PauseGame(bool _pause){
        gamePaused = _pause;
        if(gamePaused){
            Time.timeScale = 0f;
        }else{
            Time.timeScale = 1f;
        }
    }
    public void StartGame(){
        points = 0;
        LoadGame();
    }
    public void LoadMenu() => StartCoroutine(LoadScene("Menu"));
    public void LoadGame() => StartCoroutine(LoadScene("Game"));
    IEnumerator LoadScene(string sceneName){
        AsyncOperation sceneOnLoading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while(!sceneOnLoading.isDone){
            yield return null;
        }
    }
    public void SaveOptionsSettings(int _gameTime, int _timeTospawnEnemies){
        this.gameTime = _gameTime;
        this.timeTospawnEnemies = _timeTospawnEnemies;
        PlayerPrefs.SetInt("gameTime", _gameTime);
        PlayerPrefs.SetInt("timeTospawnEnemies", _timeTospawnEnemies);
    }
    public void AddPoints(int value){
        points += value;
        onPointChange?.Invoke(points);
    }
}
