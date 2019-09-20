using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeGameObject : MonoBehaviour
{
    Rigidbody2D r2d; //刚体
    protected BoxCollider2D c2d; //碰撞器
    SpriteRenderer sr; //精灵渲染器
    Stack<Vector2> posInfo = new Stack<Vector2>(); //保存自身位置
    Stack<Sprite> sprInfo = new Stack<Sprite>(); //保存动画动作(图片)
    Stack<int> dirInfo = new Stack<int>(); //保存面朝方向
    Stack<Vector2> velInfo = new Stack<Vector2>(); //保存速度信息
    float dis;
    private void Awake()
    {
        TimeManager.Instance.reverseObj.Add(this); //将自身添加进时间管理器
        LoadShadow();
    }
    private void LoadShadow() //加载影子
    {
        Shadow shadow = Instantiate(Resources.Load<Shadow>("Prefab/Shadows"));
        shadow.InitShadows(GetComponentInChildren<SpriteRenderer>());
        shadow.name = name + "sShadow";
    }
    public virtual void InitState() //初始化状态
    {
        c2d = GetComponent<BoxCollider2D>();
        r2d = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        SaveState(); //保存初始位置
    }
    public void SaveState() //保存状态
    {
        posInfo.Push(transform.position);
        sprInfo.Push(sr.sprite);
        dirInfo.Push((int)transform.eulerAngles.y);
        velInfo.Push(r2d.velocity);
    }
    public void SwitchState() //切换状态
    {
        if (TimeManager.Instance.reverse)
        {
            GetComponentInChildren<Animator>().enabled = false; //禁用动画控制器
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; //设置刚体静态           
            dis = Vector3.Distance(transform.position, posInfo.Peek()); //和上次时间点位置的距离
        }
        else
        {
            GetComponentInChildren<Animator>().enabled = true; //启用动画控制器
            r2d.bodyType = RigidbodyType2D.Dynamic; ////取消刚体静态  
            r2d.velocity = velInfo.Peek();
        }
    }
    public void ResetState() //重置状态
    {
        if (sprInfo.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, posInfo.Peek(), dis * 20 * Time.deltaTime);
            sr.sprite = sprInfo.Peek();
            transform.eulerAngles = new Vector3(0, dirInfo.Peek(), 0);
        }
    }
    public void ClearState() //清除状态
    {
        if (sprInfo.Count > 1)
        {
            posInfo.Pop();
            sprInfo.Pop();
            dirInfo.Pop();
            velInfo.Pop();
            dis = Vector3.Distance(transform.position, posInfo.Peek());
        }
        else //状态清除完毕,背景停止移动,音乐停止播放      
        {
            TimeManager.Instance.dir = 0;
            TimeManager.Instance.audioSource.pitch = 0;
        }
    }








}
