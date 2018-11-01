using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyBullet : MonoBehaviour {

    private Transform Player;
    private bool Parry = false;
    private Vector3 First_Pos;
    private bool Parry_first_check = false;


    public float m_nSpeed;
    public Vector2 Target;
    public GameObject Effect_player;
    public Sprite ParryingMat;
    public TutorialManager T_Manager;

    public Sprite[] Popsprite = new Sprite[8];


    void Start()
    {
        //날아가는 방향 앞쪽이라는것을 지정
        Player = GameManager.Instance._PlayerC.transform;
        Target = Player.position - transform.position;
        First_Pos = this.transform.position;
    }

    void Update()
    {
        //if (Parry)
        //{
        //    Target = this.gameObject.transform.position - First_Pos;
        //}

        if (Parry && !Parry_first_check)
        {
            Target = this.gameObject.transform.position - First_Pos;
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
                    GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_damaged_1);

                    GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.DAMAGED;
                }
                //
                GameObject effect = Instantiate(Effect_player, this.transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);
                if (GameManager.Instance.T_Manager._Stage == TutorialManager.T_STAGE.FIGHT)
                {
                    GameManager.Instance._PlayerU.DeleteHP();
                    if (GameManager.Instance._PlayerS.getCURHP()<=0)
                    {
                        Debug.Log("죽음");
                        for(int i = 0; i < 3; i++)
                        {

                            GameManager.Instance.T_Manager.T_Proces.CheckMonsters[i].DieCheck = false;
                        }
                    }
                }

                Destroy(this.gameObject);
            }
        }
        else //패링 탄환일 경우
        {
            if (other.CompareTag("TUTORIALENEMY"))
            {
                GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_damage);

                Destroy(this.gameObject);

                other.GetComponent<TutorialMonster>().Enemy_CurHp--;


                    if (other.GetComponent<TutorialMonster>().Enemy_CurHp < 1)
                    {
                        other.GetComponent<TutorialMonster>().Death();
                    }

            }
            
        }

        if (other.gameObject.CompareTag("WALL"))
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
