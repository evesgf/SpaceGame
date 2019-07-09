using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{

    public class WarpJumpEffect : MonoBehaviour
    {
        public bool isIn = false;
        public ParticleSystem warpRings;
        public Vector3 warpRangeScale = new Vector3(1, 1, 1.2f);
        public ParticleSystem warpMoveing;
        public Transform ShipPos;
        public float ShipJumpSpeed;
        public Vector3 ShipJumpStartPoint;
        public Vector3 ShipJumpEndPoint;
        public bool isPlayAwake = false;

        bool isRings;
        bool isWarping;

        private ParticleSystem[] particles;

        IEnumerator OnStartWarp(Transform avatar, float speed = 1f)
        {
            BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
            ShipPos = avatar;
            isRings = false;
            isWarping = false;
            warpMoveing.transform.localPosition = ShipJumpStartPoint;
            ShipPos.position = warpMoveing.transform.position;

            particles = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].playbackSpeed = speed;
            }

            var tunnels = GetComponentsInChildren<WarpJumpTunnel>();
            foreach (var tunnel in tunnels)
            {
                tunnel.Init(speed);
            }

            yield return new WaitForSeconds(warpRings.main.startDelayMultiplier / speed);

            OnRingsScale();

            var t1 = warpMoveing.main.startDelayMultiplier - warpRings.main.startDelayMultiplier;

            yield return new WaitForSeconds(t1 / speed);
            isRings = false;
            OnWarpScale();
        }

        private void Start()
        {
            if (isPlayAwake)
                Init(ShipPos);
        }

        // Update is called once per frame
        void Update()
        {
            if (isRings && !isIn)
                ShipRingsScale();

            if (isWarping)
                ShiftShipPosition();
        }

        public void Init(Transform avatar, float speed = 1f)
        {
            StartCoroutine(OnStartWarp(avatar, speed));
        }

        public void OnRingsScale()
        {
            isRings = true;
        }

        public void OnWarpScale()
        {
            isWarping = true;
        }

        void ShipRingsScale()
        {
            warpMoveing.transform.localScale = Vector3.Lerp(warpMoveing.transform.localScale, warpRangeScale,
    Time.deltaTime * ShipJumpSpeed);
            ShipPos.localScale = warpMoveing.transform.localScale;
        }

        void ShiftShipPosition()
        {
            warpMoveing.transform.localPosition = Vector3.Lerp(warpMoveing.transform.localPosition, ShipJumpEndPoint,
                Time.deltaTime * ShipJumpSpeed);
            ShipPos.position = warpMoveing.transform.position;

            if (isIn)
            {
                warpMoveing.transform.localScale = Vector3.Lerp(warpMoveing.transform.localScale, warpRangeScale,
    Time.deltaTime * ShipJumpSpeed);
            }
            else
            {
                warpMoveing.transform.localScale = Vector3.Lerp(warpMoveing.transform.localScale, Vector3.zero,
    Time.deltaTime * ShipJumpSpeed);
            }
            ShipPos.localScale = warpMoveing.transform.localScale;
        }
    }

}