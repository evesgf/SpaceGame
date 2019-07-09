using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class WarpJumpTunnel : MonoBehaviour
    {
        private new Transform transform;
        private MeshRenderer meshRenderer;

        public float StartDelay, FadeDelay;
        public Vector3 ScaleTo;
        public float ScaleTime;
        public float ColorTime, ColorFadeTime;
        public float RotationSpeed;

        private bool grow = false;
        private float speed;

        float alpha;

        int alphaID;

        private void Awake()
        {
            transform = GetComponent<Transform>();
            meshRenderer = GetComponent<MeshRenderer>();

            alphaID = Shader.PropertyToID("_Alpha");
        }

        public void Init(float speed=1f)
        {
            this.speed = speed;
            grow = false;
            meshRenderer.material.SetFloat(alphaID, 0);
            Invoke("ToggleGrow", StartDelay / speed);
            Invoke("ToggleGrow", FadeDelay / speed);
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localRotation = transform.localRotation * Quaternion.Euler(0, 0, Random.Range(-360, 360));
        }

        void ToggleGrow()
        {
            grow = !grow;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0f, 0f, RotationSpeed * speed * Time.deltaTime);
            if (grow)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, ScaleTo, Time.deltaTime * ScaleTime * speed);

                alpha = Mathf.Lerp(alpha, 1, Time.deltaTime * ColorTime * speed);
                meshRenderer.material.SetFloat(alphaID, alpha);
            }
            else
            {
                alpha = Mathf.Lerp(alpha, 0, Time.deltaTime * ColorFadeTime * speed);
                meshRenderer.material.SetFloat(alphaID, alpha);
            }
        }
    }

}