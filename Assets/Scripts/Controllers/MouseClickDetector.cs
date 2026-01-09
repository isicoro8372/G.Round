using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] clickdetect = GameObject.FindGameObjectsWithTag("ClickDetect");
        foreach (GameObject obj in clickdetect)
        {
            obj.transform.Translate(0,0,-0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GamePlayController.retry && !GamePlayController.exit)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit && GamePlayController.canScreenEvent)
            {
                if (hit.transform.name == "BarrierCollider")
                {
                    GameObject.Find("GamePlayController").GetComponent<PlayerSpawn>().GameStart();
                }
                else if (hit.transform.name == "Door")
                {
                    hit.transform.gameObject.GetComponent<Door>().Use();
                }
            }
        }
    }
}
