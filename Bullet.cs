using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(bulletSpeed, 0, 0);

        if(transform.position.x > 10)
        {
            Destroy(gameObject);
        }
    }
}
