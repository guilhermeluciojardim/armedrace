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
        if (!carDriver.isAlive){
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
