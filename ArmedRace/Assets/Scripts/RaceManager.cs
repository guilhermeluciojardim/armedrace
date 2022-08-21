using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if (!isCountDownStarted){

        }
        else if (!isRaceStarted){
            getReadyText.fontSize = 70;
            InitialCountDown();
        }
        else if (!isRaceOver){ 
            CountLapsForPlayers();
        }
        else{
            //Finish the Race
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

    }
}
