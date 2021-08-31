using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradiusBoss : Boss
{
    [SerializeField]
    private Text startCountText;

    private float shotDelayTime;
    private float shotDelay;
    private int numberBullet;

    public Vector2[] BulletPosition;

    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private Vector2 findPlayerPos;
    [SerializeField]
    private float distance;
    private bool finds = false;
    [SerializeField]
    private bool shots = false;


    // Start is called before the first frame update
    void OnEnable()
    {
        limitAreaMax = new Vector2(transform.position.x, 3.5f);
        limitAreaMin = new Vector2(transform.position.x, -3.5f);
        shotDelayTime = 1.0f;
        shotDelay = shotDelayTime;
        numberBullet = 4;
        BulletPosition = new Vector2[numberBullet];
        setumeiText.text = "「グラディウス」からビッグコアの分析\n・移動は上下のみ\n・攻撃は1秒に1回\n・同時に縦4発\n・弾自体の挙動は水平方向に進んでいく\n・中心にコアがあり弾を当てるとダメージ";
        HP = 20;
        Speed = 0.1f;
        playerObj = GameObject.Find("Player");

        bGMManager.audioSource.clip = bgmClip;
    }

    private void FixedUpdate()
    {
        if (play)
        {
           
            shotDelay -= Time.deltaTime;
            if (shotDelay < 0)
            {
                shots = true;
                Debug.Log("ショットフォルス");
                Debug.Log(distance - transform.position.y);
                
                findPlayerPos = playerObj.transform.position;
                distance = findPlayerPos.y - transform.position.y;
                if (distance < 0 && !shots)
                {
                Speed *= -1;
                }



            


                if(shots){

                
                    if (distance - transform.position.y < 0.1f && distance - transform.position.y > -0.1f)
                    {

                        Shot();
                        Debug.Log("ショット");


                    }
                    else {
                        transform.position = new Vector2(transform.position.x, transform.position.y + Speed);
                        Debug.Log ("移動");

                    }
                }


            }

            
               
            
             


            if (transform.position.y > limitAreaMax.y || transform.position.y < limitAreaMin.y)
            {
                Speed *= -1;
            }

            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //部位ごとに判定を取る必要がある
        if (collision.gameObject.tag == "Bullet")
        {
            HP--;
            Destroy(collision.gameObject);
        }
    }

    public override void  Shot()
    {
        for (int i = 0; i < BulletPosition.Length; ++i)
        {
            BulletPosition[i] = new Vector2(transform.position.x - (bossCollider.size.x / 2), transform.position.y + height / 2 - (height / (BulletPosition.Length - 1) * i));
            Instantiate(bulletPrefab, BulletPosition[i], Quaternion.identity);
        }
        shots = false;
        
        shotDelay = shotDelayTime;
    }

    public override void PreparationAction()
    {
        base.PreparationAction();
        StartCoroutine("PreparationCoroutine");
    }

    IEnumerator PreparationCoroutine()
    {
        startCountText.enabled = true;
        startCountText.text = "3";
        yield return new WaitForSeconds(1);
        startCountText.text = "2";
        yield return new WaitForSeconds(1);
        startCountText.text = "1";
        yield return new WaitForSeconds(1);
        startCountText.text = "START";
        yield return new WaitForSeconds(1);
        startCountText.text = "";
        startCountText.enabled = false;
        preparationEnd = true;
        bGMManager.BGMStart();
    }


}
