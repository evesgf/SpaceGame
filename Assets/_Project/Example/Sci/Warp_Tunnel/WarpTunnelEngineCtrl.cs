using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class WarpTunnelEngineCtrl : MonoBehaviour
    {
        public Transform[] targets;
        public WarpTunnelEngine warpTunnelEngine;

        private int nowTarget;

        // Start is called before the first frame update
        void Start()
        {
            if (warpTunnelEngine == null)
                warpTunnelEngine = GetComponent<WarpTunnelEngine>();

            nowTarget = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                warpTunnelEngine.WarpTunnelStart(targets[nowTarget].position,null,()=> {
                    nowTarget += 1;
                    if (nowTarget > targets.Length - 1) nowTarget = 0;
                });
            }
        }
    }
}