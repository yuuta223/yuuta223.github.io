using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject bulletPrefab;
    public BoxCollider2D playerCollider;
    private SpriteRenderer playerRender;
    Vector2 bulletPosition;
    public int HP;
    [Range(0,1)]
     public float speed;    // 速度調整
     [Range(0,1)]
     public float shotDelayTime;
     public float shotDelay;
     public bool oneButton = false;
     public bool play;

     public bool dead;

    private Vector2 limitAreaMax;
    private Vector2 limitAreaMin;

     public UIManager uIManager;

    public AudioSource audioSource;
    public AudioClip[] audioClip;
    // Start is called before the first frame update
    void Start()
    {
       playerCollider = this.GetComponent<BoxCollider2D>();
       playerRender = this.GetComponentInChildren<SpriteRenderer>();
        audioSource = this.GetComponentInChildren<AudioSource>();
        limitAreaMax = new Vector2(2f,4.5f);
        limitAreaMin = new Vector2(-8,-4.5f);
        dead = false;
    }

    private void FixedUpdate()
    {
        if (play)
        {
            if (Input.GetAxis("Submit") > 0
            && oneButton == false)
            {
                oneButton = true;

                Shot();
                //Debug.Log("ショット");
            }
            else if (Input.GetAxis("Submit") > 0 && oneButton == true)
            {
                //Debug.Log("連射中");
                shotDelay -= Time.deltaTime;
                if (shotDelay < 0)
                {
                    Shot();
                    shotDelay = shotDelayTime;
                }

            }
            else if (Input.GetAxis("Submit") < 1 && oneButton == true)
            {
                //Debug.Log("ショットボタン離し");
                shotDelay = shotDelayTime;
                oneButton = false;
            }

            //速さ
            float h = Input.GetAxis("Horizontal") * speed;
            float v = Input.GetAxis("Vertical") * speed;

            if ((transform.position.x < limitAreaMin.x && h < 0) || (transform.position.x > limitAreaMax.x && h > 0))
            {
                h = 0;
                Debug.Log("横移動禁止");
            }
            if((transform.position.y < limitAreaMin.y && v < 0) || (transform.position.y > limitAreaMax.y && v > 0))
            {
                v = 0;
                Debug.Log("縦移動禁止");
            }
           
            // 移動
            transform.position = new Vector2(transform.position.x + h, transform.position.y + v);
        }
        
    }

    private void Update()
    {
        
    }

    public void Shot(){
        audioSource.PlayOneShot(audioClip[0]);
        bulletPosition = new Vector2(transform.position.x + (playerCollider.size.x/2),transform.position.y);
        Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyBulletA")
        {
            uIManager.GameOver();
            Destroy(collision.gameObject);
            dead = true;
            playerRender.enabled = false;
        }
    }

    // Update is called once per frame

}
