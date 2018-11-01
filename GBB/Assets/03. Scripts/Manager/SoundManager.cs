using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
    public AudioClip Sound_icon_cancel;
    public AudioClip Sound_icon_click;
    public AudioClip Sound_map_clear;
    public AudioClip Sound_map_Entry;
    public AudioClip Sound_map_Exit;
    public AudioClip nomal_bullet;
    public AudioClip Shotgun_bullet;
    public AudioClip spiral_bullet;
    public AudioClip following_bullet;
    public AudioClip boss_damage;
    public AudioClip boss_die;
    public AudioClip boss_grap;
    public AudioClip boss_shield_on;
    public AudioClip boss_shield_broken;
    public AudioClip boss_worp;

    public AudioClip EMP;
    public AudioClip Sound_monster_normal_attack;
    public AudioClip Sound_monster_normal_die;
    public AudioClip Sound_player_attack_hit;
    public AudioClip Sound_player_damaged_1;
    public AudioClip Sound_player_die;
    public AudioClip Sound_player_shield_guard;
    public AudioClip Sound_player_shield_parrying;
    public AudioClip Sound_player_shield_break;
    public AudioClip Sound_player_shield_use;
    public AudioClip Sound_tutorial_script_click;
    public AudioClip tutorial_sound;
    public AudioClip lobby_sound;
    public AudioClip Gameplay_sound;





    AudioSource myAudio;

    public bool loop = true;
    private float _Time = 0;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
        
        StartCoroutine("BGMplay");
    }

    public IEnumerator BGMplay()
    {
        Play_Sound(tutorial_sound);
        while (loop && _Time < 14.0f)
        {
            _Time += Time.unscaledDeltaTime;
            
            if(_Time >= 14.0f)
            {
                Play_Sound(tutorial_sound);
                _Time = 0;
            }
            
            yield return null;
        }
    }
    public void Play_Sound(AudioClip playsound)
    {
        myAudio.PlayOneShot(playsound);
    }
}
