using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class UISelectorTemplate : MonoBehaviour
    { 
        protected List<GameObject> childList = new List<GameObject>();

        public virtual void initialize()
        {
            //만일 child 없을 경우 걍 종료함
            if (gameObject.transform.childCount == 0)
                return;

            //자신의 child 모두 불러와서 list 형태로 저장하기(끄고 켤 때 필요함)
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                childList.Add(transform.GetChild(i).gameObject);
            }
        }



        public virtual void OnEnterCertainEditor(string _type)
        {

        }

        // Use this for initialization
        void Start()
        {
            initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}