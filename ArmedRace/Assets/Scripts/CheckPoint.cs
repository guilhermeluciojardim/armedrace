using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    
    [SerializeField] private CarController car;

    void OnTriggerEnter(Collider coll){
        //Recover Full Weapons and count laps
        if ((coll.gameObject.CompareTag("Player")) || (coll.gameObject.CompareTag("Car"))){
            car = coll.GetComponent<CarController>();
            car.RecoverWeapons();
            car.SetLaps(1);
        }
            
    }
}


