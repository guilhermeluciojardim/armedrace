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
    public float maxAcceleration, maxSpeed;
    public float brakeAcceleration;

    public float maxSteerAngle;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput, steerInput;
    

    private Rigidbody carRb;

    private void Start(){
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void Update(){
        if (gameObject.CompareTag("Player")) {
            SetInputs (Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
        }
        AnimateWheels();
    }
    void LateUpdate(){
        Move();
        Steer();
        Brake();
    }
    
    public void SetInputs(float move, float turn){
            moveInput = - move;
            steerInput = turn;
    }
     void Move(){
        foreach(var wheel in wheels){
            if (wheel.axe1 == Axe1.Front){
                if (carRb.velocity.magnitude < maxSpeed){
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
        }
    }

    void Brake(){
        if (Input.GetKey(KeyCode.Space)){
            foreach (var wheel in wheels){
                wheel.wheelCollider.brakeTorque = brakeAcceleration;
            }
        }
        else{
            foreach (var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

}
