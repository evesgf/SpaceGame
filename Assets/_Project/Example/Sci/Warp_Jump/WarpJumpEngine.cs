using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class WarpJumpEngine : MonoBehaviour
    {
        public Transform avatar;

        public float jumpOutSpeed = 1f;                //粒子速度，1倍为3秒起跳
        public WarpJumpEffect jumpOut;

        public float jumpInSpeed = 1f;                //粒子速度，1倍为3秒起跳
        public WarpJumpEffect jumpIn;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                WarpJumpOut();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                WarpJumpIn();
            }
        }

        void WarpJumpOut()
        {
            var jo=Instantiate(jumpOut,transform);
            jo.transform.localPosition = Vector3.zero;
            jo.transform.localRotation = Quaternion.identity;
            jo.Init(avatar, jumpOutSpeed);
            Destroy(jo.gameObject, 6f);
        }

        void WarpJumpIn()
        {
            var jo = Instantiate(jumpIn, transform);
            jo.transform.localPosition = Vector3.zero;
            jo.transform.localRotation = Quaternion.identity;
            jo.Init(avatar, jumpInSpeed);
            Destroy(jo.gameObject, 6f);
        }
    }
}