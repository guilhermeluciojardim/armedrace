using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [SerializeField] private GameObject machinegunShotPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform machineGunTransform;
    [SerializeField] private Transform missileGunTransform;
    
    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Player")) {
            PlayerFiring();
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
    }
    public void FireMachineGun(){
            GameObject shot = GameObject.Instantiate(machinegunShotPrefab, machineGunTransform.position, machineGunTransform.transform.rotation) as GameObject;
            GameObject.Destroy(shot, 1.5f);
    }
    public void FireMissile(){
            GameObject shot = GameObject.Instantiate(missilePrefab, missileGunTransform.position, missileGunTransform.transform.rotation) as GameObject;
            GameObject.Destroy(shot, 1.5f);
    }
}
