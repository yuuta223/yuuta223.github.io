using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Bullet : MonoBehaviour
{
    public float c,s;
    public float mannnaka_x;
    public float mannnaka_y;
    public float vx;
    public float vy;
    float lifetime = 1.5f;

    public float speed, kakudo;
    // Start is called before the first frame update
    void Start()
    {
        c = Mathf.Cos(kakudo);
        s = Mathf.Sin(kakudo);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lifetime < 0)
        {
            Destroy(this.gameObject);
        }
        this.transform.position += new Vector3(vx * speed, vy * speed,0);
        lifetime -= Time.deltaTime;
    }
}
