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
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput, steerInput;
    float incrementAcceleration = 15000;
    

    private Rigidbody carRb;

    private void Start(){
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void Update(){
        GetInputs();
        AnimateWheels();
    }
    void LateUpdate(){
        Move();
        Steer();
        Brake();
    }

    void GetInputs(){
        moveInput = - Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }
     void Move(){
        foreach(var wheel in wheels){
            wheel.wheelCollider.motorTorque = moveInput * incrementAcceleration * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer(){
        foreach(var wheel in wheels){
            if (wheel.axe1 == Axe1.Front){
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
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
                wheel.wheelCollider.brakeTorque = incrementAcceleration/2 * brakeAcceleration * Time.deltaTime;
            }
        }
        else{
            foreach (var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

}
