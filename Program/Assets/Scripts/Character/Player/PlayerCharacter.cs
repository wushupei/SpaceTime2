using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    Rigidbody2D r2d; //刚体
    AnimationManager am; //动画管理类
    public Vector2 pSize; //声明主角碰撞器大小

    public float moveSpeed; //移动速度
    public float jumpHeight; //跳跃高度

    Collider2D hit; //获取射线信息
    public bool death; //是否死亡

    public AudioClip jumpAdio; //跳跃声音
    public AudioClip deathAudio; //死亡声音
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        am = GetComponent<AnimationManager>();
        pSize = GetComponent<BoxCollider2D>().bounds.size;
    }
    void Update()
    {
        IsFall();
    }
    void IsFall() //下落检测
    {
        //检测到速度在下落,播放下落动画
        am.PlayFall(r2d.velocity.y < -0.1f);
    }
    //判断是否在地面
    bool IsGround()
    {
        //获取碰撞器对角线的一半
        float diagonal = Mathf.Sqrt(pSize.x * pSize.x + pSize.y * pSize.y) / 2;
        //发射射线检测地面获取地面信息
        hit = Physics2D.OverlapCircle(transform.position, diagonal, LayerMask.GetMask("Ground"));
        if (hit) //如果成功获取
        {
            //获取地面碰撞器大小
            Vector2 gSize = hit.GetComponent<BoxCollider2D>().bounds.size;
            //获取主角和地面的Y轴距离
            float distanceY = Mathf.Abs(transform.position.y - hit.transform.position.y);
            //获取主角和地面的y轴偏移量
            float yOffset = (pSize.y + gSize.y) / 2;
            //如果y轴距离大于等于偏移量则在地面
            return distanceY + 0.02f >= yOffset;
        }
        else return false;
    }
    //移动 
    public void Run(float direction)
    {
        if (direction != 0)
        {
            //面朝方向
            transform.rotation = Quaternion.Euler(0, direction > 0 ? 0 : 180, 0);
            //移动
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        //播放动画        
        am.PlayRun(direction);
    }
    //跳跃
    public void Jump(bool key)
    {
        if (IsGround() && key) //如果在地面
        {
            r2d.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse); //起跳
            am.PlayJump();
            TimeManager.Instance.audioSource.PlayOneShot(jumpAdio);
        }
    }
    //死亡
    public void Death()
    {
        if (death == false)
        {
            death = true;
            am.PlayDead();
            GetComponent<BoxCollider2D>().enabled = false;
            TimeManager.Instance.audioSource.PlayOneShot(deathAudio);
        }
    }
    //复活
    public void Resuscitate()
    {
        death = false;
        am.PlayIdle();
    }
}
