using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CreatePos
{
    public Vector2 C_Pos;
    public float z_q;
    // C_Pos 를 위치에 z_q를 z 각도로 적용
}

public class Boss2_Look : MonoBehaviour {
    /*
     * 체력이 20% 이하로 떨어질 시에 페이즈 2
     * 처음 소환 -> 상단 중앙
     * 1. 탄환공격
     * 2. 워프 -> 몬스터 소환 위치 후 생성
     * -> 그랩 소환 위치 후 생성
     * 3. 레이저
     */
    public enum State
    {
        NONE, STAY, ATTACk, WARP, MOVE, RASER, DIE
    }

    public Animator _FaceAnim;
    public Animator _Hand01Anim;
    public Animator _Hand02Anim;

    public Transform[] _hands = new Transform[2];
    public Transform[] _hand_space = new Transform[2];
    public Sprite _grab_hand;
    public Sprite _normal_hand;

    public  CreatePos[] createPos = new CreatePos[3];

    public State _State = State.NONE;
    
    void Start()
    {
        OffHandSpace();
        setWarpPos(0);
        _State = State.STAY;
        StartCoroutine(CheckState());
    }

    public void setWarpPos(int index)
    {
        transform.position = createPos[index].C_Pos;
        transform.rotation = Quaternion.Euler(0, 0, createPos[index].z_q);
    }
    public void OnHandSpace()
    {
        // Position 선정-10 -18 / 21 31
        _hands[0].position =new Vector2(Random.Range(-10.0f, -18.0f), Random.Range(21.0f, 31.0f));
        _hands[1].position = new Vector2(Random.Range(-10.0f, -18.0f), Random.Range(21.0f, 31.0f));
        _hand_space[0].gameObject.SetActive(true);
        _hand_space[1].gameObject.SetActive(true);
        _Hand01Anim.gameObject.GetComponent<SpriteRenderer>().sprite = _grab_hand;
        _Hand02Anim.gameObject.GetComponent<SpriteRenderer>().sprite = _grab_hand;
    }
    public void OffHandSpace()
    {
        _hand_space[0].gameObject.SetActive(false);
        _hand_space[1].gameObject.SetActive(false);
    }
    public void setHandfirstPos()
    {
        _hands[0].position = new Vector2(-6.0f, -1.0f);
        _hands[1].position = new Vector2(6.0f, -1.0f);
        _Hand01Anim.gameObject.GetComponent<SpriteRenderer>().sprite = _normal_hand;
        _Hand02Anim.gameObject.GetComponent<SpriteRenderer>().sprite = _normal_hand;
    }


    IEnumerator CheckState()
    {
        while (_State != State.DIE)
        {
            switch (_State)
            {
                case State.NONE:
                    //워프 후 생성
                    _FaceAnim.gameObject.SetActive(true);
                    OnHandSpace();
                    yield return new WaitForSeconds(1.0f);
                    _Hand01Anim.gameObject.SetActive(true);
                    _Hand02Anim.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    OffHandSpace();
                    setHandfirstPos();
                    _State = State.STAY;
                    break;
                case State.STAY:
                    _FaceAnim.SetBool("IsAttack", false);
                    _Hand01Anim.SetBool("IsAttack", false);
                    _Hand02Anim.SetBool("IsAttack", false);
                    break;
                case State.ATTACk:
                    _FaceAnim.SetBool("IsAttack", true);
                    _Hand01Anim.SetBool("IsAttack", true);
                    _Hand02Anim.SetBool("IsAttack", true);
                    //탄환 생성
                    break;
                case State.WARP:
                    _FaceAnim.SetTrigger("IsWarp");
                    _Hand01Anim.gameObject.SetActive(false);
                    _Hand02Anim.gameObject.SetActive(false);
                    yield return new WaitForSeconds(2.5f);
                    _FaceAnim.gameObject.SetActive(false);
                    // 워프 함수// -> 생성 위치 로 이동// -> 그랩 공격 -> 레이저(플레이어 따라 다니기)
                    //-> 워프 -> 이동 -> 그랩 -> 워프 끝 -> 몬스터 소환
                    // 워프 후 재 생성 시 stat 는 none 에서 바로 stay 로 바뀜
                    setWarpPos(Random.Range(0, 3));
                    _State = State.NONE;
                    break;
                case State.MOVE:
                    break;
                case State.RASER:
                    break;
            }
            yield return null;
        }
    }
    public void ChangeState(State state)
    {
        _State = state;
    }
}
