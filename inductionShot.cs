using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inductionShot : MonoBehaviour
{
    public GameObject bossObj;
    public GameObject playerObj;
    public float speed;
    public float kyori_x;
    public float kyori_y;
    public float dist;
    public float lifeTime;
    // Start is called before the first frame update
    void Awake()
    {
        bossObj = GameObject.Find("Boss");
        playerObj = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime -= Time.deltaTime;
        dist = Vector3.Distance (playerObj.transform.position,transform.position);
        speed = 0.01f / lifeTime ;
        if(0.5f < lifeTime){
            kyori_x = playerObj.transform.position.x - this.transform.position.x;
            kyori_y = playerObj.transform.position.y - this.transform.position.y;
        }else if(-0.5f > lifeTime){
           Destroy(this.gameObject);
        }
        
        transform.position += new Vector3 (kyori_x * speed, kyori_y * speed,0);
    }
}