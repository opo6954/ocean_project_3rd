using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    public class innerTypeContainer : MonoBehaviour
    {
        int idx;
        [SerializeField]
        UnityEngine.UI.Text myText;

        public void setIdx(int _idx)
        {
            idx = _idx;
        }

        public void setText(string _text)
        {
            myText.text = _text;
        }

    }
}