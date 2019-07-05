using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GPL
{
    public class SimpleTurretUI : MonoBehaviour
    {
        public Transform realTargetIcon;
        public Text txtAngle;

        private SimpleTurretCtrl simpleTurretCtrl;

        // Start is called before the first frame update
        void Start()
        {
            simpleTurretCtrl = GetComponent<SimpleTurretCtrl>();
        }

        // Update is called once per frame
        void Update()
        {
            txtAngle.text = simpleTurretCtrl.GetAngleToTarget().ToString("f2");

            //显示炮管真实指向
            var screenPos = simpleTurretCtrl.GetTargetDistance();
            realTargetIcon.position = new Vector3(screenPos.x, screenPos.y, 0);
        }
    }
}
