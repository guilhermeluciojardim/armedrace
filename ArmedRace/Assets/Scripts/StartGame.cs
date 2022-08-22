using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartGame : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource carstartingAudio;

    void Start(){
        anim = GetComponent<Animator>();
        carstartingAudio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            anim.SetTrigger("Fade_Out");
            carstartingAudio.Play();
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(1);
    }
}
