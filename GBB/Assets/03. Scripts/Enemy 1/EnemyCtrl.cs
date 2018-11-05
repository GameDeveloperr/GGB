using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour {

    public enum EnemyType
    {
        None=0, NomalGun, ShotGun, Spiral
    }

    public enum EnemyState
    {
        None=0, Idle, Run, Attack
    }

    private Transform Target; //플레이어 (타겟용)
    public bool Check = false;
    public float SeekDist; //기준 거리
    public Transform Bullet;//총알
    public EnemyType CurEnemyType;//Enemy 종류
    public bool TargetOK; // 플레이어와의 거리 확인
    //Vector3 direct = -Vector3.forward;

    public int Enemy_MaxHp;
    public int Enemy_CurHp;
    public bool Enemy_Death = false;

    public Transform DieEffect;

    public EnemyState curState;

    private Animator anim;

    void Start()
    {

        anim = GetComponent<Animator>();
        EnemyState curState = EnemyState.Idle;
        TargetOK = false;
        Target = GameManager.Instance._PlayerC.transform;
        Enemy_CurHp = Enemy_MaxHp;
        StartCoroutine("TargetCheck");
        StartCoroutine("EnemyTypeBullet");
        StartCoroutine("Enemy_State");

    }

    public IEnumerator Enemy_State()
    {
        while (true)
        {
            switch (curState)
            {
                case EnemyState.Idle:
                    //anim.SetBool("IsRun", false);
                   // anim.SetBool("IsAttack", false);
                    break;
                case EnemyState.Run:
                   // anim.SetBool("IsRun", true);
                    //anim.SetBool("IsAttack", false);
                    break;
                case EnemyState.Attack:
                    //anim.SetBool("IsRun", false);
                    //anim.SetBool("IsAttack", true);
                    break;
            }
            yield return null;
        }
    }
    public IEnumerator TargetCheck()   //플레이어와의 거리 탐색 후 공격 
    {
        while (true)
        {

            float Dist = Vector2.Distance(gameObject.transform.position, Target.position);
            if (Dist <= SeekDist)
            {
                TargetOK = true;
                curState = EnemyState.Attack;
            }
            else
            {
                TargetOK = false;
                curState = EnemyState.Idle;

            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator EnemyTypeBullet()
    {
        while (true)
        {
            if (TargetOK)
            {
                switch (CurEnemyType)
                {
                    case EnemyType.NomalGun:
                        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_monster_normal_attack);
                        StartCoroutine(Straight(gameObject.transform, Bullet));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    case EnemyType.ShotGun:
                        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_monster_normal_attack);
                        StartCoroutine(HalfCircle(gameObject.transform, Bullet, 10.0f, 0.01f));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    case EnemyType.Spiral:
                        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_monster_normal_attack);
                        StartCoroutine(Spiral(gameObject.transform, Bullet, 20, 1, 0.1f, true));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
    }

    //기본 타입
    IEnumerator Straight(Transform shooter, Transform bulletTrans)
    {
        for (int i = 0; i < 5; i++)
        {
            if (gameObject != null)
            {
                Transform Nomalbullet = Instantiate(bulletTrans, shooter.position, shooter.rotation);
                //Nomalbullet.GetComponent<EnemyBullet>().SetTarget(Target);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    //부채꼴 타입 
    IEnumerator HalfCircle(Transform shooter, Transform bulletTrans, float volly, float shotTime)
    {
        float bulletRot = shooter.eulerAngles.z;

        if (gameObject != null)
        {
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot));
        }
        yield return new WaitForSeconds(shotTime);
        if (gameObject != null)
        {
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0,0 , bulletRot - volly));
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0,0, bulletRot + volly));
        }
        yield return new WaitForSeconds(shotTime);
        if (gameObject != null)
        {
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0,0, bulletRot - volly * 2));
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot + volly * 2));
        }
        yield return null;

    }

    //스파이럴 타입
    IEnumerator Spiral(Transform shooter, Transform bulletTrans, int shotNum, int volly, float shotTime, bool clockwise)
    {
        float bulletRot = shooter.eulerAngles.z;
        while (volly > 0)
        {
            for (int i = 0; i < shotNum; i++)
            {
                if (gameObject != null)
                {
                    Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot));
                }
                if (clockwise)
                {
                    bulletRot += 360.0f / shotNum;
                }
                else
                {
                    bulletRot -= 360.0f / shotNum;
                    if (shotTime > 0)
                    {
                        yield return new WaitForSeconds(shotTime);
                    }
                }
                volly--;
            }
        }
    }


    public void Death()
    {
        if (GameManager.Instance.curStage == Stage.EventStage)
            GameManager.Instance.CreatM.KillUp();
        Enemy_Death = true;
        Transform ef = Instantiate(DieEffect, transform.position, Quaternion.identity);
        Destroy(ef.gameObject, 1.5f);
        Destroy(gameObject);
    }
}
