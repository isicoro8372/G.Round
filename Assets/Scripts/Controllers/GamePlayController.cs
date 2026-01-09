using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private float exitFadeSpeed = -0.04f;
    [SerializeField] private float popupMovemetSpeed;

    [SerializeField] private GameObject deathEffect;

    [SerializeField] private GameObject[] stagePrefabs_Platform;
    [SerializeField] private GameObject[] stagePrefabs_CameraPosition;

    public static bool retry = false;
    public static bool exit = false;
    private bool isActivePopup = false;
    private bool isDeactivepopup = false;
    private GameObject continuePopup;
    private RectTransform continuePopupPosition;

    public static bool canScreenEvent = true;
    private float timeNow = 0;
    private float timeBeforeOneFrame = 0;

    private void Start()
    {
        retry = false;
        exit = false;
        //ポップアップを取得
        continuePopup = transform.Find("HUDOverlay(Canvas)/Continue_PopUp").gameObject;
        //ポップアップのrectTransformを取得
        continuePopupPosition = continuePopup.GetComponent<RectTransform>();
        //ポップアップ内のボタンに対しリスナを設定
        //アニメーション状態でない場合にのみ実行
        continuePopup.transform.Find("Continue_Button_Retry").GetComponent<Button>().onClick.AddListener(() =>
        {
            retry = true;
            ActivePopup(false, true);
        });
        continuePopup.transform.Find("Continue_Button_Back").GetComponent<Button>().onClick.AddListener(() =>
        {
            ActivePopup(false, false);
        });
        continuePopup.transform.Find("Continue_Button_Exit").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (SceneManager.GetActiveScene().name == "TitleScene")
            {
                exitFadeSpeed = -0.005f;
            }
            exit = true;
            ActivePopup(false, true);

        });
        //シーン読み込み時にClickDetectorを有効化
        canScreenEvent = true;
        //ポップアップ用の時間取得
        timeBeforeOneFrame = Time.realtimeSinceStartup;

        //AudioSystemの読み込み
        AudioPlayer.GetAudioSystem();

        //サウンドの再生
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            AudioPlayer.PlayAudioClip(0, 5, 1, 1, 0);
        }
        else
        {
            AudioPlayer.PlayAudioClip(0, 6, 0.45f, 1, 0);
        }

        //PlatformにControllerをつける
        GameObject[] pl_setcache = GameObject.FindGameObjectsWithTag("Platform");
        for(int i = 0; i < pl_setcache.Length; i++)
        {
            if (!pl_setcache[i].GetComponent<PlatformController>())
            {
                pl_setcache[i].AddComponent<PlatformController>();
            }
        }

        //StagePictureを取得、上書き
        for(int i = 0; i < continuePopup.GetComponent<PictureContainer>().stagePictures.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == continuePopup.GetComponent<PictureContainer>().stagePictures[i].name)
            {
                continuePopup.transform.Find("StagePicture").GetComponent<Image>().sprite = continuePopup.GetComponent<PictureContainer>().stagePictures[i];
            }
        }
    }

    private void Update()
    {
        //ポーズ時ポップアップ表示
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (continuePopup.activeSelf == false)
            {
                ActivePopup(true, true);
            }
            else if (!isActivePopup && !isDeactivepopup)
            {
                ActivePopup(false, false);
            }
        }

        //ポップアップの移動処理
        timeBeforeOneFrame = timeNow;
        timeNow = Time.realtimeSinceStartup;
        if (isActivePopup)
        {
            continuePopupPosition.anchoredPosition = new Vector2(0, continuePopupPosition.anchoredPosition.y - popupMovemetSpeed * (timeNow - timeBeforeOneFrame));
            if (continuePopupPosition.anchoredPosition.y <= 1)
            {
                continuePopupPosition.anchoredPosition = new Vector3(0, 0, 0);
                isActivePopup = false;
            }
        }
        if (isDeactivepopup)
        {
            continuePopupPosition.anchoredPosition = new Vector2(0, continuePopupPosition.anchoredPosition.y + popupMovemetSpeed * (timeNow - timeBeforeOneFrame));
            if (continuePopupPosition.anchoredPosition.y >= 899)
            {
                continuePopupPosition.anchoredPosition = new Vector3(0, 900, 0);
                isDeactivepopup = false;
                continuePopup.SetActive(false);
                Time.timeScale = 1;
                if (!exit || !retry)
                {
                    canScreenEvent = true;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (retry || exit)
        {
            GetComponent<CameraController>().ScreenFade(-1, exitFadeSpeed);
        }
    }

    private void LateUpdate()
    {
        if (transform.Find("HUDOverlay(Canvas)/Fade").gameObject.GetComponent<Image>().color.a >= 1.0f && (retry || exit))
        {
            if (retry)
            {
                Debug.Log("Scene Reloading...");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (exit)
            {
                if (SceneManager.GetActiveScene().name == "TitleScene")
                {
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                }
                else
                {
                    SceneDataCache.LoadScene(true, "TitleScene", 0);
                }
            }
        }
    }
    public void IsDead(bool goal)
    {
        GameObject player = GameObject.FindWithTag("Player").gameObject;
        player.GetComponent<PlayerContoroller>().IsDead();
        Instantiate(deathEffect, player.transform.position, Quaternion.Euler(0.0f, player.transform.rotation.eulerAngles.y, 0.0f));
        if (goal)
        {

        }
        else
        {
            AudioPlayer.PlayAudioClip(1, 7, 1, 1, 0);
        }
        canScreenEvent = false;
        StartCoroutine(SceneDataCache.WaitForSomeTimes(2, () =>
        {
            Destroy(player);
            ActivePopup(true, false);
            ChangeDrawPopup();
        }));
    }

    public void ActivePopup(bool active, bool canClose) //ポップアップのアクティブ状態の変更
    {
        //アクティブ化
        if (active)
        {
            //すでにリトライ、リタイア（ゲーム終了）が押されていない場合
            if (!retry && !exit)
            {
                Time.timeScale = 0;
                isActivePopup = true;
                canScreenEvent = false;
                continuePopup.SetActive(true);
                //ユーザーによりクローズ可
                if (canClose)
                {
                    AudioPlayer.PlayAudioClip(0, 2, 1, 1.5f, 0);
                    SceneDataCache.canCloseContinuePopup = true;
                }
                //ユーザーによりクローズ不可
                else
                {
                    SceneDataCache.canCloseContinuePopup = false;
                }
            }
        }
        //非アクティブ化
        else
        {
            if (canClose || SceneDataCache.canCloseContinuePopup)
            {
                isActivePopup = false;
                isDeactivepopup = true;
                if(retry || exit)
                {
                    if (retry)
                    {
                        AudioPlayer.PlayAudioClip(0, 2, 1, 2, 0);
                    }
                    else
                    {
                        AudioPlayer.PlayAudioClip(0, 2, 1, 0.5f, 0);
                    }
                    Time.timeScale = 1;
                }
                else
                {
                    AudioPlayer.PlayAudioClip(0, 2, 1, 1, 0);
                }
            }
        }
    }

    private void ChangeDrawPopup() //ポップアップのBackボタンの削除
    {
        Destroy(continuePopup.transform.Find("Continue_Button_Back").gameObject);
        continuePopup.transform.Find("Continue_Button_Exit").GetComponent<RectTransform>().anchoredPosition = new Vector2(192, -176);
        continuePopup.transform.Find("Continue_Button_Retry").GetComponent<RectTransform>().anchoredPosition = new Vector2(-192, -176);
    }
}
