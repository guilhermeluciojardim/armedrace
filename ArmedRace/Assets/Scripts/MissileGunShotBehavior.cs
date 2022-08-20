using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGunShotBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
   
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 65f;
    }

    void OnCollisionEnter(Collision coll){
            
            GameObject explosion = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(explosion, 2f);
            
            float radius = 100.0F;
            float power = 100000.0F;
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders){
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0f);

            }
            Destroy(gameObject);
            
    }

}
