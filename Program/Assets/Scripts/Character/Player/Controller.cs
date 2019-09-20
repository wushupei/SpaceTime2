using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Controller : TimeGameObject
{
    public float height; //死亡高度
    public GameObject image; //遮罩图片,死亡时显示
    PlayerCharacter chara; //角色类
    float timer = 0; //死亡后开始计时

    private void Update()
    {
        if (TimeManager.Instance.reverse || chara.death)
        {
            if (chara.death)
            {
                if (!TimeManager.Instance.reverse) //死亡后且未反转情况下
                {
                    c2d.enabled = false;
                    if (timer < 1f) //死亡后开始计时
                        timer += Time.deltaTime;
                    else //计时一定时间后暂停游戏
                    {
                        timer = 1;
                        TimeManager.Instance.audioSource.pitch = 0; //声音播放速度为0
                        TimeManager.Instance.dir = 0; //背景图片偏移速度为0
                        Time.timeScale = 0; //时间暂停
                        image.SetActive(true); //开启遮罩图片
                    }
                }
                else //反转时计时器复原,如果复原到死亡时间点则复活
                {
                    Time.timeScale = 1;
                    image.SetActive(false); //关闭遮罩
                    if (timer > 0)
                        timer -= Time.deltaTime;
                    else
                    {
                        timer = 0;
                        c2d.enabled = true;
                        chara.Resuscitate();
                    }
                }
            }
            return; //死亡或反转时不能控制角色
        }
        chara.Run(Input.GetAxisRaw("Horizontal")); //移动
        chara.Jump(Input.GetKeyDown(KeyCode.J)); //跳跃      
        if (transform.position.y < height) //落到悬崖下面死亡      
            chara.Death();
    }
    public override void InitState() //初始化状态
    {
        chara = GetComponent<PlayerCharacter>();
        base.InitState();
    }
}
