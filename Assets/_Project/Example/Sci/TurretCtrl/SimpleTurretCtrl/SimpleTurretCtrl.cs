using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GPL
{

    public class SimpleTurretCtrl : MonoBehaviour
    {
        public bool debugShow = false;

        public Transform swivel;                //水平旋转根节点
        public float swivelRotateSpeed;         //水平转速
        public Vector2 swivelRotateLimit;       //旋转角度限制

        public Transform barrel;                //垂直旋转根节点
        public float barrelRotateSpeed;         //垂直转速
        public Vector2 barrelRotateLimit;       //旋转角度限制

        public Transform muzzle;                //枪口位置

        internal Vector3 target;

        private RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Rotate(target);  
        }

        /// <summary>
        /// 根据目标旋转炮塔
        /// </summary>
        /// <param name="target"></param>
        private void Rotate(Vector3 target)
        {
            //通过LookRotation找出枪管垂直旋转角度
            Vector3 targetVel = target - muzzle.position;
            Quaternion targetRotationX = Quaternion.LookRotation(targetVel);
            barrel.rotation = Quaternion.RotateTowards(barrel.rotation, targetRotationX, barrelRotateSpeed * Time.deltaTime);
            //剔除其余角度旋转
            barrel.localEulerAngles = new Vector3(barrel.localEulerAngles.x, 0f, 0f);

            //上方角度限制
            if (barrel.localEulerAngles.x >= 180f && barrel.localEulerAngles.x < (360f - barrelRotateLimit.y))
            {
                barrel.localEulerAngles = new Vector3(360f - barrelRotateLimit.y, 0f, 0f);
            }
            //下方角度限制
            else if (barrel.localEulerAngles.x < 180f && barrel.localEulerAngles.x > -barrelRotateLimit.x)
            {
                barrel.localEulerAngles = new Vector3(-barrelRotateLimit.x, 0f, 0f);
            }

            //通过LookRotation找出炮塔水平旋转的角度
            Vector3 targetY = target;
            targetY.y = barrel.position.y;
            Quaternion targetRotationY = Quaternion.LookRotation(targetY - swivel.position);
            swivel.rotation = Quaternion.RotateTowards(swivel.rotation, targetRotationY, swivelRotateSpeed * Time.deltaTime);
            //剔除其余角度旋转
            swivel.localEulerAngles = new Vector3(0f, swivel.localEulerAngles.y, 0f);

            //左侧角度限制
            if (swivel.localEulerAngles.y >= 180f && swivel.localEulerAngles.y < (360f - swivelRotateLimit.y))
            {
                swivel.localEulerAngles = new Vector3(0f, 360f - swivelRotateLimit.y, 0f);
            }
            //右侧角度限制
            else if (swivel.localEulerAngles.y < 180f && swivel.localEulerAngles.y > -swivelRotateLimit.x)
            {
                swivel.localEulerAngles = new Vector3(0f, -swivelRotateLimit.x, 0f);
            }
        }

        /// <summary>
        /// 返回和目标的夹角
        /// </summary>
        /// <returns></returns>
        public float GetAngleToTarget()
        {
            return Vector3.Angle(barrel.forward, target - barrel.position);
        }

        /// <summary>
        /// 返回炮管真实指向位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRealTargetPos()
        {
            return Physics.Raycast(muzzle.position, muzzle.forward, out hit, Camera.main.farClipPlane) ? hit.point : muzzle.forward * Camera.main.farClipPlane;
        }

        private void OnDrawGizmos()
        {
            if (!debugShow) return;
            Gizmos.color = Color.red;

            Debug.DrawLine(muzzle.position, muzzle.position + muzzle.forward * Vector3.Distance(barrel.position, target), Color.red);
        }
    }
}
