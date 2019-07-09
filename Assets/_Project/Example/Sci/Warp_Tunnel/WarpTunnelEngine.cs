using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class WarpTunnelEngine : MonoBehaviour
    {
        public bool showDebug = false;
        public Vector3 target;                          //跃迁坐标点
        public float minWarpDistance=10f;               //最小跃迁距离
        public float maxWarpDistance=10000f;            //最大跃迁距离
        public float maxWarpSpeed = 100f;               //最大跃迁速度

        public float fullWarpTime=5f;                   //0到满速的加速时间

        Vector3 realTarget;                             //最终跃迁坐标点
        Vector3 warpVel;                                //跃迁方向
        float nowWarpSpeed;                             //当前跃迁速度

        public WarpTunnelEffect warpTunnelEffect;       //跳跃通道特效

        private WarpTunnelEffect nowWarpTunnelEffect;   //当前跃迁通道特效
        private Vector3 warpStartBPos;                  //起跳点到加速到满速的位置
        private float warpStartTime;                    //起跳计时器

        private Vector3 warpEndAPos;                    //开始减速到0的位置
        private float warpEndTime;                      //减速计时器

        private float fullWarpDistance;                 //0速到慢速需要的加速距离

        private void Start()
        {
            fullWarpDistance = CalculteZeroToFullWarpDistance(maxWarpSpeed, fullWarpTime);
        }

        /// <summary>
        /// 计算从0到满速的匀速变速过程所需的行驶距离
        /// </summary>
        /// <param name="maxSpeed"></param>
        /// <returns></returns>
        private float CalculteZeroToFullWarpDistance(float maxSpeed,float time)
        {
            //S=vt-at^2/2  路程等于初速度乘以时间再减去加速度乘以（时间的平方）的二分之一
            return maxSpeed * time* 0.5f;
        }

        public void WarpTunnelStart(Vector3 target,Action onStart,Action onEnd)
        {
            this.target = target;
            realTarget = target;

            warpVel = (target - transform.position).normalized;
            var warpDistance = Vector3.Distance(transform.position, target);

            //真实跃迁目标点判断
            //小于最小跃迁距离则不跃迁
            if (warpDistance < minWarpDistance)
            {
                print("小于最小跃迁距离");
                return;
            }
            //大于最大跃迁距离则计算该方向上最大跃迁坐标
            if (warpDistance > maxWarpDistance)
            {
                realTarget = transform.position + warpVel * maxWarpDistance;
                //如果最终跳点和目标跳点距离小于minWarpDistance，则直接为目标跳点
                if (Vector3.Distance(realTarget, target) < minWarpDistance)
                {
                    realTarget = target;
                }
            }

            var realWarpDistance = Vector3.Distance(transform.position, realTarget);
            //计算加速减速的缓动位置
            //如果总跃迁距离小于加速减速距离之和，则取总跃迁距离中点进行加减速
            if (realWarpDistance < fullWarpDistance * 2)
            {
                warpStartBPos = transform.position + warpVel * realWarpDistance * 0.5f;
                warpEndAPos = warpStartBPos;
            }
            else
            {
                warpStartBPos = transform.position + warpVel * fullWarpDistance;
                warpEndAPos = transform.position + warpVel * (realWarpDistance - fullWarpDistance);
            }

            //计算匀速跃迁所需时间
            var warpMoveTime = Vector3.Distance(warpStartBPos, warpEndAPos) / maxWarpSpeed;

            //开始WarpStart缓动
            Tweener t1 = transform.DOMove(warpStartBPos, fullWarpTime).SetEase(Ease.InCubic).OnStart(()=> {
                warpStartTime = 0;
                //实例化特效
                nowWarpTunnelEffect = Instantiate(warpTunnelEffect, transform);
                nowWarpTunnelEffect.StartWarpTunnel(fullWarpTime/2);
                Tweener tLookAt = transform.DOLookAt(realTarget,1f);
                if (onStart != null) onStart.Invoke();
            }).OnUpdate(()=> {
                warpStartTime += Time.deltaTime;
                nowWarpSpeed = maxWarpSpeed * warpStartTime / fullWarpTime;
            }).OnComplete(()=> {
                //结束WarpStart缓动
                //进入匀速跃迁运动
                Tweener t2 = transform.DOMove(warpEndAPos, warpMoveTime).SetEase(Ease.Linear).OnComplete(() => {
                    //结束匀速跃迁运动
                    //开始warpEnd缓动
                    Tweener t3= transform.DOMove(realTarget, fullWarpTime).SetEase(Ease.OutCubic).OnStart(() =>
                    {
                        warpEndTime = fullWarpTime;
                        nowWarpTunnelEffect.EndWarpTunnel(fullWarpTime/2);
                    }).OnUpdate(() =>
                    {
                        warpEndTime -= Time.deltaTime;
                        nowWarpSpeed = maxWarpSpeed * warpEndTime / fullWarpTime;
                    }).OnComplete(() => {
                        //结束warpEnd缓动
                        print("Warp Complete");
                        Destroy(nowWarpTunnelEffect.gameObject);
                        if (onEnd != null) onEnd.Invoke();
                    });
                });
            });
        }

        private void OnDrawGizmos()
        {
            if (!showDebug) return;
            Gizmos.DrawSphere(realTarget, 5f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(warpStartBPos, 5f);
            Gizmos.DrawLine(transform.position, warpStartBPos);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(warpEndAPos, 5f);
            Gizmos.DrawLine(warpEndAPos, realTarget);
        }
    }
}
