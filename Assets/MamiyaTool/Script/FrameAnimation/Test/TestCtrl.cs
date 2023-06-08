using MamiyaTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TestCtrl : MonoBehaviour
{
    [SerializeField] private float time;
    private FrameAnimator animator;
    private float timer;
    private int min;
    private int max;
    private int cur;

    private void Start() {
        animator = GetComponent<FrameAnimator>();
        min = 0;
        max = animator.animations.Count - 1;
    }
    private void Update() {
        timer += Time.deltaTime;
        if(timer >= time) {
            timer -= time;
            Next();
        }
    }
    private void Next() {
        cur = Mathf.Clamp(++cur, min, max);
        animator.Play(animator.animations[cur]);
    }
}
