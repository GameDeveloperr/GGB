using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour {

    //
    public bool Parrying = false;
    public SpriteRenderer ShiledImage;
    public GameObject ParryingObj;
    //
    private float time = 0;

	void OnEnable()
    {
        time = 0;
        ParryingObj.SetActive(false);
    }

    void Update()
    {
        time += Time.deltaTime;
    }

    void OnDisable()
    {
        if(Parrying)
        {
            ParryingOff();
        }
        ParryingObj.SetActive(true);
    }

    IEnumerator ParryingOn()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_shield_parrying);

        Parrying = true;
        ShiledImage.color = Color.green;

        yield return new WaitForSeconds(1.5f);

        if(Parrying)
        {
            ParryingOff();
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
        if(other.CompareTag("ENEMYBULLET"))
        {
            if(time < 0.05f)
            {
                StartCoroutine(ParryingOn());
            }
            GameManager.Instance._PlayerU.SetSkillFill(GameManager.Instance._PlayerS.AddSkillGage(10));
            //GameManager.Instance._PlayerU.SetFeverText(GameManager.Instance._PlayerS.AddFeverGage(10));
            if (Parrying)
            {
                other.transform.rotation = Quaternion.Euler(other.transform.rotation.x, other.transform.rotation.y, other.transform.rotation.z + 180.0f);
                other.GetComponent<EnemyBullet>().Payyied();
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
