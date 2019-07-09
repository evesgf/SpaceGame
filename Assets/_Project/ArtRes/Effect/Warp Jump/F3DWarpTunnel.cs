using UnityEngine;
using System.Collections;

namespace Forge3D
{
    public class F3DWarpTunnel : MonoBehaviour
    {

        public float MaxRotationSpeed;
        public float AdaptationFactor;
         
        float speed, newSpeed;
        // Use this for initialization
        void Start()
        {
            speed = 0;
            OnDirectionChange();
        }

        void OnDirectionChange()
        {
            newSpeed = Random.Range(-MaxRotationSpeed, MaxRotationSpeed);
            F3DTime.time.AddTimer(Random.Range(1, 5), 1, OnDirectionChange);
        }

        // Update is called once per frame
        void Update()
        {

            speed = Mathf.Lerp(speed, newSpeed, Time.deltaTime*AdaptationFactor);
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation*Quaternion.Euler(speed, 0, 0),
                Time.deltaTime);
        }
    }
}