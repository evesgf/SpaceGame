using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GPL
{
    public class NormalTurretUI : MonoBehaviour
    {
        public Transform realTargetIcon;
        public Text txtAngle;
        public Image pointLine;

        private NormalTurretCtrl normalleTurretCtrl;

        // Start is called before the first frame update
        void Start()
        {
            normalleTurretCtrl = GetComponent<NormalTurretCtrl>();
        }

        // Update is called once per frame
        float hypotenuse,angle;
        void Update()
        {
            if (txtAngle != null)
                txtAngle.text = normalleTurretCtrl.GetAngleToTarget().ToString("f2");

            if (realTargetIcon != null)
            {
                //显示炮管真实指向
                var screenPos = Camera.main.WorldToScreenPoint(normalleTurretCtrl.RealTargetPos);
                realTargetIcon.position = new Vector3(screenPos.x, screenPos.y, 0);
            }

            if (normalleTurretCtrl.GetAngleToTarget() > 1f)
            {
                pointLine.gameObject.SetActive(true);

                //计算角度
                //两点的x、y值
                float x = realTargetIcon.position.x - Input.mousePosition.x;
                float y = realTargetIcon.position.y - Input.mousePosition.y;

                //斜边长度
                hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));
                pointLine.rectTransform.sizeDelta = new Vector2(hypotenuse, pointLine.rectTransform.sizeDelta.y);

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

