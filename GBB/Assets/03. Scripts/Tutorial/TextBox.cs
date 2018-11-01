using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour {
    public TutorialManager T_Manager;
    public TutorialPros pro;
    //
    public bool TouchCheck = false;
    public Text txt_field;
    public Dialogue dial;
    public GameObject[] Next_Imgs = new GameObject[2];
    public GameObject AttackDist;
    //
    private int sentNum = 0;//말풍선 번호 순서
    public GameObject tBoxButton;

    public bool touch_double = false;//두번터치 확인 변수(대화상자 더블터치 확인)
    public bool enter = false;
    public GameObject P_shield;
    private bool _Stop = false;
    //
    TutorialManager.T_STAGE stage = TutorialManager.T_STAGE.MOVE;
    // Use this for initialization
    void OnEnable () {
        sentNum = 0;
        stage = T_Manager._Stage;
        Debug.Log(T_Manager._Stage);
        ResetTxtField();
        StartCoroutine(Printing(getSentence(stage)));
	}

    public string getSentence(TutorialManager.T_STAGE stage)
    {
        return dial.sentences[(int)stage][sentNum];
    }

    IEnumerator Arrow()
    {
        _Stop = false;
        while (!_Stop)
        {
            Next_Imgs[0].SetActive(true);
            yield return new WaitForSeconds(0.15f);
            Next_Imgs[0].SetActive(false);
            Next_Imgs[1].SetActive(true);
            yield return new WaitForSeconds(0.25f);
            Next_Imgs[1].SetActive(false);
        }
    }
	
    IEnumerator WaitNext()
    {
        tBoxButton.SetActive(false);
        TouchCheck = false;
        touch_double = false;
        StartCoroutine(Arrow());
        
        yield return new WaitForSeconds(0.1f);
        tBoxButton.SetActive(true);

        while(!TouchCheck)
        {
            yield return null;
        }
        _Stop = true;

        T_Manager.OnCtrlCan();
        sentNum++;//다음번 Printing 실행시 다음 문장이 나오도록 준비

        tBoxButton.SetActive(false);
        TouchCheck = false;
        touch_double = false;

        CheckNextSent();
    }

    IEnumerator Printing(string texts)
    {
        ResetTxtField();
        int i = 0;
        T_Manager.OffCtrlCan();
        tBoxButton.SetActive(true);

        while (i < texts.Length)
        {
            txt_field.text += texts[i];
            if (stage == TutorialManager.T_STAGE.SHILED && sentNum == 11)
            {//패링 예외처리
                txt_field.text = texts;
                T_Manager.OnCtrlCan();
                i = texts.Length;
            }

            yield return new WaitForSeconds(0.07f);
            i++;
            if (touch_double)
            {
                tBoxButton.SetActive(false);
                TouchCheck = false;
                txt_field.text = texts;
                touch_double = false;
                yield return null;
                i = texts.Length;
            }
        }
        StartCoroutine(WaitNext());
    }

    private void CheckNextSent()
    {
        Debug.Log(sentNum);
        switch (stage)
        {
            case TutorialManager.T_STAGE.MOVE: //스테이지
                if (sentNum == 1) {
                    StartCoroutine(Printing(getSentence(stage)));
                    return;
                }
                else if (sentNum > 1 && sentNum < 5) {
                    pro.Part01();
                    return;
                }
                else if (sentNum == 5) {
                    pro.CanGo();
                    return;
                }//이까지 1번방
                break;
            case TutorialManager.T_STAGE.ATTACK://스테이지
                if (sentNum > 0 && sentNum < 3) {
                    StartCoroutine(Printing(getSentence(stage)));
                    return;
                }
                else if (sentNum == 3)
                {
                    AttackDist.SetActive(true);
                    StartCoroutine(Printing(getSentence(stage)));
                    return;
                }//범위 생성
                else if (sentNum == 4) {
                    pro.Part02();
                    return;
                }
                else if (sentNum > 4 && sentNum < 7) {
                    StartCoroutine(Printing(getSentence(stage)));
                    return;
                }
                else if (sentNum == 7) {
                    pro.Part03();
                    return;
                }
                else if (sentNum == 8) {
                    StartCoroutine(Printing(getSentence(stage)));
                    //AttackDist.gameObject.SetActive(false);
                    pro.Monster.gameObject.SetActive(false);
                    return;
                }
                else if (sentNum == 9) {
                    pro.CanGo();
                    pro.SetOffOrb();
                    return;
                }
                break;
            case TutorialManager.T_STAGE.SHILED://스테이지
                if (sentNum > 0 && sentNum < 7) {
                    StartCoroutine(Printing(getSentence(stage)));
                    return;
                }
                else if (sentNum == 7) {
                    pro.Part04();
                    return;
                }
                else if (sentNum > 7 && sentNum < 11) {
                    StartCoroutine(Printing(getSentence(stage)));
                    return;
                }
                else if (sentNum == 11) {//타이밍에 맞춰서 막아보세여//총알멈춤 -> 지금입니다 출력
                    if (P_shield != null)
                    {
                        P_shield.SetActive(false);
                    }
                    pro.Part05();
                    return;
                }
                else if(sentNum == 12) { StartCoroutine(Printing(getSentence(stage))); return; }
                else if (sentNum == 13) {//계속하여
                    if (P_shield != null)
                    {
                        P_shield.SetActive(false);
                    }
                    pro.Part06();
                    return;
                }
                else if (sentNum == 14) {
                    if (P_shield != null)
                    {
                        P_shield.SetActive(false);
                    }
                    GameManager.Instance._PlayerC.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
                    pro.CanGo();
                    return;
                }
                break;
            case TutorialManager.T_STAGE.SKILL://스테이지
                if (sentNum == 1) { pro.Circle[5].gameObject.SetActive(true); StartCoroutine(Printing(getSentence(stage))); return; }
                else if (sentNum == 2) { pro.Part05(); return; }
                else if (sentNum > 2 && sentNum < 6) { StartCoroutine(Printing(getSentence(stage))); return; }
                else if (sentNum == 6) { pro.Monsters.SetActive(true); pro.Part07(); return; }
                else if (sentNum > 6 && sentNum < 9) { StartCoroutine(Printing(getSentence(stage))); pro.CanGo(); pro.Monsters.SetActive(false); return; }
                break;
            case TutorialManager.T_STAGE.FIGHT://스테이지
                if (sentNum == 1) { StartCoroutine(Printing(getSentence(stage))); return; }
                if(sentNum == 2) { pro.Part08(); return; }
                    break;
            default:
                break;
        }

    }
    private void ResetTxtField()
    {
        txt_field.text = "";
    }

    public void DialogTouch()
    {
        TouchCheck = true;
        if (TouchCheck && !touch_double)
            touch_double = true;
    }
    public void StartNext()
    {
        StartCoroutine(Printing(getSentence(stage)));
    }
}
