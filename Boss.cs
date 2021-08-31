using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField]
    protected GameObject bulletPrefab;
    protected BoxCollider2D bossCollider;

    [SerializeField]
    protected Sprite bossSprite;
    [SerializeField]
    protected SpriteRenderer bossRender;

    protected struct Parameta { 
    }
    [SerializeField]
    public int HP;
    [SerializeField][Range(0, 1)]
    protected float Speed;
    
    protected float width;
    [SerializeField]
    protected float height;
    public bool preparationEnd;
    public bool play;

    [SerializeField]
    protected Text setumeiText;

    protected Vector2 limitAreaMax;
    protected Vector2 limitAreaMin;
    
    [SerializeField]
    protected UIManager uIManager;

    public BGMManager bGMManager;
    public AudioSource audioSource;
    public AudioClip bgmClip;
    public AudioClip[] audioClip;

    // Start is called before the first frame update

    void Awake()
    {
        bossCollider = this.GetComponent<BoxCollider2D>();
        bossRender = GetComponentInChildren<SpriteRenderer>();
        setumeiText = GameObject.Find("SetumeiText").GetComponent<Text>();
        preparationEnd = false;
        play = false;
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        bGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update() {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            audioSource.PlayOneShot(audioClip[0]);
            HP--;
            Destroy(collision.gameObject);
        }
    }

    

    public void ShowText()
    {
        
        bossRender.sprite = bossSprite;
        width = bossRender.bounds.size.x;
        height = bossRender.bounds.size.y;
        setumeiText.enabled = true;
    }

    public void PlayTrue()
    {
        play = true;
    }

    public void PlayEnd()
    {
        uIManager.GameClear();
        play = false;
        bossRender.enabled = false;
    }

    public virtual void Shot()
    {

    }

    public virtual void PreparationAction()
    {
        play = false;
        preparationEnd = false;
        setumeiText.enabled = false;
    }
}
