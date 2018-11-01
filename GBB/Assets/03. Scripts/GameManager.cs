using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Stage
{
    None = -1, Tutorial, Stage1, Stage2
};

public class GameManager : MonoBehaviour {

    

    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                if(_instance == null)
                {
                    Debug.LogError("There is no active GameManager Object");
                }
            }
            return _instance;
        }
    }


    public PlayerCtrl _PlayerC;
    public PlayerStat _PlayerS;
    public PlayerUI _PlayerU;
    public OrbCtrl _OrbCtrl;
    public Canvas CtrlCan;
    public GameObject ETC;
    public BulletManager _Bullet;
    public Canvas Clear_can;
    public Canvas GameOver_can;
    public TutorialManager T_Manager;
    public SoundManager _SoundManager;
    public Stage1_Manager _Stage1;


    //스테이지 변수 지정
    public Stage curStage = Stage.None;
    //스테이지1 통과 조건 Count

    


    private void Awake()
    {
        //Singleton -- Instance Create
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        //GameStart();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _PlayerU.ShiledBtnDown();
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            _PlayerU.ShiledBtnUp();
        }
    
    }
    public void GameOver()
    {
        _SoundManager.Play_Sound(_SoundManager.Sound_player_die);
        CtrlCan.gameObject.SetActive(false);
        GameOver_can.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        //ETC.gameObject.SetActive(true);
    }

    public void Game_Clear()
    {
        _SoundManager.Play_Sound(_SoundManager.Sound_map_clear);

        Clear_can.gameObject.SetActive(true);
    }

    public void Stage1_Game_Clear()
    {
        if (_Stage1 != null)
        {
            if(_Stage1.Count == 20)
            {
                _Stage1.Cur_Wave++;
                _Stage1.StartCoroutine("Wave",_Stage1.Cur_Wave);
            }
            else if(_Stage1.Count == 10)
            {
                _Stage1.Cur_Wave++;
                _Stage1.StartCoroutine("Wave", _Stage1.Cur_Wave);
            }
            else if (_Stage1.Count == 0)
            {
                Clear_can.gameObject.SetActive(true);
            }
        }
        
    }

    public void Choose_Stage(Stage Stage_num)
    {
        switch (Stage_num)
        {
            case Stage.None:
                SceneManager.LoadScene("Stage_Select");
                break;
            case Stage.Tutorial:
                SceneManager.LoadScene("Tutorial");
                break;
            case Stage.Stage1:
                SceneManager.LoadScene("Play_Mode");
                break;
            case Stage.Stage2:
                SceneManager.LoadScene("Play_Mode 1");
                break;

        }
    }

}
