using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class TestNormalShield : MonoBehaviour
    {
        public float hitTimeScale = 1f;

        private Material mat;
        private float nowHitTime;

        // Start is called before the first frame update
        void Start()
        {
            mat = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            nowHitTime -= Time.deltaTime * hitTimeScale;
            nowHitTime = Mathf.Clamp01(nowHitTime);
            mat.SetFloat("_HitTime", nowHitTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            // On colission set shader vector property _HitPosition with the impact point and
            // set _hittime shader property to start the impact effect
            foreach (ContactPoint contact in collision.contacts)
            {
                mat.SetVector("_HitPosition", transform.InverseTransformPoint(contact.point));
                nowHitTime = 1;
                mat.SetFloat("_HitTime", nowHitTime);
            }
        }
    }
}
