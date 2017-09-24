using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace vrat
{
    /*
     * FileUITemplate에서 이름과 이미지 setting을 담당
     * */
    public class FileUITemplateManager : MonoBehaviour
    { 
        [SerializeField]
        UnityEngine.UI.Text myText;
        
        [SerializeField]
        UnityEngine.UI.Image myImage;
         
        protected float xOffset = 150;
        protected float yOffset = -150;

        protected float xInitOffset = 10;

        //몇번째 template인지..
        public int idx;

        public delegate void OnClickCallback(int myIdx);
        public OnClickCallback callback;

        public delegate void OnDragCallback(int myIdx);
        public OnDragCallback callbackDrag;

        public Texture2D currImg;


        public UnityEngine.UI.Button button;


        void Start()
        {
            xOffset = 150;
            yOffset = -150;
            xInitOffset = 10;
        }
        public void setIdx(int _idx)
        {
            idx = _idx;
        }

        //적당한 여백으로 배치하기
        //몇번째 녀석인지도 받음
        public virtual void setPosition(int _idx)
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
            button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(delegate{ callback(idx); });
        }
         
        public virtual void setOnDragListener(OnDragCallback _callback)
        {
            callbackDrag = _callback;
        }

        //drag start할 떄의 event
        //assetlist --> primitives로 갈 수도 있고
        //primitives --> timeline으로 갈 수도 있다.
        public virtual void OnDragStart()
        {
            //drag callback 전해주기
            callbackDrag(idx);
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

            currImg = _texture;

            myImage.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
            return true;
        }

    }
}