using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct P_State
{
    public int MAX_HP;
    public int CUR_HP;
    public int MAX_ShieldHP;
    public int Shiled_HP;
    public int Skill_gage;
    //public int Fever_gage;
}


public class PlayerStat : MonoBehaviour {

    private P_State p_state;

    void Awake()
    {
        p_state.MAX_HP = 5;
        p_state.CUR_HP = p_state.MAX_HP;
        p_state.MAX_ShieldHP = 5;
        p_state.Shiled_HP = p_state.MAX_ShieldHP;
        p_state.Skill_gage = 0;
        //p_state.Fever_gage = 0;
    }
    public void setFirst()
    {
        p_state.MAX_HP = 5;
        p_state.CUR_HP = p_state.MAX_HP;
        p_state.MAX_ShieldHP = 5;
        p_state.Shiled_HP = p_state.MAX_ShieldHP;
        p_state.Skill_gage = 0;
        GameManager.Instance._PlayerU.SetHP();
        GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.NONE;
    }

    public int getMAXHP()
    {
        return p_state.MAX_HP;
    }
    public int getCURHP()
    {
        return p_state.CUR_HP;
    }
    public void removeCURHP(int damage)
    {
        p_state.CUR_HP -= damage;
        if(p_state.CUR_HP <= 0)
        {
            GameManager.Instance._PlayerC.Die();
        }
    }
    public int getMAXShieldHP()
    {
        return p_state.MAX_ShieldHP;
    }
    public int getShiledHP()
    {
        return p_state.Shiled_HP;
    }
    public void RemoveShiledHP(int damage)
    {
        if (p_state.Shiled_HP > 0)
        {
            p_state.Shiled_HP -= damage;
        }
    }
    public void AddHP(int hp)
    {
        if(p_state.CUR_HP < p_state.MAX_HP)
        {
            p_state.CUR_HP += hp;
        }
    }
    public void AddShiledHP(int hp)
    {
        if (p_state.Shiled_HP < p_state.MAX_ShieldHP)
        {
            p_state.Shiled_HP += hp;
        }
    }
    public int getSkillgage()
    {
        return p_state.Skill_gage;
    }
    public int AddSkillGage(int gage)
    {
        p_state.Skill_gage += gage;
        if(p_state.Skill_gage >= 100)
        {
            p_state.Skill_gage = 100;
        }
        return p_state.Skill_gage;
    }

    //public int getFeverGage()
    //{
    //    return p_state.Fever_gage;
    //}
    //public int AddFeverGage(int gage)
    //{
    //    p_state.Fever_gage += gage;
    //    if (p_state.Fever_gage >= 100)
    //    {
    //        ResetFever_gage();
    //        //GameManager.Instance._PlayerU.SetFeverText(getFeverGage());
    //        //GameManager.Instance._PlayerC.OnFever();
    //    }
    //    return p_state.Fever_gage;
    //}
    //public void ResetFever_gage()
    //{
    //    p_state.Fever_gage = 0;
    //}

    public void ResetSkill_gage()
    {
        p_state.Skill_gage = 0;
    }
}
