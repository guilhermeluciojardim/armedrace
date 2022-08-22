using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private CarController player;
    [SerializeField] private CarController Enemy1;
    [SerializeField] private CarControllerAI AIEnemy1;
    [SerializeField] private CarController Enemy2;
    [SerializeField] private CarControllerAI AIEnemy2;
    [SerializeField] private CarController Enemy3;
    [SerializeField] private CarControllerAI AIEnemy3;

    [SerializeField] private TextMeshProUGUI getReadyText;
    [SerializeField] private Button restartButton;

    private bool isRaceStarted, isRaceOver, isCountDownStarted;
    private float secondsForInitialCountDown;

    void Awake(){
        TurnOffControllers();
        isCountDownStarted=false;
        isRaceStarted=false;
        isRaceOver = false;
        secondsForInitialCountDown = 6f;
    }

    void Start(){
        StartCoroutine(WaitToStartCountDown());
    }

    void TurnOffControllers(){
        player.enabled = false;
        Enemy1.enabled = false;
        Enemy2.enabled = false;
        Enemy3.enabled = false;
        AIEnemy1.enabled = false;
        AIEnemy2.enabled = false;
        AIEnemy3.enabled = false;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            PauseGame();
        }
        if (!isCountDownStarted){

        }
        else if (!isRaceStarted){
            getReadyText.fontSize = 70;
            InitialCountDown();
        }
        else if (!isRaceOver){ 
            CountLapsForPlayers();
        }
    }
void PauseGame(){
    if (Time.timeScale==1){
        Time.timeScale=0;
        getReadyText.text = "GAME PAUSED";
        getReadyText.gameObject.SetActive(true);
    }
    else{
        Time.timeScale=1;
        getReadyText.gameObject.SetActive(false);
    }
}

    IEnumerator WaitToStartCountDown(){
        yield return new WaitForSeconds(3);
        isCountDownStarted=true;
    }

    void InitialCountDown(){
        int sec = (int) secondsForInitialCountDown;
        getReadyText.text = sec.ToString();
        if (sec>0) 
        {
            secondsForInitialCountDown -= Time.deltaTime;
        }
        else{
            isRaceStarted=true;
            getReadyText.gameObject.SetActive(false);
            player.enabled = true;
            Enemy1.enabled = true;
            Enemy2.enabled = true;
            Enemy3.enabled = true;
            AIEnemy1.enabled = true;
            AIEnemy2.enabled = true;
            AIEnemy3.enabled = true;
        }
    }
    void CountLapsForPlayers(){
        if (player.GetLaps()==3){
            getReadyText.text = "YOU WIN!!!";
            PrepareRestart();
        }
        else if (Enemy1.GetLaps()==3){
            getReadyText.text = "Blue Player Wins!";
            PrepareRestart();
        }
        else if (Enemy2.GetLaps()==3){
            getReadyText.text = "Red Player Wins!";
            PrepareRestart();
        }
        else if (Enemy2.GetLaps()==3){
            getReadyText.text = "Yellow Player Wins!";
            PrepareRestart();
        }
    }

    void PrepareRestart(){
            getReadyText.gameObject.SetActive(true);
            TurnOffControllers();
            restartButton.gameObject.SetActive(true);
    }
    public void RestartRace(){
        SceneManager.LoadScene(1);
    }
}
