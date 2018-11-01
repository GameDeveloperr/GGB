using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstParryingCheck : MonoBehaviour {

    public int ParryingCount = 0;

    public bool Check = false;
    //public bool parrying_ = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ENEMYBULLET"))
        {
            Time.timeScale = 0.0f;
            Check = true;
        }
    }
}
