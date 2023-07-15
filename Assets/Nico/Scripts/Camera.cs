using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    private float height;
    [SerializeField]
    private float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            transform.position = new Vector3(0, height, player.transform.position.z - offsetZ);
        }
    }
}
