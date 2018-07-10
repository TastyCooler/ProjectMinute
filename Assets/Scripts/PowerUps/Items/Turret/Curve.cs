using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {

    public Curves curve;
    
    public bool on;

    public float duration;

    private float progress;

    private void Update()
    {
        
        
        if (on)
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                progress = 1f;
                curve = null;
            }
                if (curve) transform.localPosition = curve.GetPoint(progress);
        }
    }
}

