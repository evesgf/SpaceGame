using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class GPLMissileLauncher : MonoBehaviour
    {
        public Transform missilePrefab;
        public Transform muzzlePos;
        public Transform target;
        public MissileBase.MissileType missileType;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(CreateMissileList());
            }
        }

        IEnumerator CreateMissileList()
        {
            for (int i = 0; i < 10; i++)
            {
                CreateMissile();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void CreateMissile()
        {
            var m = Instantiate(missilePrefab, muzzlePos.position, muzzlePos.rotation);
            var mBase = m.GetComponent<MissileBase>();
            mBase.Init();
            if (target != null) mBase.target = target;
            mBase.missileType = missileType;
        }
    }
}
