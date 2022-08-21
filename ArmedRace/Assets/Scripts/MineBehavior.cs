using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
   void OnTriggerEnter(Collider coll){
        if ((coll.gameObject.CompareTag("Car")) || (coll.gameObject.CompareTag("Player"))){
            GameObject exp = GameObject.Instantiate(explosionEffect,transform.position,transform.rotation);
            GameObject.Destroy(exp,1f);
            coll.GetComponent<Rigidbody>().AddForce(coll.transform.position,ForceMode.VelocityChange);
            Destroy(gameObject);
        }
        
   }
}
