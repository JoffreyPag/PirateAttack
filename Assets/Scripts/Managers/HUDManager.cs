using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsOnHUD, pointsOnPanel, timeDisplay;
    [SerializeField] private Button playagainButton, menuButton;
    [SerializeField] private GameObject EndGameScreen;
    private float time;
    void Start()
    {
        time = GameManager.Instance.gameTime;
        playagainButton.onClick.AddListener(()=>{
                GameManager.Instance.PauseGame(false);
                GameManager.Instance.StartGame();
            });
        menuButton.onClick.AddListener(()=>{
                GameManager.Instance.PauseGame(false);
                GameManager.Instance.LoadMenu();
            });  
        GameManager.Instance.onPointChange.AddListener(UpdateScoreText);      
        UpdateScoreText(GameManager.Instance.points);
        Player.Instance.onDie.AddListener(()=>{time = 0;});
    }
    private void FixedUpdate() {
        time -= Time.deltaTime;
        UpdateTimeDisplay();
        if(time <= 0){
            EndGame();
        }
    }
    private void UpdateTimeDisplay()
    {
        TimeSpan countdown = TimeSpan.FromSeconds(time);
        timeDisplay.text = string.Format("{0:D2}:{1:D2}", countdown.Minutes, countdown.Seconds);
    }
    private void EndGame()
    {
        EndGameScreen.SetActive(true);
        GameManager.Instance.PauseGame(true);
    }
    private void UpdateScoreText(int arg0)
    {
        pointsOnHUD.text = arg0.ToString();
        pointsOnPanel.text = arg0.ToString();
    }
    private void OnDisable() {
        GameManager.Instance.onPointChange.RemoveListener(UpdateScoreText);
    }
}
