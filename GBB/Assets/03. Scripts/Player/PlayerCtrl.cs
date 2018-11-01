using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour {
    public enum PlayerState
    {
        NONE, UP, DOWN, RIGHT, LEFT, DAMAGED, DIE
    }

    public Animator[] _Anim = new Animator[3];
    public PlayerState _State = PlayerState.NONE;

    public bool _Die = false;
    public GameObject[] FeverOrbs = new GameObject[2];
    public GameObject FeverEffect;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(AnimPlayer());
	}
	

    IEnumerator AnimPlayer()
    {
        while(!_Die)
        {
            switch(_State)
            {
                case PlayerState.NONE:
                    _Anim[1].SetBool("Right", false);
                    _Anim[0].SetBool("Up", false);
                    _Anim[0].SetBool("Right", false);
                    _Anim[0].SetBool("Down", true);
                    _Anim[1].SetBool("Down", false);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case PlayerState.UP:
                    _Anim[1].SetBool("Right", false);
                    _Anim[0].SetBool("Up", true);
                    _Anim[1].SetBool("Down", true);
                    _Anim[0].SetBool("Right", false);
                    _Anim[0].SetBool("Down", false);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case PlayerState.DOWN:
                    _Anim[1].SetBool("Right", false);
                    _Anim[0].SetBool("Down", true);
                    _Anim[1].SetBool("Down", true);
                    _Anim[0].SetBool("Up", false);
                    _Anim[0].SetBool("Right", false);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case PlayerState.RIGHT:
                    _Anim[0].SetBool("Up", false);
                    _Anim[0].SetBool("Down", false);
                    _Anim[0].SetBool("Right", true);
                    _Anim[1].SetBool("Down", false);
                    _Anim[1].SetBool("Right", true);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case PlayerState.LEFT:
                    _Anim[0].SetBool("Right", true);
                    _Anim[0].SetBool("Down", false);
                    _Anim[0].SetBool("Up", false);
                    _Anim[1].SetBool("Down", false);
                    _Anim[1].SetBool("Right", true);
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case PlayerState.DAMAGED:
                    _Anim[2].gameObject.SetActive(true);
                    _Anim[0].gameObject.SetActive(false);
                    _Anim[1].gameObject.SetActive(false);
                    yield return new WaitForSeconds(1.0f);
                    _State = PlayerState.NONE;
                    if (!_Die)
                    {
                        _Anim[0].gameObject.SetActive(true);
                        _Anim[1].gameObject.SetActive(true);
                        _Anim[2].gameObject.SetActive(false);
                    }
                    break;
            }
            yield return null;
        }
    }

    public void Die()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_die);

        _Anim[2].gameObject.SetActive(true);
        _Anim[0].gameObject.SetActive(false);
        _Anim[1].gameObject.SetActive(false);
        _Anim[2].SetBool("Die", true);
        if (GameManager.Instance.curStage != Stage.Tutorial)
        {
            _Die = true;
            GameManager.Instance.GameOver();
        }
    }

    public void Skill()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.EMP);
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 30.0f);
        if (colls.Length != 0)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].CompareTag("ENEMYBULLET"))
                {
                    StartCoroutine(SkillActive(colls[i]));
                }
            }
        }
    }
    public IEnumerator SkillActive(Collider2D mon)
    {
        switch(GameManager.Instance.curStage)
        {
            case Stage.Tutorial:
                TutorialEnemyBullet bull = mon.GetComponent<TutorialEnemyBullet>();
                StartCoroutine(bull.PopActive());
                break;
            default:
                EnemyBullet bulls = mon.GetComponent<EnemyBullet>();
                StartCoroutine(bulls.PopActive());
                break;
        }

        yield return new WaitForSeconds(0.9f);
        Destroy(mon.gameObject);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 3.0f);
    //}

    //public void OnFever()
    ////{
    //    //FeverOrbs[0].SetActive(true);
    //    //FeverOrbs[1].SetActive(true);
    //    FeverEffect.SetActive(true);
    //    Invoke("OffFever", 5.0f);
    //}

    //public void OffFever()
    //{
    //    //FeverOrbs[0].SetActive(false);
    //    //FeverOrbs[1].SetActive(false);
    //    FeverEffect.SetActive(false);
    //}


}
