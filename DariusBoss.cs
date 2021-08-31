using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DariusBoss : Boss
{
    [SerializeField]
    private Text dariusWarnigText;
    [SerializeField]
    private Text[] directingText = new Text[3];

    [SerializeField]
    string[] directingTextString = {"","","" };
    [SerializeField]
    [Range(0.001f, 1f)]
    float intervalForDisplay;
    float intervalforDisplayStock;
    int displayCount;
    int currentLine = 0;


    private SpriteRenderer fadeImage;

    float alfa;
    [SerializeField][Range(0,0.1f)]
    float fadeSpeed;
    [SerializeField]
    float preparationTime = 20.0f;
    [SerializeField]
    float warningDisplayTime;
    float warningDisplayTimeStock;
    [SerializeField]
    float warningHideTime;
    float warningHideTimeStock;

    bool preparationStart;
    bool isFadeOut = false;
    bool isFadeIn = false;

    public GameObject normalBullet;
    public GameObject inductionBullet;
    public GameObject way_SourceBullet;
    GameObject way_Sourceobj;


    public GameObject way_Bullet;
    public GameObject target;

    public float tyuusinndann_x;
    public float tyuusinndann_y;
    public float vx;
    public float vy;
    public float speed;         //弾速
    public float ougikakudo;    //発射する角度 弾と弾の間の角度
    public float n_Way;         //弾数
    public float rad;           //弾数が奇数か偶数かでずらす角度

    public float theta;         //弾と弾の角度
    float pi = Mathf.PI;
    public float AngleRange;    //ラジアンに変換
    public float c, s;


    //弾の構造体
    [System.Serializable]
    private struct ShotPattern
    {
        public float shotDelayTime;
        public float shotDelay;
        public int numberBullet;
        public Vector2[] BulletPosition;
    }

    //通常弾
    [SerializeField]
    ShotPattern normalShot = new ShotPattern()
    {
        shotDelayTime = 1.0f,
        shotDelay = 0,
        numberBullet = 1,
        BulletPosition = new Vector2[1]
    };

    //誘導弾
    [SerializeField]
    ShotPattern inductionShot = new ShotPattern()
    {
        shotDelayTime = 0.5f,
        shotDelay = 0f,
        numberBullet = 4,
        BulletPosition = new Vector2[4]
    };

    //拡散弾
    [SerializeField]
    ShotPattern wayShot = new ShotPattern()
    {
        shotDelayTime = 2.0f,
        shotDelay = 0f,
        numberBullet = 4,
        BulletPosition = new Vector2[4]
    };


    // Start is called before the first frame update
    void OnEnable()
    {

        limitAreaMax = new Vector2(transform.position.x, 3.5f);
        limitAreaMin = new Vector2(transform.position.x, -3.5f);
        
        fadeImage = GameObject.Find("FadeImage").GetComponent<SpriteRenderer>();

        dariusWarnigText.text = "WARNING !";

        for(int i = 0;i < directingText.Length; i++)
        {
            directingText[i].text = directingTextString[i];
            directingText[i].enabled = false;
        }

        setumeiText.text = "「ダライアス」からZゾーンのクジラを参考に\n通常弾に加え誘導弾、拡散弾が追加\n参考作品の実質ラスボスということもあり\n難易度はかなり高め";
        dariusWarnigText.enabled = false;
        preparationStart = false;
        alfa = 0.0f;

        normalShot.shotDelay = normalShot.shotDelayTime;
        inductionShot.shotDelay = inductionShot.shotDelayTime;
        wayShot.shotDelay = wayShot.shotDelayTime;
        
        warningDisplayTimeStock = warningDisplayTime;
        warningHideTimeStock = warningHideTime;
        intervalforDisplayStock = intervalForDisplay;
        HP = 50;
        Speed = 0.1f;

        bGMManager.audioSource.clip = bgmClip;
       
    }

    private void FixedUpdate()
    {
        //警告文表示開始
        if (!play && !preparationEnd && preparationStart)
        {
            preparationTime -= Time.deltaTime;
           


            if (isFadeOut) { 
                fadeImage.color = new Color(0, 0, 0, alfa);
                alfa += fadeSpeed;
                if (alfa >= 1)
                {
                    isFadeOut = false;
                }
            }
            else if (isFadeIn)
            {
                fadeImage.color = new Color(0, 0, 0, alfa);
                alfa -= fadeSpeed;
                if(alfa <= 0)
                {
                    isFadeIn = false;
                }
            }



            if(warningDisplayTime < 0)
            {
                dariusWarnigText.enabled = false;
                warningHideTime -= Time.deltaTime; 
                if(warningHideTime < 0)
                {
                    dariusWarnigText.enabled = true;
                    warningDisplayTime = warningDisplayTimeStock;
                    warningHideTime = warningHideTimeStock;
                }
            }
            else
            {
                warningDisplayTime -= Time.deltaTime;
            }




            intervalForDisplay -= Time.deltaTime;
            //警告文表示
            if (intervalForDisplay < 0 && currentLine < directingTextString.Length)
            {
                if (displayCount < directingTextString[currentLine].Length + 1)
                {
                    directingText[currentLine].text = directingTextString[currentLine].Substring(0, displayCount);
                    displayCount++;
                    intervalForDisplay = intervalforDisplayStock;
                }
                else
                {
                    displayCount = 0;
                    currentLine++;
                }
            }



            //�{�X�O���o�I�������B�������\�����I����Ă����莞�Ԍ�Ƀt�F�[�h�C�����Ăł�����ǂ���
            if(preparationTime < 2 && !preparationEnd && dariusWarnigText.enabled)
            {
                isFadeIn = true;
                dariusWarnigText.enabled = false;
                for (int i = 0; i < directingText.Length; i++)
                {
                    directingText[i].enabled = false;
                }
                
            }
            //�{�X�o�����I�������
            else if(preparationTime < 0 && !preparationEnd){
                preparationEnd = true;
                bGMManager.BGMStart();
            }
        }
        //プレイ開始
        else if(play && preparationEnd && preparationStart)
        {
            
            
            transform.position = new Vector2(transform.position.x, transform.position.y + Speed);

            if (transform.position.y > limitAreaMax.y || transform.position.y < limitAreaMin.y)
            {
                Speed *= -1;
            }

            //各弾パターンの間隔
            normalShot.shotDelay -= Time.deltaTime;
            inductionShot.shotDelay -= Time.deltaTime;
            wayShot.shotDelay -= Time.deltaTime;

            //間隔以下になったら各弾パターン発射
            if(normalShot.shotDelay < 0){
                NormalShotAction();
                normalShot.shotDelay = normalShot.shotDelayTime;
            }
             if(inductionShot.shotDelay < 0){
                InductionShotAction();
                inductionShot.shotDelay = inductionShot.shotDelayTime;
            }
            if(wayShot.shotDelay < 0)
            {
                way_Sourceobj = Instantiate(way_SourceBullet, transform.position, Quaternion.identity);
                wayShot.shotDelay = wayShot.shotDelayTime;
            }
            if(way_Sourceobj.GetComponent<Way_Source>().wayshot)
            {
                WayShotAction();
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PreparationAction()
    {
        
        base.PreparationAction();
        
        audioSource.PlayOneShot(audioClip[1]);
        isFadeOut = true;
        dariusWarnigText.enabled = true;
        for(int i = 0;i < directingText.Length; i++)
        {
            directingText[i].enabled = true;
        }
        preparationStart = true;
        //html�̐F�w�肾�Ǝd�l�ʂ�ɂ����Ȃ������B�w��̍s�̊Ԃ�html�̎w������ނ悤�Ƀv���O�������邱�Ƃ��\�B����͎��Ԃ��Ȃ��̂łł��邱�Ƃ�D��
        directingTextString[0] = "A HUGE BATTLESHIP";
        directingTextString[1] = "GREAT THING - Z";
        directingTextString[2] = "IS APPROACHING FAST.";
    }

    private void NormalShotAction(){
        normalShot.BulletPosition[0] = new Vector2(this.transform.position.x - width / 2, transform.position.y);
        Instantiate(normalBullet, normalShot.BulletPosition[0], Quaternion.identity);
    }

    private void InductionShotAction(){
        inductionShot.BulletPosition[0] = new Vector2(this.transform.position.x - 1, this.transform.position.y + height/2);
        inductionShot.BulletPosition[1] = new Vector2(this.transform.position.x + 1, this.transform.position.y + height/2);
        inductionShot.BulletPosition[2] = new Vector2(this.transform.position.x - 1, this.transform.position.y - height/2);
        inductionShot.BulletPosition[3] = new Vector2(this.transform.position.x + 1, this.transform.position.y - height/2);
        for (int i = 0; i < inductionShot.BulletPosition.Length; i++){
        Instantiate(inductionBullet,inductionShot.BulletPosition[i], Quaternion.identity);
        }
        Debug.Log ("誘導弾発射");
    }

    private void WayShotAction()
    {
        Debug.Log("拡散弾発射");
        tyuusinndann_x = target.transform.position.x - way_Sourceobj.transform.position.x;
            tyuusinndann_y = target.transform.position.y - way_Sourceobj.transform.position.y;
            AngleRange = pi / 180 * ougikakudo;

            //弾数が奇数か偶数か
            if (n_Way % 2 == 0)
            {
                //偶数なら
                rad = (-n_Way / 2 + 0.5f) * AngleRange;
            }
            else
            {   //奇数なら
                rad = (-n_Way / 2 + 0.5f) * AngleRange;
            }

            for (int i = 0; i < n_Way; i++, rad += AngleRange)
            {



                c = Mathf.Cos(rad);
                s = Mathf.Sin(rad);
                vx = tyuusinndann_x * c - tyuusinndann_y * s;
                vy = tyuusinndann_x * s + tyuusinndann_y * c;


                GameObject bulletobj = Instantiate(way_Bullet, way_Sourceobj.transform.position, transform.rotation);
                Way_Bullet bulletSc = bulletobj.GetComponent<Way_Bullet>();
                bulletSc.kakudo = rad;
                bulletSc.speed = speed;
                bulletSc.mannnaka_x = tyuusinndann_x;
                bulletSc.mannnaka_y = tyuusinndann_y;
                bulletSc.vx = this.vx;
                bulletSc.vy = this.vy;
            }
        
    }


}
