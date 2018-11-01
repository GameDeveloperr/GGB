using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossFase
{
    None, Fase_One, Fase_two
}

public class BossCtrl : MonoBehaviour {

    public enum EnemyType
    {
        None = 0, NomalGun, ShotGun, Following, Spiral
    }

    public enum BossState
    {
        None = 0, Idle, Damage, Attack,
    }



    private Transform Target; //플레이어 (타겟용)
    private EnemyType CurEnemyType;//Enemy 종류
    private Animator anim;

    public float SeekDist; //기준 거리
    public Transform Bullet;//총알
    public bool TargetOK; // 플레이어와의 거리 
    public bool FirstCheck = false; //처음 플레이어 발견

    public int Enemy_MaxHp;
    public int Enemy_CurHp;

    public Transform DieEffect;
    public BossState curState;

    public Transform efft;// 처음 워프 이동시 폭발 
    public Transform Boss_Hole;
    public Transform Hand;//직접 공격
    public Transform Hand_Hole;//직접 공격

    public Transform[] Hand_pos;
    public Transform Boss;

    public BossFase curFase = BossFase.None;

    // 보스 방어막 MP
    public int MaxShield_MP;
    public int CurShield_MP;
    public bool Shield_Mode = false;
    public Transform Shield_Trans;



    //보스 이동 범위
    public Vector3[] boss_range = null;

    //보스 체력 UI
    public Image Boss_Hp_UI;
    //보스 MP UI
    public Image Boss_Mp_UI;

    //보스 체력바 전체
    public Image Boss_Gage;
    //보스 스킬 시전중인 것


