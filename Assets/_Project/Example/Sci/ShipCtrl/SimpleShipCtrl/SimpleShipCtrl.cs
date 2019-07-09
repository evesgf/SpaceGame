using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class SimpleShipCtrl : MonoBehaviour
    {
        public Transform avatar;
        public float forwardSpeed = 10f;
        public float rotateSpeed = 10f;
        public float upSpeed = 10f;

        public float avatarUpAngle = 5f;
        public float avatarRotateAngle = 5f;
        public float avatarAngleStep = 5f;

        internal Vector3 moveDirection;
        internal float nowForwardSpeed;
        internal float nowRotateSpeed;
        internal float nowUpSpeed;

        internal Vector3 input;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));

            OnMove(input);

            if (avatar != null)
            {
                //模拟Roll和Pitch旋转
                avatar.localRotation = Quaternion.Lerp(avatar.localRotation, Quaternion.Euler(avatarUpAngle * -input.y, 0, avatarRotateAngle * -input.x), avatarAngleStep * Time.deltaTime);
            }
        }

        public void OnMove(Vector3 input)
        {
            //Yaw旋转
            transform.Rotate(0, input.x * Time.deltaTime * rotateSpeed, 0);
            //上下前后移动
            transform.Translate(0, input.y * Time.deltaTime * upSpeed, input.z * Time.deltaTime * forwardSpeed);
        }
    }
}
