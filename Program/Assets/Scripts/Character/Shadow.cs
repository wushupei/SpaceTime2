using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public List<SpriteRenderer> shadows;
    private SpriteRenderer charaSR;
    void LateUpdate()
    {
        FollowParent();
    }
    public void InitShadows(SpriteRenderer _charaSR)
    {
        charaSR = _charaSR;
        for (int i = 0; i < shadows.Count; i++)
        {
            shadows[i].transform.localScale = charaSR.transform.localScale;
            shadows[i].color = new Color(1, 1, 1, charaSR.color.a - 0.5f - i * 0.1f);
            shadows[i].flipX = charaSR.flipX;
            shadows[i].sortingOrder = charaSR.sortingOrder - 1 - i;
        }
    }
    private void FollowParent() //跟随
    {
        for (int i = 0; i < shadows.Count; i++)
        {
            shadows[i].transform.position = Vector3.Lerp(shadows[i].transform.position, (i == 0 ? charaSR : shadows[i - 1]).transform.position, Time.deltaTime * 30);
            shadows[i].transform.rotation = charaSR.transform.rotation;
            shadows[i].sprite = charaSR.sprite;
            shadows[i].enabled = TimeManager.Instance.reverse;
        }
    }
}
