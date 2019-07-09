using UnityEngine;
using System.Collections;

namespace Forge3D
{
    public class F3DWarpJump : MonoBehaviour
    {

        public ParticleSystem WarpSpark;
        public Transform ShipPos;
        public float ShipJumpSpeed;
        public Vector3 ShipJumpStartPoint;
        public Vector3 ShipJumpEndPoint;
        public bool SendOnSpawned;

        bool isWarping;

        // Use this for initialization
        void Start()
        {
            if (SendOnSpawned)
                BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
        }

        public void OnSpawned()
        {
            isWarping = false;
            WarpSpark.transform.localPosition = ShipJumpStartPoint;
            ShipPos.position = WarpSpark.transform.position;
            F3DTime.time.AddTimer(3, 1, OnWarp);
        }

        void OnWarp()
        {
            isWarping = true;
        }

        void ShiftShipPosition()
        {
            WarpSpark.transform.localPosition = Vector3.Lerp(WarpSpark.transform.localPosition, ShipJumpEndPoint,
                Time.deltaTime*ShipJumpSpeed);
            ShipPos.position = WarpSpark.transform.position;
        }

        void Update()
        {
            if (isWarping)
                ShiftShipPosition();
        }
    }
}