using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour {

    public enum T_STAGE { MOVE, ATTACK, SHILED, SKILL, FIGHT, NONE }

    public T_STAGE _Stage = T_STAGE.MOVE;

    public GameObject WaitPannel;
    public Transform[] StartPos = new Transform[2];
    public TutorialPros T_Proces;
    public GameObject _TBox;
    public GameObject CtrCan;
    public JoystickCtrl JoyStick;
    public GameObject ClearCan;

    private float _Time = 0;

    private void Start()
    {
        GameManager.Instance.curStage = Stage.Tutorial;
        _Time = 0;
        StartCoroutine(Loading());
    }

    public IEnumerator Loading()
    {//패널 몇초간 켜기 -> 초기화 함수들 실행 (다음 스테이지를 진행하기 위한 준비)
        WaitPannel.SetActive(true);
        while(_Time < 1.0f)
        {//1초간 실행
            _Time += Time.deltaTime;
            
            yield return null;
        }
        SetStart();
        WaitPannel.SetActive(false);
        _TBox.SetActive(true);
        _Time = 0f;
    }

    public void OffCtrlCan()//조작키 off
    {
        JoyStick.DragEnd();
        CtrCan.SetActive(false);
    }

    public void OnCtrlCan()
    {
        CtrCan.SetActive(true);
    }

    public void SetStart()// 시작시 부를 함수 -> Wait패널이 깜빡하는 동안 실행
    {// 캐릭터 위치 지정
        GameManager.Instance._PlayerC.transform.position = StartPos[0].position;
        if (_Stage == T_STAGE.FIGHT)
        {
            GameManager.Instance._PlayerC.transform.position = StartPos[1].position;

        }
        //진행할 스테이지를 받아와야함
        SetStage(_Stage);
    }

    public void SetStage(T_STAGE stage)
    {//스테이지별 설정 -> 몬스터, 장애물 등 지정
        switch(stage)
        {
            case T_STAGE.MOVE:
                break;
            case T_STAGE.ATTACK:
                break;
            case T_STAGE.SHILED:
                break;
            case T_STAGE.SKILL:
                break;
            case T_STAGE.FIGHT:
                GameManager.Instance._PlayerS.setFirst();

                break;
            default:
                stage = T_STAGE.FIGHT;
                break;
        }
    }

    public void Clear()
    {
        ClearCan.SetActive(true);
    }
}
