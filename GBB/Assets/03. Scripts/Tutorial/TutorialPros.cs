using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPros : MonoBehaviour {

    public Transform[] Circle = new Transform[6];
    public Transform[] CantGo = new Transform[4];
    public Transform[] InCheck = new Transform[4];
    public Transform Monster;
    public Transform Monster2;
    public GameObject Monsters;
    public TutorialMonster[] CheckMonsters = new TutorialMonster[3];
    public Transform Player_Shield;
    public TextBox tBox;
    public TutorialManager T_Manager;

    public GameObject[] ORB = new GameObject[3];

    bool Clear = false;
    bool Die = false;

    int RoomNum = 0;
    private int Part_Idx = 0;

    //ublic bool TimeCheck = false;
    //public int count = 0;

    public void Part01()
    { 
        Circle[Part_Idx].gameObject.SetActive(true);
        StartCoroutine(CheckNext());
    }
    public void Part02()
    {
        Monster.gameObject.SetActive(true);
        StartCoroutine(MonsterCheck());
    }
    public void Part03()
    {
        StartCoroutine(CheckNext());
    }
    public void Part04()
    {
        Monster2.gameObject.SetActive(true);
        StartCoroutine(ShieldCheck());
    }
    public void Part05()
    {
        Circle[Part_Idx].gameObject.SetActive(true);
        StartCoroutine(CheckNext());
    }
    public void Part06()
    {
        StartCoroutine(DieCheck());
    }
    public void Part07()
    {
        GameManager.Instance._PlayerU.SetSkillFill(GameManager.Instance._PlayerS.AddSkillGage(100));

        StartCoroutine(SkillCheck());
    }
    public void Part08()
    {
        SetOnOrb();
        tBox.gameObject.SetActive(false);
        for (int i = 0; i < CheckMonsters.Length; i++)
        {
            CheckMonsters[i].gameObject.SetActive(true);
        }
        StartCoroutine(FightCheck());
    }
    IEnumerator FightCheck()
    {
        while(!Clear && !Die)
        {
            if (CheckMonsters[0].DieCheck && CheckMonsters[1].DieCheck&& CheckMonsters[2].DieCheck)
            {
                Clear = true;
            }
            if (GameManager.Instance._PlayerS.getCURHP() == 0)
            {
                Die = true;

                for (int i = 0; i < CheckMonsters.Length; i++)
                {
                    CheckMonsters[i].gameObject.SetActive(false);
                }
                T_Manager.StartCoroutine(T_Manager.Loading());
            }
            yield return new WaitForSeconds(0.1f);
        }
        if (!Die)
        {
            yield return new WaitForSeconds(1.0f);
            T_Manager.Clear(); }
        else { Die = false; }
    }
    IEnumerator SkillCheck()
    {
        yield return new WaitForSeconds(0.5f);
        //tBox.gameObject.SetActive(true);
        tBox.txt_field.text = "지금입니다. EMP버튼을 누르세요!";
        Time.timeScale = 0;
        T_Manager.OnCtrlCan();
        while (GameManager.Instance._PlayerS.getSkillgage() >= 100)
        {
            yield return null;
        }
        TutorialMonster[] mon = Monsters.GetComponentsInChildren<TutorialMonster>();
        for(int i = 0; i < mon.Length; ++i)
        {
            mon[i].CurEnemyType = TutorialMonster.EnemyType.None;
        }
        tBox.StartNext();
        //Monsters.SetActive(false);
    }

    IEnumerator DieCheck()
    {
        Monster2.GetComponent<TutorialMonster>().CurEnemyType = TutorialMonster.EnemyType.TUTORIAL;
        while (!Monster2.GetComponent<TutorialMonster>().DieCheck)
        {
            yield return null;
        }
        tBox.StartNext();
    }

    IEnumerator ShieldCheck()
    {
        while (!Player_Shield.GetComponent<TutorialShield>().Check)
        {
            yield return null;
        }
        GameManager.Instance._PlayerU.ShiledBtnUp();
        TutorialMonster mon = Monster2.GetComponent<TutorialMonster>();
        mon.CurEnemyType = TutorialMonster.EnemyType.None;
        tBox.StartNext();
        //Player.GetComponentInChildren<TutorialShield>().Check = false;
    }

    IEnumerator MonsterCheck()
    {
        while(!Monster.GetComponent<TutorialMonster>().Check)
        {
            yield return null;
        }
        tBox.StartNext();
    }

    IEnumerator CheckNext()
    {
        if (Part_Idx != 4)
        {
            while (!Circle[Part_Idx].GetComponent<SuccessCheck>().Check)
            {
                yield return null;
            }
        }
        else if(Part_Idx == 4)
        {
            Monster2.GetComponent<TutorialMonster>().CurEnemyType = TutorialMonster.EnemyType.TUTORIAL;
            Circle[Part_Idx].GetComponent<FirstParryingCheck>().Check = false;
            while (!Circle[Part_Idx].GetComponent<FirstParryingCheck>().Check)
            {
                yield return null;
            }
            Monster2.GetComponent<TutorialMonster>().CurEnemyType = TutorialMonster.EnemyType.None;
            tBox.TouchCheck = true;
        }
        Circle[Part_Idx].gameObject.SetActive(false);
        Part_Idx++;
        tBox.StartNext();
    }

    public void CanGo()
    {
        CantGo[RoomNum].gameObject.SetActive(false);
        StartCoroutine(CheckInNextRoom());
    }

    IEnumerator CheckInNextRoom()
    {
        while(!InCheck[RoomNum].GetComponent<SuccessCheck>().Check)
        {
            yield return null;
        }
        InCheck[RoomNum].GetComponent<SuccessCheck>().Check = false;
        T_Manager._Stage += 1;
        
        tBox.gameObject.SetActive(false);
        T_Manager.StartCoroutine(T_Manager.Loading());
    }

    public void SetOnOrb()
    {
        for(int i = 0; i < 3; i++)
        {
            ORB[i].SetActive(true);
        }
    }
    public void SetOffOrb()
    {
        for (int i = 0; i < 3; i++)
        {
            ORB[i].SetActive(false);
        }
    }
}
