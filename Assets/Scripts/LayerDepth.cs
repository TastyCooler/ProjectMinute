using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDepth : MonoBehaviour {

    public float yOffset;
    public float by; // This is not used?!
    float layer;
    SpriteRenderer rend;
    Vector3 centerBottom;

    // Use this for initialization
    void Start () {

        rend = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        centerBottom = transform.TransformPoint(rend.sprite.bounds.min);

        layer = centerBottom.y + yOffset;

        rend.sortingOrder = -(int)(layer * 10);
    }
}
