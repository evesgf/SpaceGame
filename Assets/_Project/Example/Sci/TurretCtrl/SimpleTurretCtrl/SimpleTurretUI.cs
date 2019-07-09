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
        public Image pointLine;

        private SimpleTurretCtrl simpleTurretCtrl;
        private float hypotenuse;
        private float angle;

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
            var screenPos = Camera.main.WorldToScreenPoint(simpleTurretCtrl.GetRealTargetPos());
            realTargetIcon.position = new Vector3(screenPos.x, screenPos.y, 0);

            if (simpleTurretCtrl.GetAngleToTarget() > 1f)
            {
                pointLine.gameObject.SetActive(true);

                //计算角度
                //两点的x、y值
                float x = realTargetIcon.position.x - Input.mousePosition.x;
                float y = realTargetIcon.position.y - Input.mousePosition.y;

                //斜边长度
                hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f)); pointLine.rectTransform.sizeDelta = new Vector2(hypotenuse*0.5f, pointLine.rectTransform.sizeDelta.y);

                //求出弧度
                float cos = x / hypotenuse;
                float radian = Mathf.Acos(cos);

                //用弧度算出角度    
                angle = 180 / (Mathf.PI / radian);

                if (y < 0)
                {
                    angle = -angle;
                }
                else if ((y == 0) && (x < 0))
                {
                    angle = 180;
                }

                pointLine.transform.localRotation = Quaternion.Euler(0, 0, angle);

            }
            else
            {
                pointLine.gameObject.SetActive(false);
            }
        }
    }
}
