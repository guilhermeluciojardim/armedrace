using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerAI : MonoBehaviour
{
    [SerializeField] private Transform targetPositionTransform;
    private CarController carDriver;
    private Vector3 nextTarget;
     float forwardAmount = 0f;
     float turnAmount = 0f;

    // Start is called before the first frame update
    void Awake(){
        carDriver = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        SetNextTarget(targetPositionTransform.position);
       
        // Move forward to target
         Vector3 dirToMovePosition = (nextTarget - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToMovePosition);
        //nextTarget is in Front or back
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
    }

    public void SetNextTarget(Vector3 nextTarget){
        this.nextTarget = nextTarget;
    }    
}
