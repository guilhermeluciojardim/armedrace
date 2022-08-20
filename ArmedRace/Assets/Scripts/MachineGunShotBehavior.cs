using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunShotBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 150f;
    }

    void OnCollisionEnter(Collision coll){
        
            GameObject explosion = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(explosion, 2f);
            Destroy(gameObject);
        
    }
}
