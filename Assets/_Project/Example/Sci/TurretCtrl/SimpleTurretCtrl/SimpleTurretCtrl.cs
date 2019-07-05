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
        public Transform barrel;                //垂直旋转根节点
        public float barrelRotateSpeed;         //垂直转速

        public Transform muzzle;                //枪口位置

        internal Vector3 target;

        // Start is called before the first frame update
        void Start()
        {

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
            return Camera.main.WorldToScreenPoint(muzzle.position + muzzle.forward * Vector3.Distance(barrel.position, target));
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

            //通过LookRotation找出旋转平台的角度
            Vector3 targetY = target;
            targetY.y = barrel.position.y;
            //指向目标在枪管平面的投影向量
            Quaternion targetRotationY = Quaternion.LookRotation(targetY - swivel.position);
            //
            swivel.rotation =Quaternion.RotateTowards(swivel.rotation, targetRotationY, barrelRotateSpeed * Time.deltaTime);
            //剔除其余角度旋转
            swivel.localEulerAngles = new Vector3(0f, swivel.localEulerAngles.y, 0f);
        }

        private void OnDrawGizmos()
        {
            if (!debugShow) return;
            Gizmos.color = Color.red;

            Debug.DrawLine(muzzle.position, muzzle.position + muzzle.forward * Vector3.Distance(barrel.position, target), Color.red);
        }
    }
}
