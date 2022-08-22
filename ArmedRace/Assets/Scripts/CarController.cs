using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
    private int missiles, maxMissiles;
    public GameObject minePrefab;

    public GameObject minePlacer;

    private Rigidbody carRb;

    public GameObject steerEffect;
    public GameObject respawnEffect;
    public GameObject dieEffect;

    private int lapsByPlayer;

    [SerializeField] private GameObject trackPath;
    [SerializeField] private List<Transform> targetList = new List<Transform>();

    private Transform nearestRespawnPoint;

    [SerializeField] private List<Image> minesImageList = new List<Image>();
    
    [SerializeField] private List<Image> missilesImageList = new List<Image>();

     [SerializeField] private GameObject machinegunShotPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform machineGunTransform;
    [SerializeField] private Transform missileGunTransform;
    [SerializeField] private Scrollbar turboScrollBar;

    [SerializeField] private Camera topViewCamera;
    [SerializeField] private Camera backViewCamera;

    public float health,maxHealth;
    [SerializeField] private Scrollbar healthScrollBar;
    public bool isAlive,isMineAvailable;

    [SerializeField] private AudioSource engineAudio;

    private void Start(){
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        initialRot = transform.rotation;
        turboTank = 3f; maxTurboTank = 3f;
        mines= 3; maxMines = 3;
        missiles = 3; maxMissiles = 3;
        lapsByPlayer=0;
        health = 100f; maxHealth=100f;
        isAlive=true; isMineAvailable=true;
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
        if (isAlive){
            if (gameObject.CompareTag("Player")) {
                HandBrake();
                RespawnPlayer();
                PlayerFiring();
                ChangeCamera();
            }
            Move();
            Steer();
            CheckHealth();
        }   
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

    public void Turbo(){
        if (turboTank>0){
            carRb.AddForce(-transform.forward * 40000f);
            turboTank -= Time.deltaTime;
            if (gameObject.CompareTag("Player")){
                turboScrollBar.size -= Time.deltaTime/maxTurboTank;
            }
            GameObject turbo = GameObject.Instantiate(turboEffect, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(turbo, 1f);
        }
                        
        
    }

    public void DropMine(){
        if ((mines>0) && (isMineAvailable)){
            Vector3 offset = new Vector3(5,0,0);
            GameObject mine = GameObject.Instantiate(minePrefab, minePlacer.transform.position, minePlacer.transform.rotation) as GameObject;
            if (gameObject.CompareTag("Player")){
                minesImageList[mines-1].gameObject.SetActive(false);
            }
            mines -=1;
            isMineAvailable=false;
            StartCoroutine(WaitForNextMine());
        }
            
    }
    IEnumerator WaitForNextMine(){
        yield return new WaitForSeconds(3);
        isMineAvailable=true;
        
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
    
    void PlayerFiring(){
        if (Input.GetKeyDown(KeyCode.LeftControl)){
            FireMachineGun();
        }
        else if (Input.GetKey(KeyCode.LeftControl)){
            FireMachineGun();
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt)){
            FireMissile();
        }
        if ((Input.GetKeyDown(KeyCode.Tab)) && (mines>0)){
            DropMine();
        }
        if ((Input.GetKey(KeyCode.LeftShift)) && (turboTank>0)){
            Turbo();
        }
    }
    public void FireMachineGun(){
            GameObject shot = GameObject.Instantiate(machinegunShotPrefab, machineGunTransform.position,machineGunTransform.rotation) as GameObject;
            GameObject.Destroy(shot, 1.5f);
    }
    public void FireMissile(){
        if (missiles > 0){
            GameObject shot = GameObject.Instantiate(missilePrefab, missileGunTransform.position, missileGunTransform.transform.rotation) as GameObject;
            GameObject.Destroy(shot, 1.5f);
            if (gameObject.CompareTag("Player")){
                missilesImageList[missiles-1].gameObject.SetActive(false);
            }
            missiles -=1;
        }
    }

    public void RecoverWeapons(){
        mines = maxMines;
        missiles = maxMissiles;
        turboTank = maxTurboTank;
        if (gameObject.CompareTag("Player")){
            for (int i=0;i<minesImageList.Count;i++){
                minesImageList[i].gameObject.SetActive(true);
            }
            for (int i=0;i<missilesImageList.Count;i++){
                missilesImageList[i].gameObject.SetActive(true);
            }
            turboScrollBar.size = 1f;
        }
    }

    void ChangeCamera(){
        if (Input.GetKeyDown(KeyCode.C)){
            if (topViewCamera.gameObject.activeSelf){
                backViewCamera.gameObject.SetActive(true);
                topViewCamera.gameObject.SetActive(false);
            }
            else{
                topViewCamera.gameObject.SetActive(true);
                backViewCamera.gameObject.SetActive(false);
            }
        }
    }

    void UpdateHealthBar(){
                healthScrollBar.size = health/maxHealth;
    }

    void OnCollisionEnter(Collision coll){
        if (coll.gameObject.CompareTag("Bullet")){
            health -= 0.20f;
            UpdateHealthBar(); 
        }
        else if (coll.gameObject.CompareTag("Missile")){
            health -= 30f;
            UpdateHealthBar();  
        }
    }
    void OnTriggerEnter(Collider coll){
        if (coll.gameObject.CompareTag("Mine")){
            health -= 10f;
            UpdateHealthBar();
        }
    }

    void CheckHealth(){
        if ((health<0) && (isAlive)){
            isAlive = false;
            GameObject die = GameObject.Instantiate(dieEffect, transform.position, transform.rotation) as GameObject;
        }
    }

    public void SetLaps(int num){
        lapsByPlayer +=num;
    }
    public int GetLaps(){
        return lapsByPlayer;
    }

    public void SetMines(int num){
        mines = num;
    }

    public int GetMines(){
        return mines;
    }

    public void SetMissiles(int num){
        missiles = num;
    }

    public int GetMissiles(){
        return missiles;
    }

    public void SetHealth(float num, bool add){
        if (add){
            health += num;
        }
        else{
            health -=num;
        }
        
    }
    public float GetHealth(){
        return health;
    }



}
