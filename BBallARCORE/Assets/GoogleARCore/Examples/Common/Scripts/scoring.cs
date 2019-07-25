namespace GoogleARCore.Examples.Common
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class scoring : MonoBehaviour
    {
        public GameObject effect;

        void Update()
        {
            effect.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (Shot.shot)
            {
                Shot.made();
                effect.SetActive(true);
            }
        }
    }
}