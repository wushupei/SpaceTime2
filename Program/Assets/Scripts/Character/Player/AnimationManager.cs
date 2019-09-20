using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator at;
    void Start()
    {
        at = GetComponentInChildren<Animator>();
    }
    //奔跑动画
    public void PlayRun(float direction)
    {
        at.SetBool("Run", direction != 0);
    }
    //跳跃动画
    public void PlayJump()
    {
        at.SetTrigger("Jump");
    }
    //落下动画
    public void PlayFall(bool play)
    {
        at.SetBool("Fall", play);
    }
    //死亡动画
    public void PlayDead()
    {
        at.SetTrigger("Dead");
    }
    //待机动画
    public void PlayIdle()
    {
        at.SetTrigger("Idle");
    }
}
