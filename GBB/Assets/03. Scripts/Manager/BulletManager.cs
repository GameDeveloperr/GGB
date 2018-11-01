using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLETTYPE { ORB_01, ORB_02, ORB_03, PARRYING, BOSS }
[System.Serializable]
public struct TypeArr
{
    public BULLETTYPE type;
    public OrbBulletCtrl[] orbbullets;
    // + 보스 뷸렛 // 빈 것은 null
}
public class BulletManager : MonoBehaviour {


    public TypeArr[] types = new TypeArr[5];

    public void BulletOn(GameObject bullet)
    {
        bullet.SetActive(true);
    }
    public void BulletOff(GameObject bullet)
    {
        bullet.SetActive(false);
    }
    //ORB
    public void ORBBulletSet(BULLETTYPE type, Transform orb, Transform Target, int count)
    {
        types[(int)type].orbbullets[count].transform.position = orb.position;
        float _z = Mathf.Atan2(Target.position.y - orb.position.y,
                    Target.position.x - orb.position.x) * Mathf.Rad2Deg;
        types[(int)type].orbbullets[count].transform.rotation = Quaternion.Euler(0, 0, _z);
        types[(int)type].orbbullets[count].gameObject.SetActive(true);
        types[(int)type].orbbullets[count].SetBullet(Target);
    }

    public void ORB_Take_Damage(Collider2D other)
    {
        if (other.CompareTag("ENEMY"))
        {
            BossCtrl boss = other.GetComponent<BossCtrl>();
            boss.Enemy_CurHp -= 1;
            if (boss.Enemy_CurHp < 0) boss.Enemy_CurHp = 0;
        }

        if (other.CompareTag("ENEMY_MOB"))
        {
            EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();
            enemy.Enemy_CurHp -= 1;
            if (enemy.Enemy_CurHp < 0) enemy.Enemy_CurHp = 0;

        }


    }

    public void ORB_Take_Damage_Shield(Collider2D other)
    {
        BossCtrl boss = other.GetComponent<BossCtrl>();
        boss.CurShield_MP -= 1;

    }
}
