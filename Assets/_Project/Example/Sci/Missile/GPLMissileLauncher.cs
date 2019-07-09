using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class GPLMissileLauncher : MonoBehaviour
    {
        public Transform target;
        public FireBase[] fires;
        public NormalTurretCtrl[] normalTurretCtrls;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var t in normalTurretCtrls)
            {
                t.target = target.position;
            }

            if (Input.GetMouseButtonDown(0))
            {
                foreach (var f in fires)
                {
                    f.target = target;
                    f.OnFire();
                }
            }
        }
    }
}
