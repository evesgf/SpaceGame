using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_01 : MonoBehaviour
{
    public float lifeTime = 5f;
    public float flySpeed = 100f;

    private float nowLifeTime;
    private AudioSource audioSource;

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
