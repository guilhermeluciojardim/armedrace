using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            anim.SetTrigger("Fade_Out");
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }
}
