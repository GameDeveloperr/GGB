using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    //Shiled
    //public Text Shiled_txt;
    //public Sprite[] Shiled_imgs = new Sprite[2];
    public Button Shiled_Btn;
    //
    public GameObject _Shiled;
    //
    public Button SkillBtn;
    public Image Skill_Fill;
    //
    public Slider HP_Slider;
    //
    public Slider Shield_HP_Slider;
    public Text Shiled_HP_Txt;
    //
    private bool ShieldHit = false;
    private bool BodyHit = false;
    //Fever
    //public Text Fevertxt;

    void Start()
    {
        _Shiled.SetActive(false);
       // Shiled_Btn.GetComponent<Image>().sprite = Shiled_imgs[0];
        SetSkillFill(GameManager.Instance._PlayerS.getSkillgage());
        //SetFeverText(GameManager.Instance._PlayerS.getFeverGage());

        SetHP();

        StartCoroutine(RecoveryShieldHP());
        StartCoroutine(RecoveryHP());
    }
    public void SetHP()
    {
        HP_Slider.maxValue = GameManager.Instance._PlayerS.getMAXHP();
        HP_Slider.value = GameManager.Instance._PlayerS.getCURHP();
        Shield_HP_Slider.maxValue = GameManager.Instance._PlayerS.getMAXShieldHP();
        Shield_HP_Slider.value = GameManager.Instance._PlayerS.getShiledHP();
        Shiled_HP_Txt.text = GameManager.Instance._PlayerS.getShiledHP().ToString();
    }

    public void DeleteHP()
    {
        GameManager.Instance._PlayerS.removeCURHP(1);
        HP_Slider.value = GameManager.Instance._PlayerS.getCURHP();
        BodyHit = true;
    }
    public void AddHP()
    {
        GameManager.Instance._PlayerS.AddHP(1);
        HP_Slider.value = GameManager.Instance._PlayerS.getCURHP();
    }

    public void AddShiledHP()
    {
        GameManager.Instance._PlayerS.AddShiledHP(1);
        Shield_HP_Slider.value = GameManager.Instance._PlayerS.getShiledHP();
        Shiled_HP_Txt.text = GameManager.Instance._PlayerS.getShiledHP().ToString();
    }

    public void DeleteShieldHP()
    {
        GameManager.Instance._PlayerS.RemoveShiledHP(1);
        Shield_HP_Slider.value = GameManager.Instance._PlayerS.getShiledHP();
        Shiled_HP_Txt.text = GameManager.Instance._PlayerS.getShiledHP().ToString();
        if(GameManager.Instance._PlayerS.getShiledHP() == 0)
        {
            ShiledBtnUp();
        }
        ShieldHit = true;
    }

    public void ShiledBtnDown()
    {
        if (GameManager.Instance._PlayerS.getShiledHP() > 0)
        {
            //GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_shield_use);

            GetComponent<CapsuleCollider2D>().enabled = false;
            _Shiled.SetActive(true);
            //Shiled_Btn.GetComponent<Image>().sprite = Shiled_imgs[1];
        }
        else
        {
            GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_shield_break);
        }
    }
    public void ShiledBtnUp()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
        //Shiled_Btn.GetComponent<Image>().sprite = Shiled_imgs[0];
        _Shiled.SetActive(false);
    }
    public void SetSkillFill(int gage)
    {
        Skill_Fill.fillAmount = (float)(gage * 0.01);
    }
    //public void SetSkillText(int gage)
    //{
    //    Shiled_txt.text = gage.ToString() + "%";
    //}

    //public void SetFeverText(int gage)
    //{
    //    Fevertxt.text = gage.ToString() + "%";
    //}

    IEnumerator RecoveryShieldHP()
    {
        float time = 0f;
        while (!GameManager.Instance._PlayerC._Die)
        {
            time += Time.deltaTime;
            if (ShieldHit)
            {
                time = 0;
                yield return new WaitForSeconds(0.5f);
                ShieldHit = false;
            }
            if (time >= 3.0f)
            {
                time = 0;
                AddShiledHP();
            }
            yield return null;
        }
    }
    IEnumerator RecoveryHP()
    {
        float _time = 0f;
        while (!GameManager.Instance._PlayerC._Die)
        {
            _time += Time.deltaTime;
            if (BodyHit)
            {
                _time = 0;
                yield return new WaitForSeconds(0.5f);
                BodyHit = false;
            }
            if (_time >= 5.0f)
            {
                _time = 0;
                AddHP();
            }
            yield return null;
        }
    }
}
