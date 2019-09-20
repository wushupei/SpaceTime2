using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : TimeGameObject
{
    public float moveSpeed;
    Transform rayPos; //射线源,用于检测地面

    private void Update()
    {
        if (!TimeManager.Instance.reverse)
            Move();
    }
    //移动 
    public void Move()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPos.position, -transform.up, 0.2f);
        if (hit.point == Vector2.zero)
            transform.rotation *= Quaternion.Euler(0, 180, 0);

        transform.Translate(transform.right.x * moveSpeed * Time.deltaTime, 0, 0, Space.World);
    }
    //碰撞
    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.transform.tag == "Wall") //碰到墙转身
            transform.rotation *= Quaternion.Euler(0, 180, 0);
        if (obj.transform.tag == "Player") //如果和主角碰撞
        {
            Vector3 playerPos = obj.transform.position; //获取主角当前坐标
            Vector3 selfPos = transform.position; //获取自身当前坐标
            //获取偏移量(自身宽度的一半)
            float OffsetX = GetComponentInChildren<BoxCollider2D>().bounds.size.x / 2f;
            //如果主角在自己上方，并且在x轴偏移量之内,判定被主角踩到
            if (playerPos.y > selfPos.y && Mathf.Abs(playerPos.x - selfPos.x) <= OffsetX)
            {
                //消除惯性
                obj.transform.GetComponent<Rigidbody2D>().Sleep();
                //让主角往上弹
                obj.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100, ForceMode2D.Impulse);
                //播放跳跃动画
                obj.transform.GetComponent<AnimationManager>().PlayJump();
            }
            else //否则就视为主角被攻击
                obj.transform.GetComponent<PlayerCharacter>().Death(); //调用主角死亡方法  
        }
    }

    public override void InitState() //初始化状态
    {
        rayPos = transform.Find("RayPos");
        base.InitState();
    }
}
