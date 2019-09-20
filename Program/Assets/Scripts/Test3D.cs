using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 根据摄像机高度模拟地面3D效果
/// 地面图片需要先拉九宫格
/// 根据摄像机与地面距离设置地面图片高度
/// </summary>
public class Test3D : MonoBehaviour
{
    Transform cameraTF;
    SpriteRenderer sr;
    public float minH, maxH, offset, lerp; //高度最小值,最大值,偏移量,插值

    void Start()
    {
        cameraTF = FindObjectOfType<CameraFollow>().transform;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (cameraTF.position.y > transform.position.y)
            offset = (cameraTF.position.y - transform.position.y) / lerp + minH;

        offset = Mathf.Clamp(offset, minH, maxH);
        sr.size = new Vector2(sr.size.x, offset);
    }
}
