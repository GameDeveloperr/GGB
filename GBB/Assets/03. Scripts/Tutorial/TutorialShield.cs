using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShield : MonoBehaviour {

    public bool Parrying = false;
    public SpriteRenderer ShiledImage;
    public GameObject ParryingObj;
    //
    public bool Check = false;
    public int CheckCount = 0;
    public bool CanParrying = false;
    public bool FirstParrying = true;
    //
    public FirstParryingCheck ParryingCircle; 

    private float time = 0;

    void OnEnable()
    {
        if (ParryingCircle.ParryingCount == 0)
        {
            Time.timeScale = 1.0f;
        }
        time = 0;
        ParryingObj.SetActive(false);
    }

    void Update()
    {
        time += Time.deltaTime;
    }

    void OnDisable()
    {
        if (Parrying)
        {
            ParryingOff();
        }
        ParryingObj.SetActive(true);
    }

    IEnumerator ParryingOn()
    {
        if (CanParrying)
        {
            GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_shield_parrying);

            Parrying = true;
            ShiledImage.color = Color.green;
            if(FirstParrying && ParryingCircle.Check)
            {
                ParryingCircle.ParryingCount++;
                FirstParrying = false;
            }

            yield return new WaitForSeconds(1.5f);

            if (Parrying)
            {
                ParryingOff();
            }
        }
    }

    public void ParryingOff()
    {
        Parrying = false;
        ShiledImage.color = Color.white;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 6.0f);
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ENEMYBULLET"))
        {
            if(CheckCount == 2) { Check = true; }
            if (CheckCount == 3) { CanParrying = true; }
           // if(ParryingCircle.parrying_)
            CheckCount++;
            if (time < 0.05f)
            {
                StartCoroutine(ParryingOn());
            }
            GameManager.Instance._PlayerU.SetSkillFill(GameManager.Instance._PlayerS.AddSkillGage(10));
            
            if (Parrying)
            {
                other.transform.rotation = Quaternion.Euler(other.transform.rotation.x, other.transform.rotation.y, other.transform.rotation.z + 180.0f);
                other.GetComponent<TutorialEnemyBullet>().Payyied();
            }
            else
            {
                GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_shield_guard);

                GameManager.Instance._PlayerU.DeleteShieldHP();
                Destroy(other.gameObject);
            }
        }
    }
}
