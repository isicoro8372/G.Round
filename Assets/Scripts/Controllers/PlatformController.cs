using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public bool reverse = false;
    private GameObject parent;
    private void Start()
    {
        if (!gameObject.CompareTag("Platform"))
        {
            parent = transform.parent.gameObject;
        }
        else
        {
            parent = this.gameObject;
        }
    }
    void FixedUpdate()
    {
        float rotate = 0;
        if(!GamePlayController.retry && !GamePlayController.exit && gameObject.CompareTag("Platform"))
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                rotate += 0.5f;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    rotate += 0.5f;
                }
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rotate -= 0.5f;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    rotate -= 0.5f;
                }
            }
            Rotate_Platform(rotate);
        }
    }
    void OnTriggerStay2D(Collider2D collidier) //Platformに接地した場合一定速度以下なら子オブジェクトに
    {
        if (collidier.CompareTag("Player"))
        {
            if (collidier.gameObject.GetComponent<PlayerContoroller>().IsMoved() == false)
            {
                collidier.transform.SetParent(parent.transform);
            }
        }
        else if (collidier.CompareTag("StageObject"))
        {
            if (collidier.gameObject.GetComponent<StageObjectController>().IsMoved() == false)
            {
                collidier.transform.SetParent(parent.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collidier)
    {
        if (collidier.gameObject.GetComponent<PlayerContoroller>() || collidier.gameObject.GetComponent<StageObjectController>())
        {
            string tag = collidier.tag;
            switch(tag)
            {
                case "StageObject":
                    collidier.transform.SetParent(GameObject.FindGameObjectWithTag("StageObject").transform);
                    break;
                default:
                    collidier.transform.SetParent(null);
                    break;
            }
        }
    }
    public void Rotate_Platform(float rotate)
    {
        if (reverse)
        {
            transform.Rotate(0, 0, -rotate);
        }
        else
        {
            transform.Rotate(0, 0, rotate);
        }
    }
}


