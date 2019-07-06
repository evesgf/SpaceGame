using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class GPLMissileTarget : MonoBehaviour
    {
        public float rotateSpeed = 60f;

        public Transform target;
        public float distance = 10f;
        public float hight = 5f;

        // Start is called before the first frame update
        void Start()
        {
            if (!target) target = GameObject.Find("Target").transform;
            target.position = new Vector3(0, hight, distance);
        }

        private void FixedUpdate()
        {
            transform.Rotate(transform.up * rotateSpeed * Time.fixedDeltaTime);
        }
    }
}
