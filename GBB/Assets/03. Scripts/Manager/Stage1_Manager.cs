using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_Manager : MonoBehaviour {

    public Transform Mob_type1;
    public Transform Mob_type2;

    public Transform[] wave1_pos;
    public Transform[] wave2_pos;
    public Transform[] wave3_pos;


    public Transform Creat_eff;
    public int Cur_Wave = 1;
    public int Count=30;

    private void Start()
    {
        StartCoroutine("Wave", Cur_Wave);
    }

    public IEnumerator Wave(int Wave)
    {
        yield return new WaitForSeconds(3.0f);//다음 스테이지 넘어가기전

        
        switch (Wave)
        {
            case 1:
                for (int i = 0; i < 10; i++)
                {
                    Transform eff = Instantiate(Creat_eff, wave1_pos[i].position, Quaternion.identity);
                    Destroy(eff.gameObject, 1.0f);

                    if (i < 5)
                    {
                        Instantiate(Mob_type1, wave1_pos[i].position,Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Mob_type2, wave1_pos[i].position, Quaternion.identity);
                    }


                }
                break;
            case 2:
               

                for (int i = 0; i < 10; i++)
                {
                    Transform eff = Instantiate(Creat_eff, wave2_pos[i].position, Quaternion.identity);
                    Destroy(eff.gameObject, 1.0f);

                    if (i < 5)
                    {
                        Instantiate(Mob_type1, wave2_pos[i].position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Mob_type2, wave2_pos[i].position, Quaternion.identity);
                    }
                }
                break;
            case 3:

                for (int i = 0; i < 10; i++)
                {
                    Transform eff = Instantiate(Creat_eff, wave3_pos[i].position, Quaternion.identity);
                    Destroy(eff.gameObject, 1.0f);
                    if (i < 5)
                    {
                        Instantiate(Mob_type1, wave3_pos[i].position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Mob_type2, wave3_pos[i].position, Quaternion.identity);
                    }
                }
                break;

        }
        yield return null;

    }
}
