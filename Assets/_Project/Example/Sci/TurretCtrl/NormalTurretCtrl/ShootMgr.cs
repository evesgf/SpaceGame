using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class ShootMgr : MonoBehaviour
    {
        public FireBase[] fires;
        public FireBase nowFire;

        // Update is called once per frame
        void Update()
        {
            if (nowFire != null && Input.GetMouseButton(0))
            {
                nowFire.OnFire();
            }
        }
    }
}
