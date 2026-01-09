using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Vector3 movePos;
    private GameObject cameraMovePos;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float smoothing;
    [SerializeField] private float cameraSize;
    [SerializeField] private float fadeAlpha;
    [SerializeField] private float fadeTime;
    private GameObject fade;
    private GameObject hudOverlay;

    private void Start()
    {
        //カメラ生成
        mainCamera = Instantiate(mainCamera, new Vector3(0, 0, 0), Quaternion.identity);
        
        //カメラ
        mainCamera = GameObject.Find("MainCamera(Clone)");
        cameraMovePos = GameObject.Find("CameraPosition");

        //HUDOverlay取得
        hudOverlay = transform.Find("HUDOverlay(Canvas)").gameObject;

        //Fade取得
        fade = hudOverlay.transform.Find("Fade").gameObject;

        //Canvasへカメラ指定
        hudOverlay.GetComponent<Canvas>().worldCamera = mainCamera.GetComponent<Camera>();

        //カメラ初期化
        CameraReset();

        //初期位置へ移動
        GetComponent<CameraController>().MoveToNextPos(0, 0, 5);

        //画面フェード
        fade.SetActive(true);
        ScreenFade(-1, 0.015f);
    }

    void FixedUpdate()
    {
        //画面フェード
        Color fadeTemp = hudOverlay.transform.Find("Fade").gameObject.GetComponent<Image>().color;
        if (fadeAlpha >= 0)
        {
            fadeTemp.a = Mathf.Lerp(fadeTemp.a, fadeAlpha, fadeTime);
        }
        else
        {
            fadeTemp.a -= fadeTime;
        }
        //Alphaが0より下になってしまった場合0を代入
        if (fadeTemp.a < 0)
        {
            fadeTemp.a = 0;
        }
        //Alphaを適応させる
        hudOverlay.transform.Find("Fade").gameObject.GetComponent<Image>().color = fadeTemp;

        //カメラ移動
        if (smoothing <= 0)
        {
            mainCamera.transform.position = movePos;
        }
        else
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, movePos, smoothing);
        }
        mainCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(mainCamera.GetComponent<Camera>().orthographicSize, cameraSize, smoothing);
    }

    //======================
    // カメラ移動関数
    // 引数 float smooth => スムージング速度
    //      int num  => 移動先の空オブジェクトの番号
    //      int scale => カメラのスケール
    // カメラ位置を移動させる
    // smoothを0以下に設定で即時移動
    //======================
    public void MoveToNextPos(float smooth, int num,int scale)
    {
        smoothing = smooth;
        movePos = cameraMovePos.transform.Find(num.ToString()).gameObject.transform.position;
        cameraSize = scale;
        Debug.Log("GetCameraPos (" + num + "), Pos:" + movePos);
    }

    //======================
    // 画面フェード関数
    // 引数 float alpha => 目標値
    //      float time  => フェードが終わるまでの時間
    // 画面をフェードイン、アウトさせる
    // 目標値がマイナスの場合、timeの値分一定の速度でフェードさせる
    //======================
    public void ScreenFade(float alpha, float time)
    {
        if (hudOverlay.transform.Find("Fade"))
        {
            fadeAlpha = alpha;
            fadeTime = time;
        }
        else
        {
            Debug.LogError("Fade not found.");
        }
    }

    private void CameraReset()
    {
        mainCamera.transform.position = new Vector3(0, 0, -1);
        mainCamera.GetComponent<Camera>().orthographicSize = 5.0f;
    }
}