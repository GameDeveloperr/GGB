using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonCtrl : MonoBehaviour {
    public Canvas Palyer_Can;
    public GameObject Controll_Menu;

    public Image[] tuto_img = new Image[3];
    public Button tuto_btn;
    public Text tuto_txt;

    private bool check = false;

    public void PresStart()
    {
        SceneManager.LoadScene("Stage_Select");
    }

    public void TutoOn()
    {
        for(int i = 0; i < tuto_img.Length; ++i)
        {
            tuto_img[i].enabled = false;
        }
        tuto_btn.enabled = false;
        tuto_txt.enabled = false;
    }
    public void TutoOff()
    {
        for (int i = 0; i < tuto_img.Length; ++i)
        {
            tuto_img[i].enabled = true;
        }
        tuto_btn.enabled = true;
        tuto_txt.enabled = true;
    }

    public void Skill()
    {
        if(GameManager.Instance._PlayerS.getSkillgage() >= 100)
        {
            GameManager.Instance._PlayerC.Skill();
            GameManager.Instance._PlayerS.ResetSkill_gage();
            GameManager.Instance._PlayerU.SetSkillFill(GameManager.Instance._PlayerS.getSkillgage());

            if(GameManager.Instance.curStage == Stage.Tutorial && GameManager.Instance.T_Manager != null)
            {
                Time.timeScale = 1.0f;
            }
        }
    }
    
    public void Controll_Btn()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_icon_click);

        if (!check) {
            Palyer_Can.enabled = false;
            Time.timeScale = 0.0f;
            Controll_Menu.SetActive(true);
            if(GameManager.Instance.curStage.Equals(Stage.Tutorial)) TutoOn();
            check = true;
        }
    }
    public void ETCOff()
    {
        if(check)
        {
            Controll_Menu.SetActive(false);
            if (GameManager.Instance.curStage.Equals(Stage.Tutorial)) TutoOff();
            Palyer_Can.enabled = true;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            check = false;
        }
    }

    public void Resume()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_icon_click);

        Controll_Menu.SetActive(false);
        Palyer_Can.enabled = true;
        Time.timeScale = 1.0f;
        check = false;
        RePlay();
    }

    public void Exit()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_map_Exit);
        Application.Quit();
    }

    public void RePlay()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_map_Entry);
        Time.timeScale = 1.0f;
        GameManager.Instance.Choose_Stage(GameManager.Instance.curStage);
    }


    public void Stage_Tutorial_btn()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_icon_click);

        Time.timeScale = 1.0f;

        GameManager.Instance.curStage = Stage.Tutorial;
        GameManager.Instance.Choose_Stage(GameManager.Instance.curStage);

    }

    public void Stage1_btn()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_icon_click);

        Time.timeScale = 1.0f;
        GameManager.Instance.curStage = Stage.Stage1;
        GameManager.Instance.Choose_Stage(GameManager.Instance.curStage);

    }
    public void Stage2_btn()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_icon_click);

        Time.timeScale = 1.0f;

        GameManager.Instance.curStage = Stage.Stage2;
        GameManager.Instance.Choose_Stage(GameManager.Instance.curStage);

    }

    public void NextStage_btn()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_icon_click);

        Time.timeScale = 1.0f;

        GameManager.Instance.curStage++;
        GameManager.Instance.Choose_Stage(GameManager.Instance.curStage);
    }

    public void SelectStage_btn()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_icon_click);

        Time.timeScale = 1.0f;

        GameManager.Instance.curStage= Stage.None;
        GameManager.Instance.Choose_Stage(GameManager.Instance.curStage);
    }


}
