using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPL
{
    public abstract class FireBase : MonoBehaviour
    {
        public virtual void OnFire() { }

        public virtual void StartFire() { }

        public virtual void EndFire() { }
    }
}
