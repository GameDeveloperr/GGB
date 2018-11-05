using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCtrl : MonoBehaviour
{
    public BULLETTYPE Type;
    //
    private float _Time = 0;
    //
    private Vector3 BeforePos = Vector3.zero;
    private Vector3 CurPos = Vector3.zero;
    public Transform Player;
    public float Orb_rot_Speed = 80.0f;
    public float OrbPos = 0;
    public float Dist = 1.5f;
    //
    public float ReSet_Time = 2.0f; //총알 리셋 시간
    public float Attack_Time = 0.5f;
    private float AttackDist = 3.0f;
    private bool Attack = false;
    public Transform Target;
    private SpriteRenderer Orb_color;


    private void Start()
    {
        Orb_color = GetComponent<SpriteRenderer>();

        Player = GameManager.Instance._PlayerC.transform;
        BeforePos = Player.position;
        transform.position = new Vector3(Player.position.x + Dist, Player.position.y + OrbPos, Player.position.z);
    }
    void Update()
    {
        _Time += Time.deltaTime;
        if(_Time >= ReSet_Time && !Attack)
        {
            if(CheckEnemy())
            {
                StartCoroutine(OnBullet());
            }
        }
    }
    private void LateUpdate()
    {
        Move();
        transform.RotateAround(Player.position, Vector3.forward, Orb_rot_Speed * Time.deltaTime);
    }
    public void Move()
    {
        CurPos = Player.position;
        if (BeforePos != CurPos)
        {
            float moveX = CurPos.x - BeforePos.x;
            float movey = CurPos.y - BeforePos.y;
            transform.position = new Vector3(transform.position.x + moveX, transform.position.y + movey, 0);
        }

        BeforePos = Player.position;
    }
    //
    private bool CheckEnemy()
    {//플레이어 기준으로 적 판단
        Collider2D[] colls = Physics2D.OverlapCircleAll(Player.position, AttackDist);
        
        for (int i = 0; i != colls.Length; i++)
        {
            if ((colls[i].CompareTag("ENEMY") || colls[i].CompareTag("ENEMY_MOB") || colls[i].CompareTag("SLIME")) && Target == null)
            {
                Target = colls[i].gameObject.transform;
            }else if((colls[i].CompareTag("ENEMY") || colls[i].CompareTag("ENEMY_MOB") || colls[i].CompareTag("SLIME")) && Target != null)
            {
                if(Vector2.Distance(Player.position, Target.position) > Vector2.Distance(Player.position, colls[i].gameObject.transform.position))
                {
                    Target = colls[i].gameObject.transform;
                }
            }
            else
            {
                Target = null;
            }
        }
        if(Target != null)
        {
            Orb_color.color = Color.blue;
            return true;
        }
        else {
            Orb_color.color = Color.white;
            return false;
        }
    }
    IEnumerator OnBullet()
    {
        Attack = true;
        int count = 0;
        while (Target != null && count < 3)
        {
            GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_attack_hit);

            GameManager.Instance._Bullet.ORBBulletSet(Type, transform, Target, count);
            count++;
            yield return new WaitForSeconds(Attack_Time);
        }
        _Time = 0;
        Attack = false;
    }

}
