using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{

    public class SimpleTurretLauncher : MonoBehaviour
    {
        public LayerMask targetLayer;

        public Transform target;
        public Transform targetIcon;
        public SimpleTurretCtrl[] turrets;

        private Vector3 targetPos;
        private RaycastHit hit;

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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
                {
                    targetPos = hit.point;
                }
                else
                {
                    targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
                }

            }
            foreach (var turret in turrets)
            {
                turret.target = targetPos;
            }

            var pos = Camera.main.WorldToScreenPoint(targetPos);
            targetIcon.position = new Vector3(pos.x, pos.y, 0);
        }
    }
}