using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    
    [SerializeField] private CarController car;

    void OnTriggerEnter(Collider coll){
        //Recover Player Full Weapons
        if (coll.gameObject.CompareTag("Player")){
            car = coll.GetComponent<CarController>();
            car.RecoverWeapons();
        }
        car.SetLaps();
    }

}
