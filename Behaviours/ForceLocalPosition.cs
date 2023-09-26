using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DuskMod
{
    class ForceLocalPosition : MonoBehaviour
    {
        public Vector3 localPos;
        public void Update()
        {
            if (base.transform.localPosition != localPos)
            {
                base.transform.localPosition = localPos;
            }
        }
    }
}
