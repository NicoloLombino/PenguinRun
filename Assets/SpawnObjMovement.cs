using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjMovement : MonoBehaviour
{
    public GameManager gm;
    public GameObject particles;
    public AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        if(sound == null)
        {
            sound = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, 0, gm.gameSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        GameObject effect = Instantiate(particles, gameObject.transform.position, Quaternion.identity);

    }
}
