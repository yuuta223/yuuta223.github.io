using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgmClip;
    bool oneshot = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void BGMStart()
    {
        audioSource.Play();
        Debug.Log("BGMçƒê∂");
    }
    
    public void BGMStop()
    {
        audioSource.Stop();
        Debug.Log("BGMí‚é~");
    }
}
