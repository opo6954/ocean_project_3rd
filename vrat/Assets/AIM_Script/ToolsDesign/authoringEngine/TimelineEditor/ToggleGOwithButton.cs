using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    public class ToggleGOwithButton : MonoBehaviour
    {
        [SerializeField]
        GameObject target;

        void Start()
        {
            target.SetActive(false);
        }

        public void OnToggleGO()
        {
            target.SetActive(!target.activeSelf);
        }

    }
}