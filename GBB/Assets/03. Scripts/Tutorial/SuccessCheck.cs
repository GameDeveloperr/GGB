using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessCheck : MonoBehaviour {

    public bool Check = false;

    public bool Wait = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && this.gameObject.name != "AttackDist")
        {
            GameManager.Instance.T_Manager.T_Proces.CantGo[0].gameObject.SetActive(true);
            Check = true;
        }
        if(this.gameObject.name == "AttackDist" && collision.CompareTag("TUTORIALENEMY"))
        {
            Wait = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.name == "AttackDist" && collision.CompareTag("TUTORIALENEMY"))
        {
            if (!Wait) StartCoroutine(CheckOut());
        }
    }

    IEnumerator CheckOut()
    {
        Wait = true;
        while(Wait)
        {
            yield return new WaitForSeconds(1.5f);
            if (Wait)
            {
                Check = true;
                Wait = false;
            }
        }
    }
}
