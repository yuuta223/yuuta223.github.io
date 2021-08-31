using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Source : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float bulletSpeed;
    public float lifetime;
    public bool wayshot = false;
    // Start is called before the first frame update
    void Awake()
    {
        wayshot = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        transform.Translate(-bulletSpeed, 0, 0);

        if (lifetime < 0)
        {
            wayshot = true;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        if(lifetime < -0.03)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
