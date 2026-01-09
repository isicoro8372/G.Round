using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    Rigidbody2D rb;
    private GameObject emission, barrier;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject particleSpawnEmission, particleSpawnImpact, particleSpawnBarrier;

    void Start()
    {
        //プレイヤー生成
        Instantiate(player, transform.position, transform.rotation);
        //エフェクト生成
        emission = Instantiate(particleSpawnEmission, transform.position, transform.rotation);
        barrier = Instantiate(particleSpawnBarrier, transform.position, transform.rotation);
        //プレイヤーのRigidbody取得、移動不能化
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    void Update()
    {
        //スペースが押されたらゲーム開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameStart();
        }
    }

    public void GameStart() //スタート関数
    {
        AudioPlayer.PlayAudioClip(0, 0, 0.8f, 1, 0);
        gameObject.GetComponent<CameraController>().MoveToNextPos(0.1f, 1, 10);
        rb.constraints = RigidbodyConstraints2D.None;
        Instantiate(particleSpawnImpact, transform.position, transform.rotation);
        Destroy(emission);
        Destroy(barrier);
        GetComponent<PlayerSpawn>().enabled = false;
    }
}
