using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField] private float addForce_X = 20.0f;
    [SerializeField] private float addForce_Y = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("StageObject"))
        {
            //Velocityの初期化
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            //横方向へのAddForce
            if (transform.TransformDirection(transform.eulerAngles).z > 0 && transform.TransformDirection(transform.eulerAngles).z < 180)
            {
                //左向き中心の90度からどれだけ離れているか
                float x = 90 - transform.TransformDirection(transform.eulerAngles).z;
                if (x < 0)
                {
                    x = -x;
                }
                //90を引いて横方向の強さを算出
                x -= 90;
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(x * addForce_X, 0.0f));
            }
            else if (transform.TransformDirection(transform.eulerAngles).z > 180 && transform.TransformDirection(transform.eulerAngles).z < 360)
            {
                //右向き中心の270度からどれだけ離れているか
                float x = 270 - transform.TransformDirection(transform.eulerAngles).z;
                if (x > 0)
                {
                    x = -x;
                }
                //90を足して横方向の強さを算出
                x += 90;
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(x * addForce_X, 0.0f));
            }

            //縦方向へのAddForce
            if (transform.TransformDirection(transform.eulerAngles).z > 90 && transform.TransformDirection(transform.eulerAngles).z < 270)
            {
                //下向き中心の180度からどれだけ離れているか
                float y = 180 - transform.TransformDirection(transform.eulerAngles).z;
                if (y > 0)
                {
                    y = -y;
                }
                //90を引いて縦方向の強さを算出
                y -= 90;
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, y * addForce_Y));
            }
            else if (transform.TransformDirection(transform.eulerAngles).z != 270 || transform.TransformDirection(transform.eulerAngles).z != 90)
            {
                //上向き中心の0度からどれだけ離れているか
                float y = transform.TransformDirection(transform.eulerAngles).z;
                if (y > 180)
                {
                    y = 360 - y;
                }
                y = 90 - y;
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, y * addForce_Y));
            }
            AudioPlayer.PlayAudioClip(2, 3, 1, 1, 0);
        }
    }
}
