using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(collision.gameObject.GetComponent<SpawnObjMovement>().particles, gameObject.transform.position, Quaternion.identity);
        //collision.gameObject.GetComponent<SpawnObjMovement>().sound.Play();ù
        GameObject blockaudio = Instantiate(collision.gameObject.GetComponent<SpawnObjMovement>().sound.gameObject);
        //collision.gameObject.GetComponentsInChildren<MeshRenderer>().enabled = false;
        //collision.gameObject.GetComponentsInChildren<BoxCollider>().enabled = false;
        Destroy(collision.gameObject, 0.1f);
        Destroy(gameObject);
        
    }
}
