using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class MissileBase : MonoBehaviour
    {
        /// <summary>
        /// 导弹发射类型
        /// </summary>
        public enum MissileType
        {
            Unguided,       //无锁
            Guided,         //跟踪
            Predictive      //预判
        }

        public Transform target;
        public LayerMask layerMask;

        public bool showDebug = false;

        public MissileType missileType;         //导航类型
        public float lifeTime = 5f;             //存活时间
        public float flySpeed=300f;             //飞行速度

        public float rotateSpeed = 10f;         //跟踪转向系数

        public GameObject explodePrefab;        //爆炸特效
        public float minHitDistance=1f;         //最小撞击距离
        public float RaycastAdvance = 2f;       //无锁时前向检测距离

        bool isHit = false;                     //碰撞flag
        float timer = 0;                        //飞行时间

        private Vector3 targetLastPos;          //缓存上一帧的目标位置
        private Vector3 step;                   //本帧飞行步长
        private Vector3 predictivePos;          //预判位置
        private bool isMissileDestroy = false;  //特效flag

        public void Init()
        {
            isHit = false;
            timer = 0;
            targetLastPos = Vector3.zero;
        }

        public void OnHit()
        {
            isHit = true;
            Destroy(gameObject);
            OnMissileDestroy();
        }

        public void OnMissileDestroy()
        {
            if (explodePrefab == null) return;
            var ex = Instantiate(explodePrefab,transform.transform.position,transform.rotation) as GameObject;
            Destroy(ex, 1f);
        }

        private void FixedUpdate()
        {
            if (isHit)
            {
                //撞击后执行一次函数
                
            }
            else
            {
                //目标导航
                if (target != null)
                {
                    //跟踪
                    if (missileType == MissileType.Guided)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation,
                            Quaternion.LookRotation(target.position - transform.position), Time.fixedDeltaTime * rotateSpeed);
                    }
                    //预判
                    else if (missileType == MissileType.Predictive)
                    {
                        //计算预判位置
                        predictivePos = TargetPredict(transform.position, target.position, targetLastPos,
                            flySpeed);
                        targetLastPos = target.position;

                        transform.rotation = Quaternion.Lerp(transform.rotation,
                            Quaternion.LookRotation(predictivePos - transform.position), Time.fixedDeltaTime * rotateSpeed);
                    }
                }

                //计算本帧飞行步长
                step = transform.forward * Time.fixedDeltaTime * flySpeed;

                //撞击判断
                if (target != null && missileType != MissileType.Unguided &&
                    Vector3.SqrMagnitude(transform.position - target.position) <= minHitDistance)
                {
                    OnHit();
                }
                else if (missileType == MissileType.Unguided &&
                         Physics.Raycast(transform.position, transform.forward, step.magnitude * RaycastAdvance, layerMask))
                {
                    OnHit();
                }
                // Nothing hit
                else
                {
                    //超时检测
                    if (timer >= lifeTime)
                    {
                        OnHit();
                    }
                }

                transform.position += step;
            }

            timer += Time.fixedDeltaTime;
        }

        private void OnDrawGizmos()
        {
            if (!showDebug) return;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * RaycastAdvance);

            if (target == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.forward * Vector3.SqrMagnitude(transform.position - target.position));

            if (predictivePos == Vector3.zero) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(predictivePos,0.5f);
            Gizmos.DrawLine(transform.position, transform.forward * Vector3.SqrMagnitude(transform.position - predictivePos));
        }

        /// <summary>
        /// 预判目标位置
        /// </summary>
        /// <param name="basePos">自身当前帧位置</param>
        /// <param name="targetPos">目标当前帧位置</param>
        /// <param name="targetLastPos">目标上一帧位置</param>
        /// <param name="baseVel">自身当前帧速度</param>
        /// <returns></returns>
        private Vector3 TargetPredict(Vector3 basePos,Vector3 targetPos,Vector3 targetLastPos,float baseVel)
        {
            //计算目标帧飞行向量
            Vector3 targetVel = (targetPos - targetLastPos) / Time.fixedDeltaTime;

            //计算预期到目标的时间
            float flyTime = GetProjFlightTime(targetPos - basePos, targetVel, baseVel);

            if (flyTime > 0)
                return targetPos + flyTime * targetVel;
            return targetPos;
        }

        /// <summary>
        /// 计算预期飞抵目标的时间
        /// </summary>
        /// <param name="distance">自身与目标的距离</param>
        /// <param name="targetVel">目标的飞行向量</param>
        /// <param name="baseVel">自身的飞行向量</param>
        /// <returns></returns>
        private float GetProjFlightTime(Vector3 distance, Vector3 targetVel, float baseVel)
        {
            float a = Vector3.Dot(targetVel, targetVel) - baseVel * baseVel;
            float b = 2.0f * Vector3.Dot(targetVel, distance);
            float c = Vector3.Dot(distance, distance);

            float det = b * b - 4 * a * c;

            if (det > 0)
                return 2 * c / (Mathf.Sqrt(det) - b);
            return -1;
        }
    }
}
