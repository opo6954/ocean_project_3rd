using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    /*
     * primitves 소환할 때 innerType의 container 역할을 함
     * 
     * 가능한 trigger type을 불러와서 버튼 형태로 모두 표시하고 버튼 클릭시 창이 사라지고 primitves에 respawn된다
     * 
     * */
    public class innerTypeContainer : MonoBehaviour
    {
        int idx;
        [SerializeField]
        UnityEngine.UI.Text myPrimitvesTypeLabel;

        [SerializeField]
        UnityEngine.UI.Button button;

        public delegate void OnClickSecondPhase(int idx);
        OnClickSecondPhase callback;

        float yOffset = -30;

        //내가 몇 번째 primitives인지 설정하기
        public void setIdx(int _idx)
        {
            idx = _idx;
        }

        public void setOnClickListener(OnClickSecondPhase _callback)
        {
            callback = _callback;
            button.onClick.AddListener(()=> {callback(idx);});
        }

        //나의 type 설정하기
        public void setText(string _text)
        {
            myPrimitvesTypeLabel.text = _text;
        }
        //primitives의 숫자 및 순서에 맞게 버튼 소환해야 함
        public void setPositioning(int idx)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, idx * yOffset);
        }

    }
}