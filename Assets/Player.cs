using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController cc;
    public GameManager gm;

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



    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        cc.Move(transform.right * Input.GetAxis("Horizontal") * speed * Time.smoothDeltaTime - Vector3.up * 9.81f * Time.deltaTime + Vector3.up * jumpF * Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.W) && canJump)
        {
            canJump = false;
            jumpF = jumpForce;
            StartCoroutine(JumpTimer());
        }
        //transform.position = new Vector3(transform.position.x, transform.position.y, -25);
        if(Input.GetKeyDown(KeyCode.S) && snowballsNum >= 1 && !isInvincible)
        {
            GameObject snowball = Instantiate(snowballPrefab, snowHole.position, snowHole.rotation);
            snowballsNum--;
            gm.snowballText.text = "SnowBalls: " + snowballsNum.ToString();
        }
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
            Destroy(collision.gameObject);
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
            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag == "SnowBall")
        {
            AddSnow();
            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag == "PowerUp")
        {
            gm.InvinciblePlayer();
            StartCoroutine(InvincibleTimer(invincibleTimer));
            Destroy(other.gameObject);
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
