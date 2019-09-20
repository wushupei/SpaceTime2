using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerCharacter player;
    float minX, maxX, minY, maxY;
    private void Start()
    {
        CameraAdaptation();
    }
    private void CameraAdaptation() //摄像机跟随适配
    {
        string screen = Screen.width + "_" + Screen.height;
        switch (screen)
        {
            case "1600_900":
                minX = -10; maxX = 10; minY = 8; maxY = 18;
                break;
            case "1280_1024":
                minX = -20; maxX = 20; minY = 7; maxY = 19;
                break;
        }
    }

    void LateUpdate()
    {
        if (player.death) return; //玩家死亡取消跟随

        //获取玩家的坐标
        Vector3 pos = player.transform.position;
        //获取摄像机和玩家的距离
        float distance = Vector3.Distance(transform.position, pos);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        //距离越远摄像机跟随越快
        transform.position = Vector3.Lerp(transform.position, pos + Vector3.back + Vector3.up, Time.deltaTime * distance);
    }
}
