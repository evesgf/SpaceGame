using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class WarpTunnelEffect : MonoBehaviour
    {
        public float rotationSpeed;

        public float startDuration = 3f;
        public AnimationCurve colorCurve;
        public Color startColor;
        public Color durationColor;

        public GameObject distortion;
        public AnimationCurve refractionFactorCurve;
        public float startRefractionFactor=0f;
        public float durationRefractionFactor=0.05f;

        private Material _material;
        private Material _distortionMaterial;

        public void StartWarpTunnel(float startDuration)
        {
            StartWarpTunnel(startDuration, startColor, durationColor);
        }

        public void StartWarpTunnel(float startDuration, Color startColor, Color durationColor)
        {
            _material = gameObject.GetComponent<MeshRenderer>().material;
            _distortionMaterial = distortion.GetComponent<MeshRenderer>().material;
            StartCoroutine(OnStartWarpTunnel(startDuration, startColor, durationColor));
        }

        IEnumerator OnStartWarpTunnel(float startDuration, Color startColor, Color durationColor)
        {
            _material.SetColor("_node_6523", startColor);
            _distortionMaterial.SetFloat("_RefractionFactor", startRefractionFactor);
            for (float i = 0; i < startDuration; i+=Time.deltaTime)
            {
                Color c = Color.Lerp(_material.GetColor("_node_6523"), durationColor, colorCurve.Evaluate(i / startDuration));
                _material.SetColor("_node_6523", c);

                float r= durationRefractionFactor* refractionFactorCurve.Evaluate(i / startDuration);
                _distortionMaterial.SetFloat("_RefractionFactor", r);
                yield return 0;
            }
        }

        public void EndWarpTunnel(float endDuration)
        {
            EndWarpTunnel(endDuration, startColor, durationColor);
        }

        public void EndWarpTunnel(float endDuration, Color startColor, Color durationColor)
        {
            _material = gameObject.GetComponent<MeshRenderer>().material;
            _distortionMaterial = distortion.GetComponent<MeshRenderer>().material;
            StartCoroutine(OnEndWarpTunnel(endDuration, startColor, durationColor));
        }

        IEnumerator OnEndWarpTunnel(float endDuration, Color startColor, Color durationColor)
        {
            _material.SetColor("_node_6523", durationColor);
            _distortionMaterial.SetFloat("_RefractionFactor", durationRefractionFactor);
            for (float i = 0; i < endDuration; i += Time.deltaTime)
            {
                Color c = Color.Lerp(_material.GetColor("_node_6523"), startColor, colorCurve.Evaluate(i / endDuration));
                _material.SetColor("_node_6523", c);

                float r = durationRefractionFactor - durationRefractionFactor * refractionFactorCurve.Evaluate(i / startDuration);
                _distortionMaterial.SetFloat("_RefractionFactor", r);

                yield return 0;
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.Euler(rotationSpeed, 0, 0),
                Time.deltaTime);
        }
    }
}
