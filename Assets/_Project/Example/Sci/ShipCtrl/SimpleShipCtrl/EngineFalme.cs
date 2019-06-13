using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{

    public class EngineFalme : MonoBehaviour
    {
        public enum InputAxis
        {
            x, y, z
        }

        public ShipMove shipMove;
        public InputAxis inputAxis = InputAxis.y;
        public bool isInversion = false;

        public Vector3 minFalmeScale;
        public Vector3 maxFalmeScale;
        public Transform falme;

        public float minStar = 0;
        public float maxStar = 20;
        public ParticleSystem star;

        private ParticleSystem.EmissionModule emission;
        private float value;

        // Start is called before the first frame update
        void Start()
        {
            falme.localScale = minFalmeScale;

            if (star != null)
            {
                emission = star.emission;
                emission.rateOverTime = minStar;
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (inputAxis)
            {
                case InputAxis.x:
                    value = shipMove.input.x;
                    break;
                case InputAxis.y:
                    value = shipMove.input.y;
                    break;
                case InputAxis.z:
                    value = shipMove.input.z;
                    break;
            }

            if (isInversion)
            {
                if (value > 0) value = 0;
                falme.localScale = Vector3.Lerp(minFalmeScale, maxFalmeScale, Mathf.Abs(value));

                if (star != null)
                    emission.rateOverTime = maxStar * value;
            }
            else
            {
                if (value < 0) value = 0;
                falme.localScale = Vector3.Lerp(minFalmeScale, maxFalmeScale, Mathf.Abs(value));

                if (star != null)
                    emission.rateOverTime = maxStar * value;
            }
        }
    }

}