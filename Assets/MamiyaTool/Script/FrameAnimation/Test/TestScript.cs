using MamiyaTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private FrameAnimator animator;
    private void Start() {
        animator = GetComponent<FrameAnimator>();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(animator.IsPlaying)
                animator.Pause();
            else
                animator.Play();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            animator.Stop(true);
        if(Input.GetKeyDown(KeyCode.F1))
            animator.Stop();
    }
}
