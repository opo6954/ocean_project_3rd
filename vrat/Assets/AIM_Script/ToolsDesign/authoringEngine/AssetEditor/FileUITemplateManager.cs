using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * FileUITemplate에서 이름과 이미지 setting을 담당
     * */
    public class FileUITemplateManager : MonoBehaviour
    {
        UnityEngine.UI.Text myText;
        UnityEngine.UI.RawImage myImage;

        private readonly float xOffset = 150;
        private readonly float yOffset = -150;

        private readonly float xInitOffset = 10;

        //몇번째 template인지..
        public int idx;

        public delegate void OnClickCallback(int myIdx);
        public OnClickCallback callback;

        public UnityEngine.UI.Button b;



        void Start()
        {
            
        }

        

        public void setComponent()
        {
            myImage = GetComponent<UnityEngine.UI.RawImage>();
            myText = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        }

        


        //적당한 여백으로 배치하기
        //몇번째 녀석인지도 받음
        public void setPosition(int _idx)
        {
            idx = _idx;

            int tmpIdx = idx;

            //홀수번째일 경우 xOffset만큼 주기

            Vector3 position = new Vector3(xInitOffset,0,0);

            if (tmpIdx % 2 == 1)
            {
                position.x = position.x + xOffset;
                tmpIdx = tmpIdx - 1;
            }

            int idxOri = tmpIdx / 2;

            position.y = 0 + yOffset * idxOri;

            GetComponent<RectTransform>().localPosition = position;
        }


        public void turnColor(bool isSelect)
        {
            if (isSelect == true)
            {
                myImage.color = new Color( 0.63f, 0.62f, 1.0f,1.0f);
            }
                
            else
            {
                myImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
             
        }
         
        public void setOnClickListener(OnClickCallback _callback)
        {
            callback = _callback;
            b = GetComponent<UnityEngine.UI.Button>();
            b.onClick.AddListener(delegate{ callback(idx); });
            //transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate() { callback(idx); });
            
        }


        public bool setName(string _fileName)
        {
            
            if (myText == null)
            {
                return false;
            }

            myText.text = _fileName;
            return true;
        }

        public bool setImg(Texture2D _texture)
        {
            if (myImage == null)
            {
                return false;
            }

            myImage.texture = _texture;
            return true;
        }

    }
}