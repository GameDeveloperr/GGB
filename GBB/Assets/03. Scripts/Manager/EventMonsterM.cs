using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventMonsterM : MonoBehaviour {

    public Text CountText;
    public Text CreateCountText;

    public Text DeathCount;
    public int KillCount = 0;

    public Transform[] CreateType = new Transform[3];

    public GameObject Tiles;
    public List<Transform> Holls = new List<Transform>();
    public Transform[] CreatePos = new Transform[24];

    private int MinCount = 10;
    private int MaxCount = 0;
    public List<Transform> CheckCount;

    private void Start()
    {
        for(int i = 0; i < 24; ++i)
        {
            CreatePos[i] = Tiles.transform.GetChild(i);
        }

        CountText.text += KillCount.ToString();
        MaxCount = MinCount + 3;
        StartCoroutine(StartCreate());
    }

    IEnumerator StartCreate()
    {
        CreateCountText.gameObject.SetActive(true);
        float _time = 0;
        while(_time <= 3.1f)
        {
            _time += Time.deltaTime;

            CreateCountText.text = ((int)_time).ToString();

            yield return null;
        }
        yield return new WaitForSeconds(0.9f);
        CreateCountText.text = "Start";
        yield return new WaitForSeconds(0.3f);
        CreateCountText.gameObject.SetActive(false);
        CheckCount = CreateMob();//생성 +Count
        StartCoroutine(CheckNext());
    }
    public void KillUp()
    {
        KillCount++;
        CountText.text = "KillCount : " + KillCount.ToString();
        DeathCount.text = CountText.text;
    }
    IEnumerator CheckNext()
    {
        bool AllDie = false;
        while(!AllDie)
        {
            for(int i = 0; i < CheckCount.Count; ++i)
            {
                if (CheckCount[i] != null) break;
                if (i == (CheckCount.Count - 1) && CheckCount[i] == null) AllDie = true;
                yield return null;//킬 카운트 세기 
            }
            yield return new WaitForSeconds(0.5f);
        }
        CheckCount.Clear();
        if(Holls.Count > 0)
        {
            for(int i = 0; i < Holls.Count; ++i)
            {
                Destroy(Holls[i].gameObject);
                yield return null;
            }
            Holls.Clear();
        }
        StartCoroutine(StartCreate());
        Debug.Log("end");
    }

    public List<Transform> CreateMob()
    {
        List<Transform> MobCount = new List<Transform>();

        for(int i = 0; i < 24; ++i)
        {
            int Num = Random.Range(0, 3);
            
            Transform Mob = Instantiate(CreateType[Num], CreatePos[i].position, Quaternion.identity);
            if (Num == 0)
                Holls.Add(Mob);
            else if (Num == 1 || Num == 2)
                MobCount.Add(Mob);
        }

        return MobCount;
    }
}
