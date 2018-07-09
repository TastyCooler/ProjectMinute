using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    PlayerController player;
    Vector3 velocity;

    [SerializeField] float camDelay = 1;
    [SerializeField] float titleCamDelay = 3f;
 
    [SerializeField] Transform[] cameraPathForTitle;
    int targetTransform = 0;
    bool isCameraInTitleMode = true;

    [SerializeField] TitlescreenController titleScreenController;

    // Use this for initialization
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        titleScreenController.OnGameStarted += OnGameStarted;
	}
	
    void OnGameStarted()
    {
        isCameraInTitleMode = false;
    }

	// Update is called once per frame
	void LateUpdate () {
        if(!isCameraInTitleMode)
        {
            if(titleCamDelay > camDelay)
            {
                titleCamDelay -= 0.05f;
            }
            else if(titleCamDelay != camDelay)
            {
                titleCamDelay = camDelay;
            }
            Vector3 targetPosition = player.Cursor.transform.TransformPoint(new Vector3(0, 0, 0));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, titleCamDelay);
        }
        else
        {
            Vector3 targetPosition = cameraPathForTitle[targetTransform].TransformPoint(new Vector3(0, 0, 0));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, titleCamDelay);
            if (HelperMethods.V3Equal(transform.position, cameraPathForTitle[targetTransform].position, 50f))
            {
                if(targetTransform < cameraPathForTitle.Length -1)
                {
                    targetTransform++;
                }
                else
                {
                    targetTransform = 0;
                }
            }
        }
    }
}
