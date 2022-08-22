using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerAI : MonoBehaviour
{
    [SerializeField] private Transform currentTarget;
    [SerializeField] private List<Transform> targetList = new List<Transform>();
    [SerializeField] private GameObject trackPath;
    [SerializeField] private GameObject respawnEffect;
    private Transform currentTargetTransform;
    private CarController carDriver;
    private Vector3 nextTarget;
     float forwardAmount = 0f;
     float turnAmount = 0f;
     int listIndex;

     [SerializeField] private List<GameObject> otherCarsList = new List<GameObject>();

    // Start is called before the first frame update
    void Awake(){
        carDriver = GetComponent<CarController>();
        GetTrackPath();
    }

    void GetTrackPath(){
        //Count how many target exists
        int index=trackPath.transform.childCount;
        //Populate the list
        for (int i=1;i<=index;i++){
            string targetName = "Target_" + i;
            currentTargetTransform = trackPath.transform.Find(targetName);
            targetList.Add(currentTargetTransform);
        }
        //Set the first Target
        listIndex=0;
        currentTarget = targetList[listIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (carDriver.isAlive){

            // Set the next target in the trackpath
            float distToTarget = Vector3.Distance (transform.position, currentTarget.position);
            if (distToTarget < 10f){
                //Lap completed
                if (listIndex < targetList.Count -1){
                    listIndex+=1;    
                }
                else{
                    listIndex=0;
                }
                currentTarget = targetList[listIndex];
            }
            SetNextTarget(currentTarget.position);

            // Move forward to target
            Vector3 dirToMovePosition = (nextTarget - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToMovePosition);

            //check if nextTarget is in Front or back
            if (dot > 0){
                forwardAmount = -1f;
            }
            else{
                forwardAmount = 1f;
            }

            //Angle to target
            float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);
            if (angleToDir > 0){
                turnAmount = -1f;
            }
            else{
                turnAmount = 1f;
            }

            carDriver.SetInputs(forwardAmount, turnAmount);
            StartCoroutine(CheckTimeStuck(transform.position));
            //Enemy0
            //Shoot if there is enemy car in front
            //Place Mines if there is enemy car in back
            
                Vector3 dirToEnemy0 = (otherCarsList[0].transform.position - transform.position).normalized;
                float dotEnemy0 = Vector3.Dot(transform.forward, dirToEnemy0);

                if (dotEnemy0 < 0){
                    carDriver.FireMachineGun();
                }
                else{
                    carDriver.DropMine();
                }
                //Fire Missiles
                float distToEnemy0 = Vector3.Distance (transform.position, otherCarsList[0].transform.position);
                if ((distToEnemy0 > 15f) && (dotEnemy0<0)){
                    carDriver.FireMissile();
                }
            //Enemy1
            //Shoot if there is enemy car in front
            //Place Mines if there is enemy car in back
            
                Vector3 dirToEnemy1 = (otherCarsList[1].transform.position - transform.position).normalized;
                float dotEnemy1 = Vector3.Dot(transform.forward, dirToEnemy1);

                if (dotEnemy1 < 0){
                    carDriver.FireMachineGun();
                }
                else{
                    carDriver.DropMine();
                }
                //Fire Missiles
                float distToEnemy1 = Vector3.Distance (transform.position, otherCarsList[1].transform.position);
                if ((distToEnemy1 > 15f) && (dotEnemy1<0)){
                    carDriver.FireMissile();
                }      
            //Enemy2
            //Shoot if there is enemy car in front
            //Place Mines if there is enemy car in back
            
                Vector3 dirToEnemy2 = (otherCarsList[2].transform.position - transform.position).normalized;
                float dotEnemy2 = Vector3.Dot(transform.forward, dirToEnemy2);

                if (dotEnemy2 < 0){
                    carDriver.FireMachineGun();
                }
                else{
                    carDriver.DropMine();
                }
                //Fire Missiles
                float distToEnemy2 = Vector3.Distance (transform.position, otherCarsList[2].transform.position);
                if ((distToEnemy2 > 15f) && (dotEnemy2<0)){
                    carDriver.FireMissile();
                }          
            
            // Use turbo if is on right place of track
            if ((listIndex==1) || (listIndex==3) || (listIndex==12) || (listIndex==13) || (listIndex==28)) {
                carDriver.Turbo();
            }

        }
    }

    public void SetNextTarget(Vector3 nextTarget){
        this.nextTarget = nextTarget;
    }    

    IEnumerator CheckTimeStuck(Vector3 pos){
        yield return new WaitForSeconds(3);
        float distance = Vector3.Distance (transform.position,pos);
        Vector3 offset = new Vector3(0,0.5f,0);
        if (distance<5){
            transform.position = targetList[listIndex].position + offset;
            transform.rotation = carDriver.initialRot;
            transform.LookAt(targetList[listIndex+1].position);
            transform.Rotate(0,180,0);
            GameObject respawn = GameObject.Instantiate(respawnEffect, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(respawn, 1f); 
        }
    }
}