    //Boss가 죽는것을 각  BulletScript에서 실행 할 것인지 아니면  BossScripts에서 update나 corutine으로 해보던가
    public bool worp_check = false;
    public bool Hand_check = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        BossState curState = BossState.Idle;
        TargetOK = false;
        Target = GameManager.Instance._PlayerC.transform;
        CurEnemyType = EnemyType.None;
        CurShield_MP = 0;
        Boss_Mp_UI.fillAmount = (float)(CurShield_MP * 0.01);
        Enemy_CurHp = Enemy_MaxHp;
        StartCoroutine("TargetCheck");
        StartCoroutine("EnemyTypeBullet");
        StartCoroutine("Enemy_State");
        StartCoroutine("Direct_Attack");
    }

    public IEnumerator Enemy_State()
    {
        while (true)
        {
            switch (curState)
            {
                case BossState.Idle:
                    //anim.SetBool("Worp_Down", false);
                    //anim.SetBool("Worp_Up", false);
                    break;
                case BossState.Damage:
                    anim.SetTrigger("Damage");
                    break;
                case BossState.Attack:
                    break;
  
            }
            yield return null;
        }
    }

    public IEnumerator TargetCheck()   //플레이어와의 거리 탐색 후 공격 
    {
        while (true)
        {
            float Dist = Vector2.Distance(Boss.position, Target.position);
            if (Dist <= SeekDist)
            {
                if (!FirstCheck)
                {
                    TargetOK = true;
                    Boss_Gage.gameObject.SetActive(true);
                    worp_check = true;
                    StartCoroutine("Worp_TIme");
                    FirstCheck = true;
                    yield return new WaitForSeconds(3.0f);
                    curFase = BossFase.Fase_One;

                }

                if (FirstCheck)
                {
                    TargetOK = true;
                    Boss_Gage.gameObject.SetActive(true);
                    curState = BossState.Attack;
                }
            }
            else
            {
                TargetOK = false;
                Boss_Gage.gameObject.SetActive(false);

                curState = BossState.Idle;

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
                //Debug.Log(CurEnemyType);
                switch (CurEnemyType)
                {
                    case EnemyType.NomalGun:
                        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.nomal_bullet);

                        if (curFase == BossFase.Fase_One)
                        {
                            StartCoroutine(Straight(Boss, Bullet, 5));
                            yield return new WaitForSeconds(3.0f);
                        }
                        else if (curFase == BossFase.Fase_two)
                        {
                            StartCoroutine(Straight(Boss, Bullet, 12));
                            yield return new WaitForSeconds(3.0f);
                        }
                        break;
                    case EnemyType.ShotGun:
                        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Shotgun_bullet);

                        if (curFase == BossFase.Fase_One)
                        {
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(0.25f);
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(0.25f);
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(3.0f);
                        }
                        else if (curFase == BossFase.Fase_two)
                        {
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(0.25f);
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(0.25f);
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(0.25f);
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(0.25f);
                            StartCoroutine(HalfCircle(Boss, Bullet, 10.0f, 0.01f));
                            yield return new WaitForSeconds(3.0f);
                        }
                        break;
                    case EnemyType.Spiral:
                        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.spiral_bullet);

                        StartCoroutine(Spiral(Boss, Bullet, 20, 1, 0.1f, true));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    case EnemyType.Following:
                        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.following_bullet);

                        if (curFase == BossFase.Fase_One)
                        {
                            StartCoroutine(FowllingShot(Boss, Bullet, 2, 1.5f));
                            StartCoroutine(FowllingShot(Boss, Bullet, 2, -1.5f));
                            yield return new WaitForSeconds(3.0f);
                        }
                        else if (curFase == BossFase.Fase_two)
                        {
                            StartCoroutine(FowllingShot(Boss, Bullet, 3, 1.5f));
                            StartCoroutine(FowllingShot(Boss, Bullet, 3, -1.5f));
                            yield return new WaitForSeconds(3.0f);
                        }
                        break;
                }
            }
            yield return null;
        }
    }

    //유도탄
    IEnumerator FowllingShot(Transform shooter, Transform bulletTrans, int bullet_count, float addPos)//보스 중심 +- 1.5
    {
        Vector3 shotPos = new Vector3(shooter.position.x + addPos, shooter.position.y, 0);
        for (int i = 0; i < bullet_count; ++i)
        {
            if (gameObject != null)
            {
                Transform Nomalbullet = Instantiate(bulletTrans, shotPos, shooter.rotation);
                Nomalbullet.GetComponent<EnemyBullet>().Follwing = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        CurEnemyType = (EnemyType)Random.Range(1, 4);
        yield return null;
    }
    //기본 타입
    IEnumerator Straight(Transform shooter, Transform bulletTrans, int bullet_count)
    {
        for (int i = 0; i < bullet_count; i++)
        {
       
            if (gameObject != null)
            {    
                Transform Nomalbullet = Instantiate(bulletTrans, shooter.position, shooter.rotation);
            }
            yield return new WaitForSeconds(0.1f);
        }
        CurEnemyType = (EnemyType)Random.Range(1, 4);

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
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot - volly));
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot + volly));
        }
        yield return new WaitForSeconds(shotTime);
        if (gameObject != null)
        {
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot - volly * 2));
            Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, 0, bulletRot + volly * 2));
        }

        CurEnemyType = (EnemyType)Random.Range(1, 4);
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
            yield return new WaitForSeconds(2f);

            CurEnemyType = (EnemyType)Random.Range(1, 4);

            yield return null;
        }
    }

    //유도탄
    

    //워프
    public IEnumerator Worp_TIme()
    {
        if (worp_check)
        {
            
                if (!FirstCheck) yield return new WaitForSeconds(2.0f);

            Vector3 Tr = Target.position;

            if(Target.position.x < boss_range[0].x)
            {
                Tr = new Vector3(boss_range[0].x, Target.position.y, 0);
            }
            else if(Target.position.x > boss_range[1].x)
            {
                Tr = new Vector3(boss_range[1].x, Target.position.y, 0);
            }
            if (Target.position.y > boss_range[0].y)
            {
                Tr = new Vector3(Target.position.x, boss_range[0].y, 0);
            }
            else if (Target.position.y < boss_range[1].y)
            {
                Tr = new Vector3(Target.position.x, boss_range[1].y, 0);
            }

            if (TargetOK)
                {
                    Boss_Hole.position = Tr;
                    Boss_Hole.gameObject.SetActive(true);
                    yield return new WaitForSeconds(1.0f);
                    Boss_Hole.gameObject.SetActive(false);
                    Boss.position = new Vector2(Boss_Hole.position.x, Boss_Hole.position.y + 1.0f);
                    efft.gameObject.SetActive(true);
                    efft.position = Boss.position;
                    GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_worp);

                    Invoke("Eff_False", 0.6f);//폭파 몇초뒤 삭제
                    worp_check = false;
                    CurEnemyType = EnemyType.Spiral;
                    yield return new WaitForSeconds(1.0f);
                    Hand_check = true;
                }
        }
        yield return null;
    }

    //손공격
    public IEnumerator Direct_Attack()
    {

        while (true)
        {
            
            if (Hand_check && !worp_check)
            {
                Vector3 Tr = Target.position;

                //if (Target.position.x < boss_range[0].x)
                //{
                //    Tr = new Vector3(boss_range[0].x, Target.position.y, 0);
                //}
                //else if (Target.position.x > boss_range[1].x)
                //{
                //    Tr = new Vector3(boss_range[1].x, Target.position.y, 0);
                //}
                //if (Target.position.y > boss_range[0].y)
                //{
                //    Tr = new Vector3(Target.position.x, boss_range[0].y, 0);
                //}
                //else if (Target.position.y < boss_range[1].y)
                //{
                //    Tr = new Vector3(Target.position.x, boss_range[1].y, 0);
                //}
                if (TargetOK)
                    {
                        //홀 적용
                        Hand_Hole.position = Tr;
                        Hand_Hole.gameObject.SetActive(true);
                        //curState = BossState.Worp_Down;
                        yield return new WaitForSeconds(2.0f);
                        Hand_Hole.gameObject.SetActive(false);
                        Hand.gameObject.SetActive(true);
                        //Hand.position = Hand_Hole.position;
                        Hand.position = new Vector2(Hand_Hole.position.x, Hand_Hole.position.y - 0.6f);
                        yield return new WaitForSeconds(1.0f);
                        Hand.gameObject.SetActive(false);
                        yield return new WaitForSeconds(2.0f);
                    }
            }
            yield return null;
        }
    }


    //방어막 사용
    public void Boss_Shield_On()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_shield_on);
        Shield_Mode = true;
        Shield_Trans.gameObject.SetActive(true);
        CurShield_MP = MaxShield_MP;
        Boss_Mp_UI.fillAmount = 1.0f;
    }

    public void Boss_Shield_Off()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_shield_broken);
        Shield_Trans.gameObject.SetActive(false);
        Shield_Mode = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))//폭발
        {
            if (!GameManager.Instance._PlayerC._Die)
            {
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.DAMAGED;
            }

            GameManager.Instance._PlayerU.DeleteHP();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)//접촉시 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!GameManager.Instance._PlayerC._Die)
            {
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.DAMAGED;
            }
            GameManager.Instance._PlayerU.DeleteHP();
        }
    }


    
    void Hand_False(int num)
    {
        Hand.gameObject.SetActive(false);
    }

    public void Death()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_die);
        this.gameObject.SetActive(false);
        DieEffect.position = this.gameObject.transform.position;
        DieEffect.gameObject.SetActive(true);
        Invoke("death_eff", 1.0f);
        Invoke("GameClear", 1.0f);

    }
    void GameClear()
    {
        GameManager.Instance.Game_Clear();
    }


    //죽음 이펙트
    void death_eff()
    {
        DieEffect.gameObject.SetActive(false);
    }

    // 워프 이펙트
    void Eff_False()
    {
        efft.gameObject.SetActive(false);
    }

}
