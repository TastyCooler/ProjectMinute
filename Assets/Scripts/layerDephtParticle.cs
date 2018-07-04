using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layerDephtParticle : MonoBehaviour {

    public float yOffset;
    float layer;
    ParticleSystemRenderer rend;
    Vector3 centerBottom;

    // Use this for initialization
    void Start()
    {

        rend = GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        centerBottom = transform.TransformPoint(rend.bounds.min);

        layer = centerBottom.y + yOffset;

        rend.sortingOrder = -(int)(layer * 10);
    }
}
