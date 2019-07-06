using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class Randomize : MonoBehaviour
    {
        private new Transform transform; // Cached transform
        private Vector3 defaultScale; // Default scale

        public bool RandomScale, RandomRotation; // Randomize flags
        public float MinScale, MaxScale; // Min/Max scale range
        public float MinRotation, MaxRotaion; // Min/Max rotation range

        void Awake()
        {
            // Store transform component and default scale
            transform = GetComponent<Transform>();
            defaultScale = transform.localScale;
        }

        // Randomize scale and rotation according to the values set in the inspector
        void OnEnable()
        {
            if (RandomScale)
                transform.localScale = defaultScale * Random.Range(MinScale, MaxScale);

            if (RandomRotation)
                transform.rotation *= Quaternion.Euler(0, 0, Random.Range(MinRotation, MaxRotaion));
        }
    }
}
