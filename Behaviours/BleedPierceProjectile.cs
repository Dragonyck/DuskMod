using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DuskMod
{
    class BleedPierceProjectile : MonoBehaviour
    {
        public void OnDisable()
        {
            Destroy(this);
        }
    }
}
