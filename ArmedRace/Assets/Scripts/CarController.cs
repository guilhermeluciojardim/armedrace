using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarController : MonoBehaviour
{
    public enum Axe1{
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel{
        public GameObject sheelModel;
        public WheelCollider wheelCollider;

        public Axe1 axe1;
    }

    public Quaternion initialRot;
    public float maxAcceleration, maxSpeed;
    public float brakeAcceleration;

    public float maxSteerAngle;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput, steerInput;

    private float turboTank, maxTurboTank;
    public GameObject turboEffect;
    
    private int mines, maxMines;
    public GameObject minePrefab;

    public GameObject minePlacer;

    private Rigidbody carRb;

    public GameObject steerEffect;
    public GameObject respawnEffect;

    [SerializeField] private GameObject trackPath;
    [SerializeField] private List<Transform> targetList = new List<Transform>();

    private Transform nearestRespawnPoint;

    private void Start(){
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        initialRot = transform.rotation;
        turboTank = 3f; maxTurboTank=3f;
        mines= 3; maxMines = 3;
        GetTrackPath();
    }

    void GetTrackPath(){
        //Count how many target exists
        int index=trackPath.transform.childCount;
        //Populate the list
        for (int i=1;i<=index;i++){
            string targetName = "Target_" + i;
            targetList.Add(trackPath.transform.Find(targetName));
        }
    }

    void Update(){
        if (gameObject.CompareTag("Player")) {
            SetInputs (Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
        }
        AnimateWheels();
    }
    void LateUpdate(){
        if (gameObject.CompareTag("Player")) {
            HandBrake();
            Turbo();
            DropMine();
            RespawnPlayer();
        }
        Move();
        Steer();
    }
    
    public void SetInputs(float move, float turn){
            moveInput = - move;
            steerInput = turn;
    }
     void Move(){
        foreach(var wheel in wheels){
            if (wheel.axe1 == Axe1.Front){
                if (carRb.velocity.magnitude < maxSpeed) {
                    wheel.wheelCollider.motorTorque = moveInput * maxAcceleration;
                }
                else{
                    wheel.wheelCollider.motorTorque = maxAcceleration;
                }
                
            }
        }
    }

    void Steer(){
        foreach(var wheel in wheels){
            if (wheel.axe1 == Axe1.Front){
                var _steerAngle = steerInput * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.8f);
            }
        }
    }

    void AnimateWheels(){
        foreach(var wheel in wheels){
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.sheelModel.transform.position = pos;
            wheel.sheelModel.transform.rotation = rot;
            var currentSteerAngle = steerInput * maxSteerAngle;
            if (((carRb.velocity.magnitude > 5) && (currentSteerAngle > 15)) || ((carRb.velocity.magnitude > 5) && (currentSteerAngle < -15))){
                GameObject steer = GameObject.Instantiate(steerEffect, wheel.sheelModel.transform.position, wheel.sheelModel.transform.rotation) as GameObject;
                GameObject.Destroy(steer, 0.1f); 
            }
        }
    }

    void HandBrake(){
        if (Input.GetKey(KeyCode.Space)){
            foreach (var wheel in wheels){
                if (wheel.axe1 == Axe1.Rear){
                    wheel.wheelCollider.brakeTorque = brakeAcceleration;
                    GameObject steer = GameObject.Instantiate(steerEffect, wheel.sheelModel.transform.position, wheel.sheelModel.transform.rotation) as GameObject;
                    GameObject.Destroy(steer, 0.1f);
                }
                else{
                    wheel.wheelCollider.brakeTorque = brakeAcceleration * 5000f;
                }
            }
        }
        else{
            foreach (var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void Turbo(){
        if ((Input.GetKey(KeyCode.LeftShift)) && (turboTank>0)){
            carRb.AddForce(-transform.forward * 40000f);
            turboTank -= Time.deltaTime;
            GameObject turbo = GameObject.Instantiate(turboEffect, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(turbo, 1f);            
        }
    }

    void DropMine(){
        if ((Input.GetKeyDown(KeyCode.Tab)) && (mines>0)){
            Vector3 offset = new Vector3(5,0,0);
            GameObject mine = GameObject.Instantiate(minePrefab, minePlacer.transform.position, minePlacer.transform.rotation) as GameObject;
            mines -=1;
        }
    }
    void RespawnPlayer(){
        if (Input.GetKeyDown(KeyCode.R)){
            float minDist = 1000f;
            for (int i=1;i<targetList.Count;i++){
                float dist = Vector3.Distance(transform.position,targetList[i].position);
                if (dist < minDist){
                    minDist = dist;
                    nearestRespawnPoint = targetList[i].transform;
                }
            }
            Vector3 offset = new Vector3(0,0.4f,0);
            transform.position = nearestRespawnPoint.position + offset;
            transform.rotation = initialRot;
            transform.Rotate(0,180,0);
            GameObject respawn = GameObject.Instantiate(respawnEffect, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(respawn, 1f); 
        }
    }

}
