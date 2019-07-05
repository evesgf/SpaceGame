using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class NormalTurretCtrl : MonoBehaviour
    {
        public LayerMask targetLayer;

        public bool debugShow = false;

        public Transform swivel;                //水平旋转根节点
        public float swivelRotateSpeed;         //水平转速
        public Vector2 HeadingLimit;

        public Transform barrel;                //垂直旋转根节点
        public float barrelRotateSpeed;         //垂直转速
        public Vector2 ElevationLimit;

        public Transform muzzle;                //枪口位置

        internal Vector3 target;

        private bool fullAccess = false;

        // Start is called before the first frame update
        void Start()
        {
            if (HeadingLimit.y - HeadingLimit.x >= 359.9f)
                fullAccess = true;
        }

        // Update is called once per frame
        void Update()
        {
            Rotate();
        }

        public float GetAngleToTarget()
        {
            return Vector3.Angle(barrel.forward, target - barrel.position);
        }

        public Vector3 GetTargetDistance()
        {
            Vector3 targetPos;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(muzzle.position,muzzle.forward, out hit, Camera.main.farClipPlane))
            {
                targetPos = Camera.main.WorldToScreenPoint(hit.point);
            }
            else
            {
                targetPos=Camera.main.WorldToScreenPoint(muzzle.position + muzzle.forward * Vector3.Distance(barrel.position, target));
            }

            return targetPos;
        }

        private void Rotate()
        {
            //通过LookRotation找出枪管垂直旋转角度
            Vector3 targetVel = target - muzzle.position;
            //指向目标的角度
            Quaternion targetRotationX = Quaternion.LookRotation(targetVel);
            //
            barrel.rotation = Quaternion.RotateTowards(barrel.rotation, targetRotationX, swivelRotateSpeed * Time.deltaTime);
            //剔除其余角度旋转
            barrel.localEulerAngles = new Vector3(barrel.localEulerAngles.x, 0f, 0f);
            //角度限制
            if (barrel.transform.localEulerAngles.x >= 180f && barrel.transform.localEulerAngles.x < (360f - ElevationLimit.y))
            {
                barrel.transform.localEulerAngles = new Vector3(360f - ElevationLimit.y, 0f, 0f);
            }
            //down
            else if (barrel.transform.localEulerAngles.x < 180f && barrel.transform.localEulerAngles.x > -ElevationLimit.x)
            {
                barrel.transform.localEulerAngles = new Vector3(-ElevationLimit.x, 0f, 0f);
            }

            //通过LookRotation找出旋转平台的角度
            Vector3 targetY = target;
            targetY.y = barrel.position.y;
            //指向目标在枪管平面的投影向量
            Quaternion targetRotationY = Quaternion.LookRotation(targetY - swivel.position);
            //
            swivel.rotation = Quaternion.RotateTowards(swivel.rotation, targetRotationY, barrelRotateSpeed * Time.deltaTime);
            //剔除其余角度旋转
            swivel.localEulerAngles = new Vector3(0f, swivel.localEulerAngles.y, 0f);
            //角度限制
            if (!fullAccess)
            {
                //checking for turning left
                if (swivel.transform.localEulerAngles.y >= 180f && swivel.transform.localEulerAngles.y < (360f - HeadingLimit.y))
                {
                    swivel.transform.localEulerAngles = new Vector3(0f, 360f - HeadingLimit.y, 0f);
                }

                //right
                else if (swivel.transform.localEulerAngles.y < 180f && swivel.transform.localEulerAngles.y > -HeadingLimit.x)
                {
                    swivel.transform.localEulerAngles = new Vector3(0f, -HeadingLimit.x, 0f);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!debugShow) return;
            Gizmos.color = Color.red;

            Debug.DrawLine(muzzle.position, muzzle.position + muzzle.forward * Vector3.Distance(barrel.position, target), Color.red);
        }
    }
}
