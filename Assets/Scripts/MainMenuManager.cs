using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        animator.SetBool("Start", true);
    }

    public void StartGame() // set by animation
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
