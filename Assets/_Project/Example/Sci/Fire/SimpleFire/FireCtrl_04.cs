using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class FireCtrl_04 : MonoBehaviour
    {
        public LayerMask targetLayer;

        public Bullet_04 bullet;

        public Transform Muzzle;

        private RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }

        public void Fire()
        {
            var b = Instantiate(bullet, Muzzle.position, Muzzle.rotation);
            b.Init();
        }
    }
}
