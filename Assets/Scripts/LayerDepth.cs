using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDepth : MonoBehaviour {

    public int SortingOrder
    {
        get
        {
            return rend.sortingOrder;
        }
    }

    public float yOffset;
    float layer;
    SpriteRenderer rend;
    Vector3 centerBottom;

    [SerializeField] bool isChild;

    // Use this for initialization
    void Start () {

        rend = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        centerBottom = transform.TransformPoint(rend.sprite.bounds.min);

        layer = centerBottom.y + yOffset;

        if(!isChild)
        {
            rend.sortingOrder = -(int)(layer * 10);
        }
        else if(isChild)
        {
            rend.sortingOrder = transform.parent.GetComponent<LayerDepth>().SortingOrder + 1;
        }
    }
}
