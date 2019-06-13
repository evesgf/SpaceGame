using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GPL
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipMove : MonoBehaviour
    {
        public Transform avatar;                    //船体
        public float maxForwardSpeed = 10f;         //最大前进速度
        public float forwoardAcc = 5f;              //前进加速度
        public float forwardDamp = 5f;              //前进阻尼

        public float maxUpSpeed = 10f;              //最大上升速度
        public float upAcc = 5f;                    //最大上升加速度
        public float upDamp = 5f;                   //上升阻尼

        public float maxRotateSpeed = 10f;          //最大旋转速度
        public float rotateAcc = 5f;                //旋转加速度
        public float rotateDamp = 5f;               //旋转阻尼

        public float avatarUpAngle = 5f;            //船体上升倾角
        public float avatarRotateAngle = 5f;        //船体旋转倾角
        public float avatarAngleInterpolation = 5f; //倾角转动差值

        internal Vector3 moveDirection;             //移动向量
        internal float nowForwardSpeed;             //当前前向移动速度
        internal float nowUpSpeed;                  //当前上升速度
        internal float nowRotateSpeed;              //当前旋转速度
        internal float nowRotateAngle;              //当前飞船转角

        internal Vector3 input;

        private Rigidbody m_rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            nowRotateAngle = transform.rotation.eulerAngles.y;
        }

        // Update is called once per frame
        void Update()
        {
            if (avatar != null)
            {
                //船体倾角模拟
                avatar.localRotation = Quaternion.Lerp(avatar.localRotation, Quaternion.Euler(avatarUpAngle * -(nowUpSpeed / maxUpSpeed), 0, avatarRotateAngle * -(nowRotateSpeed / maxRotateSpeed)), avatarAngleInterpolation * Time.deltaTime);
            }
        }

        public void OnMove(Vector3 input)
        {
            this.input = input;

            //旋转计算
            if (input.x > float.Epsilon || input.x < -float.Epsilon)
            {
                nowRotateSpeed += rotateAcc * Time.fixedDeltaTime * input.x;
            }
            else
            {
                nowRotateSpeed = Mathf.Lerp(nowRotateSpeed, 0, rotateDamp * Time.fixedDeltaTime);
            }
            nowRotateSpeed = Mathf.Clamp(nowRotateSpeed, -maxRotateSpeed, maxRotateSpeed);

            nowRotateAngle = nowRotateAngle + nowRotateSpeed;
            m_rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, nowRotateAngle, 0), rotateDamp * Time.fixedDeltaTime));

            //移动计算
            if (input.z > float.Epsilon || input.z < -float.Epsilon)
            {
                //nowForwardSpeed += forwoardAcc * Time.fixedDeltaTime * input.z;
                nowForwardSpeed = Mathf.Lerp(nowForwardSpeed, nowForwardSpeed + forwoardAcc * input.z, Time.fixedDeltaTime);
            }
            else
            {
                nowForwardSpeed = Mathf.Lerp(nowForwardSpeed, 0, forwardDamp * Time.fixedDeltaTime);
            }
            nowForwardSpeed = Mathf.Clamp(nowForwardSpeed, -maxForwardSpeed, maxForwardSpeed);


            if (input.y > float.Epsilon || input.y < -float.Epsilon)
            {
                nowUpSpeed = Mathf.Lerp(nowUpSpeed, nowUpSpeed + upAcc * input.y, Time.fixedDeltaTime);
            }
            else
            {
                nowUpSpeed = Mathf.Lerp(nowUpSpeed, 0, upDamp * Time.fixedDeltaTime);
            }
            nowUpSpeed = Mathf.Clamp(nowUpSpeed, -maxUpSpeed, maxUpSpeed);

            moveDirection = transform.up * nowUpSpeed + transform.forward * nowForwardSpeed;
            m_rigidbody.MovePosition(transform.position + moveDirection);
        }
    }
}
