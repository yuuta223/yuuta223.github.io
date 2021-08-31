using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.UIElements;

public class MainGameManager : MonoBehaviour
{

    public GameObject bossObject;
    //�Q�[���̗���
    public enum GameState
    {
        Start,
        Preparation,
        Playing,
        End
    }

    //�Q�[���^�C�v
    public enum GameType
    {
        Gradius,
        Darius,
        RType
    }


    [SerializeField]
    private GameState currentGameState;
    [SerializeField]
    public GameType currentGameType;
    //[SerializeField]
    

    [SerializeField]
    private Player playerScript;
    [SerializeField]
    private Boss BossScript;


    public BGMManager bgmManager;


    // Start is called before the first frame update
    void Start()
    {
        bgmManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        bossObject = GameObject.Find("Boss");
        currentGameType = (GameType)DDObjects.gameType;
        currentGameState = GameState.Start;
        OnGameStateChange(currentGameState);
        
        
        Debug.Log("DDobjGameType:" + DDObjects.gameType);
        Debug.Log("currentGameType" + currentGameType);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;   // UnityEditor�̎��s���~���鏈��
#else
        Application.Quit();                                // �Q�[�����I�����鏈��
#endif
        }

        if(currentGameState == GameState.Start)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetCurrentState(GameState.Preparation);
            }
        }
        else if (currentGameState == GameState.Preparation && BossScript.preparationEnd)
        {
            SetCurrentState(GameState.Playing);
        }
        else if (currentGameState == GameState.Playing)
        {
            if (BossScript.HP <= 0)
            {
                SetCurrentState(GameState.End);
            }
            else if(playerScript.dead && Input.GetKeyDown(KeyCode.R)){
                SceneManager.LoadScene("Title");
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (Time.timeScale == 0.0f)
                {
                    Time.timeScale = 1.0f;
                }
                else
                {
                    Time.timeScale = 0.0f;
                }
            }
        }
        else if(currentGameState == GameState.End)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    //�S�̗̂���Ǘ�
    public void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                StartAction();
                break;
            case GameState.Preparation:
                BossScript.PreparationAction(); //�J�n����
                break;

            case GameState.Playing:
                PlayingAction();
                break;

            case GameState.End:
                EndAction();
                break;
                

        }

    }

    

    //�e�{�X�̐����e�L�X�g�\��
    void StartAction()
    {
        switch (currentGameType)
        {
            case GameType.Gradius:
                BossScript = bossObject.GetComponent<GradiusBoss>();
                break;
            case GameType.Darius:
                BossScript = bossObject.GetComponent<DariusBoss>();
                break;
            case GameType.RType:
                BossScript = bossObject.GetComponent<RtypeBoss>();
                break;
        }
        BossScript.enabled = true;
        BossScript.ShowText();
        bgmManager.BGMStop();
        
    }

    




    //�X�e�[�g�؂�ւ�
    public void SetCurrentState(GameState state)
    {
        currentGameState = state;
        OnGameStateChange(currentGameState);
    }

    //�Q�[���J�n
    void PlayingAction()
    {
        BossScript.PlayTrue();
        playerScript.play = true;
    }

    //�Q�[���I��
    void EndAction()
    {
        BossScript.PlayEnd();
        playerScript.play = false;
    }
}
