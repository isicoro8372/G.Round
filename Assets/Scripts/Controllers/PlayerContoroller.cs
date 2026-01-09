using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteMask mask;

    Vector3 currentPos;
    float len;

    [SerializeField] bool debug;
    [SerializeField] private float friction = 0.05f;
    [SerializeField] private float scaleDecreaseAmount = 0.05f;

    public static bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody取得
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        //SpriteMask取得
        //mask = transform.Find("Sprite Mask").gameObject.GetComponent<SpriteMask>();

        //isDeadの初期化
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //最初の1フレームのみ除外
        if (currentPos != null) 
        {
            len = (currentPos - transform.position).magnitude;
        }

        //現在位置保存
        currentPos = transform.position;

        //移動禁止状態なら回転させる
        if (rb.constraints == RigidbodyConstraints2D.FreezePosition && !debug)
        {
            transform.Rotate(0, 0, 1.5f);
        }

        //画面外に出たら死亡判定
        if (!gameObject.GetComponent<SpriteRenderer>().isVisible && !isDead && rb.constraints != RigidbodyConstraints2D.FreezePosition)
        {
            GameObject.Find("GamePlayController").GetComponent<GamePlayController>().IsDead(false);
        }

        /*死亡しているならマスクのスケールを減らす
        if (isDead)
        {
            if(mask.transform.localScale.x > 0)
            {
                mask.transform.localScale = new Vector3(mask.transform.localScale.x - scaleDecreaseAmount, mask.transform.localScale.y - scaleDecreaseAmount, 1);
            }
            else
            {
                mask.transform.localScale = new Vector3(0, 0, 1);
            }
        }*/
        
        //死亡しているならPlayerのスケールを減らす
        if (isDead)
        {
            if(transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x - scaleDecreaseAmount, transform.localScale.y - scaleDecreaseAmount, 1);
            }
            else
            {
                transform.localScale = new Vector3(0, 0, 1);
            }
        }

        //デバッグ
        if (debug)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(-0.2f, 0, 0);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(0.2f, 0, 0);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0.2f, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, -0.2f, 0);
            }
            if (Input.GetKey(KeyCode.X))
            {
                rb.constraints = RigidbodyConstraints2D.None;
                debug = false;
            }
        }
    }

    //======================
    // 移動判定関数
    // 戻り値 bool
    // 一定以上の速度で動いているかどうかの判定
    //======================

    public bool IsMoved()
    {
        if (len > friction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void IsDead()
    {
        isDead = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
