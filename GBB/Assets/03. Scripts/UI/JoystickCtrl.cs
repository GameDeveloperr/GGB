using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickCtrl : MonoBehaviour {

    // 공개
    public Transform Player;        // 플레이어.
    public Transform Stick;         // 조이스틱.
    public Transform Shiled;
    // 비공개
    private Vector2 StickFirstPos;  // 조이스틱의 처음 위치.
    private Vector2 JoyVec;         // 조이스틱의 벡터(방향)
    private float Radius;           // 조이스틱 배경의 반 지름.
    private bool MoveFlag;          // 플레이어 움직임 스위치.

    //이동속도 
    public float Speed = 6.0f;


    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;

        // 캔버스 크기에대한 반지름 조절.
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        MoveFlag = false;
    }

    void Update()
    {
        if (MoveFlag)
        {
            if (transform.localScale.x > 0)
            {
                Player.Translate(new Vector3(JoyVec.x, JoyVec.y, 0) * Time.deltaTime * Speed);
            }
            else
            {
                Player.Translate(new Vector3(-JoyVec.x, JoyVec.y, 0) * Time.deltaTime * Speed);
            }
            //ShiledPos();// 방패 이동 (2번 안)
        }
    }

    //방패
    public void ShiledPos()
    {
        float digree = Mathf.Atan2(JoyVec.y, JoyVec.x) * Mathf.Rad2Deg;
        Shiled.rotation = Quaternion.Euler(0, 0, digree - 90.0f);
    }

    // 드래그
    public void Drag(BaseEventData _Data)
    {
        MoveFlag = true;
        PointerEventData Data = _Data as PointerEventData;
        Vector2 Pos = Data.position;

        // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
        JoyVec = (Pos - StickFirstPos).normalized;
        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float Dis = Vector2.Distance(Pos, StickFirstPos);

        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는 곳으로 이동.
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
        else
            Stick.position = StickFirstPos + JoyVec * Radius;

        if (GameManager.Instance._PlayerC._State != PlayerCtrl.PlayerState.DAMAGED)
        {
            CheckPlayerState();
        }
    }

    public void CheckPlayerState()
    {
        if (!GameManager.Instance._PlayerC._Die)
        {
            bool LR = false;
            if (JoyVec.x > 0.5f)
            {//right
                LR = true;
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.RIGHT;
            }
            else if (JoyVec.x < -0.5f)
            {//left
                LR = true;
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.LEFT;
            }
            else if (JoyVec.y < 0 && !LR)
            {
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.DOWN;
            }
            else if (JoyVec.y > 0 && !LR)
            {
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.UP;
            }
        }
    }

    // 드래그 끝.
    public void DragEnd()
    {
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
        JoyVec = Vector2.zero;          // 방향을 0으로.
        MoveFlag = false;

        if (!GameManager.Instance._PlayerC._Die)
        {
            GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.NONE;
        }
    }

}
