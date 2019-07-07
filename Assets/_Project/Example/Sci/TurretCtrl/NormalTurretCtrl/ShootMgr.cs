using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GPL
{
    public class ShootMgr : MonoBehaviour
    {
        public FireBase[] fires;
        public FireBase nowFire;
        public Text text;

        private int fireFlag;

        private void Start()
        {
            if (nowFire != null)
            {
                for (int i = 0; i < fires.Length; i++)
                {
                    if (fires[i] == nowFire)
                    {
                        fireFlag = i;
                        break;
                    }
                }
            }
            ShowTurret();
        }

        // Update is called once per frame
        void Update()
        {

            if (nowFire != null && Input.GetMouseButton(0))
            {
                nowFire.OnFire();
            }
            if (nowFire != null && Input.GetMouseButtonDown(0))
            {
                nowFire.StartFire();
            }
            if (nowFire != null && Input.GetMouseButtonUp(0))
            {
                nowFire.EndFire();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                fireFlag = fireFlag - 1 >= 0 ? fireFlag - 1: fires.Length-1;
                ShowTurret();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                fireFlag = fireFlag + 1 < fires.Length ? fireFlag + 1:0;
                ShowTurret();
            }
        }

        void ShowTurret()
        {
            foreach (var f in fires)
            {
                f.enabled = false;
                f.gameObject.SetActive(false);
            }
            nowFire = fires[fireFlag];
            nowFire.enabled = true;
            nowFire.gameObject.SetActive(true);
            text.text = nowFire.name;
        }
    }
}
