using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset -= new Vector2(0.03f, 0.03f) * Time.deltaTime;
    }
}
