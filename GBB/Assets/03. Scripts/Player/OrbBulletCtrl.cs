using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBulletCtrl : MonoBehaviour {

    private Vector2 Target_Dir;
    public float m_nSpeed = 5.0f;
    private Animator _Anim;

    bool On = false;

    void Start()
    {
        _Anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        On = true;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        while(On)
        {
            this.gameObject.transform.Translate(Target_Dir.normalized * m_nSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
        transform.position = transform.position;
        _Anim.SetBool("End", true);
        yield return new WaitForSeconds(0.5f);

        GameManager.Instance._Bullet.BulletOff(gameObject);
    }

    public void SetBullet(Transform Target)
    {
        Target_Dir = Target.position - transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            On = false;
        }

        if (other.gameObject.CompareTag("ENEMY_MOB"))
        {
            GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_damage);

            EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();
            GameManager.Instance._Bullet.ORB_Take_Damage(other);
            //Debug.Log(enemy.Enemy_CurHp);
            On = false;

            if (enemy.Enemy_CurHp <= 0 && !enemy.Enemy_Death)
            {
                GameManager.Instance._Stage1.Count--;
                //Debug.Log("count : " + GameManager.Instance._Stage1.Count);
                GameManager.Instance.Stage1_Game_Clear();
                enemy.Death();
            }

        }

        if (other.gameObject.CompareTag("ENEMY"))
        {
            On = false;


            if (other.GetComponent<BossCtrl>().Shield_Mode == false) //방패 모드가 아닐때
            {
                GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_damage);
                GameManager.Instance._Bullet.ORB_Take_Damage(other);
                BossCtrl Boss = other.GetComponent<BossCtrl>();
                Boss.curState = BossCtrl.BossState.Damage;
                if (Boss.Enemy_CurHp==50|| Boss.Enemy_CurHp == 100 || Boss.Enemy_CurHp == 200 || Boss.Enemy_CurHp == 300 || Boss.Enemy_CurHp == 400)
                {
                    Debug.Log("워프");
                    Boss.worp_check = true;
                    Boss.Hand_check = false;
                    Boss.StartCoroutine("Worp_TIme");
                }

                if (Boss.Enemy_CurHp == Boss.Enemy_MaxHp*0.7 || Boss.Enemy_CurHp == Boss.Enemy_MaxHp * 0.3)
                {
                    Boss.Boss_Shield_On();
                }

                if (Boss.Enemy_CurHp == Boss.Enemy_MaxHp*0.4) Boss.curFase = BossFase.Fase_two; //적 피가 40프로 남았을때 페이즈2로 전환


                //float bosshp;
                Boss.Boss_Hp_UI.fillAmount = (float)(Boss.Enemy_CurHp * 0.002);
                //Debug.Log("hp:"+other.GetComponent<BossCtrl>().Enemy_CurHp);

                if (Boss.Enemy_CurHp < 1)
                {
                    Boss.Death();
                }

            }
            else// 보스가 방패모드 일경우 패링 공격만 통함 일반공격 X
            {
                //GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.);

            }
        }

        if(other.gameObject.CompareTag("TUTORIALENEMY"))
        {
            GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_damage);

            //GameManager.Instance._Bullet.ORB_Take_Damage(other);
            On = false;
        }

       

    }

}
