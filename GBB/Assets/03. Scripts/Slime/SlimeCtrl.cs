using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCtrl : MonoBehaviour
{

    private bool Attack = false;
    public Transform Target;
    private float TraceDist = 5.0f;
    private float TraceSpeed = 10.0f;
    private CircleCollider2D _Coll;
    private Animator _Anim;

    public int curHp;
    public bool Die = false;

    // Use this for initialization
    void Start()
    {
        Target = GameManager.Instance._PlayerC.transform;
        _Coll = GetComponent<CircleCollider2D>();
        _Anim = GetComponent<Animator>();
        StartCoroutine(TraceCheck());
    }

    public bool TakeAttack()
    {
        _Anim.SetTrigger("IsAttack");
        return false; 
    }
    IEnumerator GoAhead()
    {
        _Coll.enabled = true;
        bool reach = false;
        Vector2 Look = (Target.position - transform.position).normalized;
        Vector2 Pos = Target.position;
        while(!reach)
        {
            transform.Translate(Look * TraceSpeed * Time.deltaTime);
            if(Vector2.Distance(Pos, transform.position) <= 1.0f)
            {
                reach = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Attack = TakeAttack();
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(1.0f);
        _Coll.enabled = false;
        
        StartCoroutine(TraceCheck());
    }//곂친경우 계속 공격 (오류) 

    IEnumerator TraceCheck()
    {
        while (!Attack)
        {
            float dist = Vector2.Distance(transform.position, Target.position);
            if (dist <= TraceDist)
            {
                Attack = true;
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            yield return null;
        }
        StartCoroutine(GoAhead());
        yield return new WaitForSeconds(1.5f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && Attack)
        {
            if (!GameManager.Instance._PlayerC._Die)
            {
                GameManager.Instance._PlayerC._State = PlayerCtrl.PlayerState.DAMAGED;
            }
            GameManager.Instance._PlayerU.DeleteHP();
        }
    }
    public void Death()
    {
        if (GameManager.Instance.curStage == Stage.EventStage)
            GameManager.Instance.CreatM.KillUp();
        Die = true;
        Destroy(transform.parent.gameObject);
    }
}
