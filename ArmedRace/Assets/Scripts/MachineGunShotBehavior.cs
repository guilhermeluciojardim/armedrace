using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunShotBehavior : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 150f;
    }
}
