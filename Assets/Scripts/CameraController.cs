using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    PlayerController player;
    [SerializeField] float lerpSpeed = 1f;

	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Vector3.Lerp(transform.position, player.Cursor.transform.position, lerpSpeed);
	}
}
