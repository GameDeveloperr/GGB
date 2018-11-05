using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    private Transform Player;
    private bool Parry = false;
    private Vector3 First_Pos;
    private bool Parry_first_check = false;

    public float m_nSpeed;
    public Vector2 Target;
    public GameObject Effect_player;
    public Sprite ParryingMat;

    public bool Follwing = false;

    public Sprite[] Popsprite = new Sprite[8];
    public Sprite follow_spr;

    void Start()
    {
        //처음 생성되는 위치 저장  패링 탄환을 사용하기 위한 방법
        //날아가는 방향 앞쪽이라는것을 지정
        Player = GameManager.Instance._PlayerC.transform;
        Target = Player.position - transform.position;
        First_Pos = this.transform.position;
    }


    void Update()
    {
        if (Follwing && !Parry)
        {
            GetComponent<SpriteRenderer>().sprite = follow_spr;
            Target = Player.position - transform.position;
        }

        if (Parry && !Parry_first_check)
        {
            Target =  this.gameObject.transform.position- First_Pos;
            Parry_first_check = true;
        }

        this.gameObject.transform.Translate(Target.normalized * m_nSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!Parry)
        {
            if (other.CompareTag("Player"))
            {
                //
                if (!GameManager.Instance._PlayerC._Die)
                {
                    GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.DAMAGED;
                }
                //
                GameObject effect = Instantiate(Effect_player, this.transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);
                GameManager.Instance._PlayerU.DeleteHP();

                Destroy(this.gameObject);
            }
        }
        else //패링 탄환일 경우
        {
            if(other.CompareTag("ENEMY"))//보스일떄
            {
                GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_damage);
                BossCtrl Boss = other.GetComponent<BossCtrl>();
                Boss.curState = BossCtrl.BossState.Damage;
                if (Boss.Shield_Mode == false){ //보스 방패모드 아닐때
                    GameManager.Instance._Bullet.ORB_Take_Damage(other);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 보스 체력에 따른 스킬 및 페이즈 변화 체크
                    if (Boss.Enemy_CurHp == 50 || Boss.Enemy_CurHp == 100 || Boss.Enemy_CurHp == 200 || Boss.Enemy_CurHp == 300 || Boss.Enemy_CurHp == 400)
                    {
                        Boss.worp_check = true;
                        Boss.Hand_check = false;
                        Boss.StartCoroutine("Worp_TIme");
                    }

                    if (Boss.Enemy_CurHp == Boss.Enemy_MaxHp * 0.7 || Boss.Enemy_CurHp == Boss.Enemy_MaxHp * 0.3)
                    {
                        Boss.Boss_Shield_On();
                    }

                    if (Boss.Enemy_CurHp == Boss.Enemy_MaxHp * 0.4) Boss.curFase = BossFase.Fase_two; //적 피가 40프로 남았을때 페이즈2로 전환

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    GameManager.Instance._Bullet.ORB_Take_Damage(other);//패링탄환 데미지 2 를 입히기 위해 나중에 체크

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 보스 체력에 따른 스킬 및 페이즈 변화 체크
                    if (Boss.Enemy_CurHp == 50 || Boss.Enemy_CurHp == 100 || Boss.Enemy_CurHp == 200 || Boss.Enemy_CurHp == 300 || Boss.Enemy_CurHp == 400)
                    {
                        Boss.worp_check = true;
                        Boss.Hand_check = false;
                        Boss.StartCoroutine("Worp_TIme");
                    }

                    if (Boss.Enemy_CurHp == Boss.Enemy_MaxHp * 0.7 || Boss.Enemy_CurHp == Boss.Enemy_MaxHp * 0.3)
                    {
                        Boss.Boss_Shield_On();
                    }

                    if (Boss.Enemy_CurHp == Boss.Enemy_MaxHp * 0.4) Boss.curFase = BossFase.Fase_two; //적 피가 40프로 남았을때 페이즈2로 전환

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    Boss.Boss_Hp_UI.fillAmount = (float)(Boss.Enemy_CurHp * 0.002);
                    Destroy(this.gameObject);

                    if (Boss.Enemy_CurHp < 1)//죽음
                    {
                        Boss.Death();
                    }

                }
                else //보스 방패모드일때
                {
                    GameManager.Instance._Bullet.ORB_Take_Damage_Shield(other);
                    if (Boss.CurShield_MP < 1)
                    {
                        Boss.Boss_Shield_Off();
                    }
                    Boss.Boss_Mp_UI.fillAmount = (float)(Boss.CurShield_MP * 0.2);
                    Destroy(this.gameObject);

                }
                
            }

            if (other.CompareTag("ENEMY_MOB"))//일반몹일때
            {
                GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_damage);

                EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();
                Destroy(this.gameObject);

                GameManager.Instance._Bullet.ORB_Take_Damage(other);
                GameManager.Instance._Bullet.ORB_Take_Damage(other);

                if (enemy.Enemy_CurHp <= 0 && !enemy.Enemy_Death && GameManager.Instance.curStage == Stage.Stage1)
                {
                    GameManager.Instance._Stage1.Count--;
                    //Debug.Log("Count : "+GameManager.Instance._Stage1.Count);
                    GameManager.Instance.Stage1_Game_Clear();
                    enemy.Death();
                }
                else if(enemy.Enemy_CurHp <= 0 && !enemy.Enemy_Death && GameManager.Instance.curStage == Stage.EventStage)
                {
                    enemy.Death();
                }
            }
        }

        if(other.gameObject.CompareTag("WALL"))
        {
            Destroy(this.gameObject);
        }
    }
    public void Payyied()
    {
        Parry = true;
        GetComponent<SpriteRenderer>().sprite = ParryingMat;
        gameObject.tag = "ORB_BULLET";
        m_nSpeed *= 2.0f;
    }
    public IEnumerator PopActive()
    {
        m_nSpeed = 0f;
        GetComponent<CircleCollider2D>().enabled = false;
        for (int i = 0; i < Popsprite.Length; ++i)
        {
            GetComponent<SpriteRenderer>().sprite = Popsprite[i];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
