using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterStage : MonoBehaviour
{
    public Sprite popupPicture;
    public int stageNumber;

    [SerializeField] private Vector3 popUpScale, popUpPosition;
    [SerializeField] private bool enterStage;
    private GameObject popUp;
    private GameObject gamePlayController;

    void Start()
    {
        //初期化
        popUp = transform.Find("PopUp").gameObject;
        gamePlayController = GameObject.Find("GamePlayController").gameObject;
        popUp.transform.SetLocalPositionAndRotation(new Vector3(0, 8, 0), Quaternion.identity);
        popUp.transform.localScale = new Vector3(0, 0, 0);
        popUp.SetActive(false);
        enterStage = false;
    }

    void FixedUpdate()
    {
        if (popUp.transform.localScale.x <= 0)
        {
            if (enterStage)
            {
                popUp.SetActive(true);
            }
            else
            {
                popUp.SetActive(false);
            }
        }

        if (!enterStage && popUp.transform.localScale.x >= 0)
        {
            popUp.transform.localScale -= popUpScale;
            popUp.transform.localPosition -= popUpPosition;
        }
        else if (enterStage && popUp.transform.localScale.x <= 2.0f)
        {
            popUp.transform.localScale += popUpScale;
            popUp.transform.localPosition += popUpPosition;
        }
    }

    void LateUpdate()
    {
        if(gamePlayController.transform.Find("HUDOverlay(Canvas)/Fade").gameObject.GetComponent<Image>().color.a >= 1.0f && enterStage && !GamePlayController.retry)
        {
            Debug.Log("EnterStage");
            SceneDataCache.LoadScene(false, "Stage", SceneDataCache.selectedStage);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && popUp)
        {
            enterStage = true;
            gamePlayController.GetComponent<CameraController>().ScreenFade(-1, -0.005f);
            SceneDataCache.selectedStage = stageNumber;

            //移動した場合フェード解除
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                gamePlayController.GetComponent<CameraController>().ScreenFade(-1, 0.1f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && popUp)
        {
            enterStage = false;
            gamePlayController.GetComponent<CameraController>().ScreenFade(-1, 0.1f);
            SceneDataCache.selectedStage = -1;
        }
    }
}
