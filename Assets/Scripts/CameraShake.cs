using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Script, shaking the camera if the shake amount is set to anything above 0
/// </summary>
public class CameraShake : MonoBehaviour
{

    #region Fields

    public Transform camTransform; // Transform of the camera to shake. Grabs the gameObject's transform if null

    public float shakeDuration = 0f; // How long the camera should shake for.

    public float shakeAmount = 0.7f; // Amplitude of the shake. A larger value shakes the camera harder.
    public float decreaseFactor = 1.0f; // A larger value will make the camera stop shaking earlier

    Vector3 originalPos;

    #endregion

    void Awake()
    {
        // Get the camera transform
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    // Get the original position at the time, the camera started shaking
    void OnEnable()
    {
        // Get the original position of the camera
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        // Offsets the camera in a certain spherical range
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            // Resets the camera to the original position
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}