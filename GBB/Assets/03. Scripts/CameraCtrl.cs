using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    public Transform _Player;

    private float _Dist = -5.0f;
	void Start () {
        transform.position = new Vector3(_Player.position.x, _Player.position.y, _Dist);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(_Player.position.x, _Player.position.y, _Dist);
    }
}
