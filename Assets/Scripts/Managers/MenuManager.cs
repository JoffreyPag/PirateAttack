using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button playButton, optionsButton, quitOptionsButton, saveOptionsChange;
    [SerializeField] Slider sessionTimeSlider, enemiesSpawnTimeSlider;
    [SerializeField] TextMeshProUGUI sessionTimeText, enemiesSpawnTimeText;
    [SerializeField] GameObject MenuPanel, OptionsPanel;

    private void Start() {
        playButton.onClick.AddListener(()=>{ GameManager.Instance.StartGame(); });
        optionsButton.onClick.AddListener(()=>{
            MenuPanel.SetActive(false);
            OptionsPanel.SetActive(true);
        });
        quitOptionsButton.onClick.AddListener(()=>{
            OptionsPanel.SetActive(false);
            MenuPanel.SetActive(true);
        });
        sessionTimeSlider.onValueChanged.AddListener(UpdateSessionTimeText);
        enemiesSpawnTimeSlider.onValueChanged.AddListener(UpdateEnemiesSpawnTimeText);
        sessionTimeSlider.value = GameManager.Instance.gameTime;
        UpdateSessionTimeText(sessionTimeSlider.value);
        enemiesSpawnTimeSlider.value = GameManager.Instance.timeTospawnEnemies;
        UpdateEnemiesSpawnTimeText(enemiesSpawnTimeSlider.value);
        saveOptionsChange.onClick.AddListener(()=>{
            GameManager.Instance.SaveOptionsSettings((int)sessionTimeSlider.value, (int)enemiesSpawnTimeSlider.value);
        });

    }
    private void UpdateSessionTimeText(float value){
        TimeSpan time = TimeSpan.FromSeconds(value);
        sessionTimeText.text = string.Format("{0:D2}:{1:D2}",time.Minutes, time.Seconds);
    }
    private void UpdateEnemiesSpawnTimeText(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        enemiesSpawnTimeText.text = string.Format("{0:D2}:{1:D2}",time.Minutes, time.Seconds);
    }
}
