using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //单例
    private static TimeManager _Instance;
    public static TimeManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("TimeManager");
                _Instance = obj.AddComponent<TimeManager>();
            }
            return _Instance;
        }
    }
    public List<TimeGameObject> reverseObj = new List<TimeGameObject>(); //所有受时间缩放控制的物体
    public bool reverse; //时间是否倒退
    public AudioSource audioSource;
    public Material bj; //背景图片材质
    float pos; //背景图片纹理坐标
    public float dir = -0.1f; //背景图片纹理偏移方向

    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        bj = GameObject.Find("Bj").GetComponent<SpriteRenderer>().material;
        InitAllState();
        InvokeRepeating("SaveAllState", 0.05f, 0.05f);
        InvokeRepeating("ClearAllState", 0.1f, 0.05f);
    }

    private void Update()
    {
        //背景图片纹理偏移
        pos += Time.deltaTime * dir;
        bj.mainTextureOffset = new Vector2(pos, 0);
        if (Input.GetKeyDown(KeyCode.Space)) //按下空格后为时间倒放模式
        {
            reverse = true; //倒放模式
            audioSource.pitch = -1; //音效播放顺序
            dir = 0.1f; //背景图片往右偏移
            bj.color = new Color(1, 1, 0.5f, 1); //背景颜色           
            SwitchAllState(); //所有倒放的物体切换状态
        }
        if (Input.GetKeyUp(KeyCode.Space)) //弹起空格后为时间正放模式
        {
            audioSource.pitch = 1;
            dir = -0.1f;
            bj.color = new Color(1, 1, 1, 1);
            reverse = false;
            SwitchAllState();
        }
        if (reverse == true)
            ResetAllState();
    }
    void InitAllState() //所有物体初始化状态
    {
        for (int i = 0; i < reverseObj.Count; i++)
        {
            reverseObj[i].InitState();
        }
    }
    void SaveAllState() //所有物体保存状态
    {
        if (reverse == false)
        {
            for (int i = 0; i < reverseObj.Count; i++)
            {
                reverseObj[i].SaveState();
            }
        }
    }
    void SwitchAllState() //所有物体切换状态
    {
        for (int i = 0; i < reverseObj.Count; i++)
        {
            reverseObj[i].SwitchState();
        }
    }
    void ResetAllState() //所有物体重置状态
    {
        for (int i = 0; i < reverseObj.Count; i++)
        {
            reverseObj[i].ResetState();
        }
    }
    void ClearAllState() //所有物体清除状态
    {
        if (reverse == true)
        {
            for (int i = 0; i < reverseObj.Count; i++)
            {
                reverseObj[i].ClearState();
            }
        }
    }
}
