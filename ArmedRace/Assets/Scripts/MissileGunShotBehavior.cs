using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGunShotBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    // Update is called once per frame
    void Update()
    {
        transform.position +=transform.forward * Time.deltaTime * 65f;
    }

    void OnCollisionEnter(Collision coll){
            GameObject explosion = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(explosion, 2f);
    }

}
