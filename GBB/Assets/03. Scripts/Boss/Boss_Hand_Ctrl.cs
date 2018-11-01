using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Hand_Ctrl : MonoBehaviour {

    private void Start()
    {
        GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.boss_grap);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance._SoundManager.Play_Sound(GameManager.Instance._SoundManager.Sound_player_damaged_1);

            if (!GameManager.Instance._PlayerC._Die)
            {
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.DAMAGED;
            }

            GameManager.Instance._PlayerU.DeleteHP();
            GameManager.Instance._PlayerU.DeleteHP();
        }
    }
}
