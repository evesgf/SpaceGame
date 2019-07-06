using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class NormalShipCtrl_Fire : MonoBehaviour
    {
        public LayerMask targetLayer;

        public Transform target;
        public Transform targetIcon;
        public NormalTurretCtrl[] turrets;

        private Vector3 targetPos;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                targetPos = target.position;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f,0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
                {
                    targetPos = hit.point;
                }
                else
                {
                    targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f, Screen.height*0.5f, Camera.main.farClipPlane));
                }

            }
            foreach (var turret in turrets)
            {
                turret.target = targetPos;
            }

            var pos = Camera.main.WorldToScreenPoint(targetPos);
            if(targetIcon!=null)
                targetIcon.position = new Vector3(pos.x, pos.y, 0);
        }
    }
}
