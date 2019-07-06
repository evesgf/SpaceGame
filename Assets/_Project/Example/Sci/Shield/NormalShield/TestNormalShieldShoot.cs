using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class TestNormalShieldShoot : MonoBehaviour
    {
        public GameObject bullet;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = ray.GetPoint(10);
                var b = Instantiate(bullet, transform.position, transform.rotation);
                b.GetComponent<Rigidbody>().velocity = target - transform.position;
            }
        }
    }
}
