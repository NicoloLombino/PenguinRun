using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController cc;
    AudioSource music;

    public GameManager gm;
    public bool isGamePC;

    [Header("movement")]
    public float speed;
    public float jumpForce;
    private float jumpF;
    public bool canJump;

    [Header("stats")]
    public int health = 3;
    public int healthMax = 3;
    public float invincibleTimer;
    public bool isInvincible;
    public int snowballsNum = 3;
    public int coins = 0;

    [Header("component")]
    public Transform snowHole;
    public GameObject snowballPrefab;
    public GameObject invincibleLight;

    [Header("mobile component")]
    Vector3 moveDirection;
    Touch touch;
    Vector3 direction;
    Vector3 initPos;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGamePC)
        {
            speed = 10;
            cc.Move(transform.right * Input.GetAxis("Horizontal") * speed * Time.smoothDeltaTime - Vector3.up * 9.81f * Time.deltaTime + Vector3.up * jumpF * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.W) && canJump)
            {
                canJump = false;
                jumpF = jumpForce;
                StartCoroutine(JumpTimer());
            }

            if (Input.GetKeyDown(KeyCode.S) && snowballsNum >= 1 /*&& !isInvincible*/)
            {
                GameObject snowball = Instantiate(snowballPrefab, snowHole.position, snowHole.rotation);
                snowballsNum--;
                gm.snowballText.text = "SnowBalls: " + snowballsNum.ToString();
            }
        }
        else
        {
            speed = 0.1f;
            cc.Move(transform.right * direction.x * speed * Time.smoothDeltaTime - Vector3.up * 9.81f * Time.deltaTime + Vector3.up * jumpF * Time.deltaTime);

            if (Input.touchCount > 0)
            {

                touch = Input.GetTouch(0);
                initPos = touch.position;
                /*
                if (touch.phase == TouchPhase.Began)
                {
                    initPos = touch.position;
                }
                */
                if (touch.phase == TouchPhase.Moved)
                {
                    direction = touch.deltaPosition;
                    //if(direction.x < gameObject.transform.position.x)
                    //{
                    //    cc.Move(transform.right)
                    //}
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    if (touch.position.y > initPos.y + 1)
                    {
                        if (canJump)
                        {
                            canJump = false;
                            jumpF = jumpForce;
                            StartCoroutine(JumpTimer());
                        }
                    }
                    else if (touch.position.y <= initPos.y - 1)
                    {
                        if (snowballsNum >= 1)
                        {
                            GameObject snowball = Instantiate(snowballPrefab, snowHole.position, snowHole.rotation);
                            snowballsNum--;
                            gm.snowballText.text = "SnowBalls: " + snowballsNum.ToString();
                        }
                    }
                }

                else { direction = Vector3.zero; }

                //moveDirection = new Vector3(touch.position.x - initPos.x,0, 0);

                

            }
        }


        //transform.position = new Vector3(transform.position.x, transform.position.y, -25);        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
        }
        */
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Collisione con un muro");
            health--;
            GameObject effect = Instantiate(collision.gameObject.GetComponent<SpawnObjMovement>().particles, gameObject.transform.position, Quaternion.identity);
            //collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            GameObject blockaudio = Instantiate(collision.gameObject.GetComponent<SpawnObjMovement>().sound.gameObject);
            foreach(Transform child in collision.transform)
            {
                foreach(BoxCollider col in child.GetComponents<BoxCollider>())
                {
                    col.enabled = false;
                }
            }
            Destroy(collision.gameObject,0.2f);
            Destroy(effect, 1);
            //gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gm.InvinciblePlayer();
            gm.healthText.text = "Health: " + health.ToString();

            if (health >= 1)
            {
                StartCoroutine(InvincibleTimer(invincibleTimer));
            }

            else
            {
                // Death
                StartCoroutine(Death(2.5f));
                Destroy(gameObject, 3f);
                gm.isInGame = false;
                gm.gameSpeed = 0;
            }
            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            canJump = true;
        }

        else if(other.gameObject.tag == "Coin")
        {
            AddCoins(gm.coinsNum);
            other.gameObject.GetComponent<SpawnObjMovement>().sound.Play();
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(other.gameObject,0.1f);
        }

        else if (other.gameObject.tag == "SnowBall")
        {
            AddSnow();
            other.gameObject.GetComponent<SpawnObjMovement>().sound.Play();
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(other.gameObject,0.1f);
        }

        else if (other.gameObject.tag == "PowerUp")
        {
            gm.InvinciblePlayer();
            other.gameObject.GetComponent<SpawnObjMovement>().sound.Play();
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(InvincibleTimer(invincibleTimer));
            Destroy(other.gameObject, 0.1f);
        }

        else if (other.gameObject.tag == "Sea")
        {
            health = 0;
            StartCoroutine(Death(2.5f));
            Destroy(gameObject, 3f);
            gm.isInGame = false;
            gm.gameSpeed = 0;
        }
        /*
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("AAAA");
            gm.InvinciblePlayer(invincibleTimer);
        }
        */
    }
    
    private IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(1);
        jumpF = 0;
    }

    private IEnumerator InvincibleTimer(float timer)
    {
        isInvincible = true;
        invincibleLight.SetActive(true);
        yield return new WaitForSeconds(timer);
        isInvincible = false; ;
        invincibleLight.SetActive(false);
    }

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
        gm.coinsText.text = "Coins: " + coins.ToString();

        if(coins >= 7)
        {
            health++;
            coins -= 7;
        }
    }

    public void AddSnow()
    {
        snowballsNum++;
        gm.snowballText.text = "SnowBalls: " + snowballsNum.ToString();
    }

    public IEnumerator Death(float timerToDeath)
    {
        yield return new WaitForSeconds(timerToDeath);
        gm.gameoverPanel.SetActive(true);
    }
}
