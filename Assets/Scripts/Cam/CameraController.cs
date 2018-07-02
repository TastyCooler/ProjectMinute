using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    PlayerController player;
    Vector3 velocity;

    [SerializeField] float camDelay = 1;

    // Use this for initialization
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 targetPosition = player.Cursor.transform.TransformPoint(new Vector3(0, 0, 0));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, camDelay);
    }
}
