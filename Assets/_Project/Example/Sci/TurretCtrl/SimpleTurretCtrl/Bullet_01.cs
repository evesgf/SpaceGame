using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_01 : MonoBehaviour
{
    public float lifeTime = 5f;                 //子弹生命周期
    public float flySpeed = 100f;               //子弹飞行速度
    public float power = 10f;                   //子弹命中时产生爆炸力

    private float nowLifeTime;
    private AudioSource audioSource;
    private RaycastHit hit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Init()
    {
        nowLifeTime = 0;
        audioSource.Play();
    }

    private void OnFly(float time)
    {
        if (nowLifeTime >= lifeTime)
        {
            Destroy(gameObject);
        }

        transform.position += transform.forward * flySpeed*time;
        nowLifeTime += time;
    }

    private void FixedUpdate()
    {
        OnFly(Time.fixedDeltaTime);
    }
}
