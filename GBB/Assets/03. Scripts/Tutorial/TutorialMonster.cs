using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMonster : MonoBehaviour {

    public enum EnemyType
    {
        None = 0, NomalGun, ShotGun, Spiral, TUTORIAL
    }

    public enum EnemyState
    {
        None = 0, Idle, Run, Attack
    }

    public Transform Target; //플레이어 (타겟용)
    public bool Check = false;
    public bool DieCheck = false;
    public float SeekDist; //기준 거리
    public Transform Bullet;//총알
    public EnemyType CurEnemyType;//Enemy 종류
    public bool TargetOK; // 플레이어와의 거리 확인
    //Vector3 direct = -Vector3.forward;

    public int Enemy_MaxHp;
    public int Enemy_CurHp;

    public Transform DieEffect;

    public EnemyState curState;

    private Animator anim;
    //
    public bool CanDie = false;
    int count = 0;

    void OnEnable()
    {
        anim = GetComponent<Animator>();
        EnemyState curState = EnemyState.Idle;
        TargetOK = false;
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
                        StartCoroutine(Straight(gameObject.transform, Bullet));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    case EnemyType.ShotGun:
                        StartCoroutine(HalfCircle(gameObject.transform, Bullet, 10.0f, 0.01f));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    case EnemyType.Spiral:
                        StartCoroutine(Spiral(gameObject.transform, Bullet, 20, 1, 0.1f, true));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    case EnemyType.TUTORIAL:
                        StartCoroutine(Tutorial(gameObject.transform, Bullet));
                        yield return new WaitForSeconds(8.0f);
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
    IEnumerator Tutorial(Transform shooter, Transform bulletTrans)
    {
        for(int i = 0; i < 3; i++)
        {
            if (gameObject != null)
            {
                if (CurEnemyType == EnemyType.TUTORIAL)
                    Instantiate(bulletTrans, shooter.position, shooter.rotation);
                else
                    i = 3;
            }
            yield return new WaitForSeconds(2.0f);
        }
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
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot - volly));
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot + volly));
        }
        yield return new WaitForSeconds(shotTime);
        if (gameObject != null)
        {
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot - volly * 2));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ORB_BULLET"))
        {
            if(count >10)
            Check = true;
        }
        count++;
        if(collision.CompareTag("ORB_BULLET") && GameManager.Instance.T_Manager._Stage == TutorialManager.T_STAGE.FIGHT)
        {
            Enemy_CurHp--;
            if(Enemy_CurHp < 1) { Death(); }
        }
    }

    public void Death()
    {
        if (!DieCheck)
        {
            DieCheck = true;
            Transform ef = Instantiate(DieEffect, transform.position, Quaternion.identity);
            Destroy(ef.gameObject, 1.5f);
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
        
    }
}
