using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public class NormalShipCtrl : MonoBehaviour
    {
        internal Vector3 input;
        internal ShipMove shipMove;

        // Start is called before the first frame update
        void Start()
        {
            shipMove = GetComponent<ShipMove>();
        }

        // Update is called once per frame
        void Update()
        {
            input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));
        }

        private void FixedUpdate()
        {
            shipMove.OnMove(input); 
        }
    }
}
