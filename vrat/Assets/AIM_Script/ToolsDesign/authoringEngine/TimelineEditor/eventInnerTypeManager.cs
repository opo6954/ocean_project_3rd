using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    //event template에서 내부 inner type에서의 연결을 담당함

    public static class PRIMColor
    {
        public static Color noneAttach = new Color(127,127,127);

        public static Color triggerColor = new Color(0,149,255);

        public static Color actionColor = new Color(214,91,0);

        public static Color instColor = new Color(165,167,0);

    }
    
    public class eventInnerTypeManager : MonoBehaviour
    {
        //primitives이름이 들어간 버튼을 의미
        [SerializeField]
        GameObject primButton;

        [SerializeField]
        GameObject primImgObject;

        [SerializeField]
        PRIMITIVESUI myType;

        [SerializeField]
        PRIMDETECTDROP myTypeForEvent;


        [SerializeField]
        private UnityEngine.UI.RawImage assetImg;


        public delegate void OnClickAssetImg(int orderIdx, PRIMDETECTDROP _type);
        public OnClickAssetImg callback;

        public void setOnClickAssetImg(OnClickAssetImg _callback)
        {
            callback = _callback;
        }
        

        

        public bool isDetectDrop(Vector2 position)
        {
            

            var rt = primButton.GetComponent<RectTransform>();
            Vector2 localPos = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, position, Camera.main, out localPos);

            //if (RectTransformUtility.RectangleContainsScreenPoint(rt, localPos, Camera.main) == true)
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, position, Camera.main) == true)
            {
                Debug.Log("Detect true on " + myType.ToString());
                return true;
            }
            return false;
        }

        //만일 드랍을 해서 붙여진 경우에 혹은 x버튼 눌러서 지울 시 불려짐
        //texture도 바꿔야 할 듯 연결된 asset의 texture로...

        public virtual void isAssetImgClick(int clickedOrderIdx)
        {
            callback(clickedOrderIdx, myTypeForEvent);
        }

        public void setAttached(bool isSelect)
        {
            changeColor(isSelect);    
        }


        void changeColor(bool isSelect)
        {
            //button의 색깔 바꾸기
            //true일 경우 붙이기 with texture 2D
            if (isSelect == true)
            {
                switch (myType)
                {
                    case PRIMITIVESUI.TRIGGER:
                        primButton.GetComponent<UnityEngine.UI.Image>().color = PRIMColor.triggerColor;
                        break;
                    case PRIMITIVESUI.ACTION:
                        primButton.GetComponent<UnityEngine.UI.Image>().color = PRIMColor.actionColor;
                        break;
                    case PRIMITIVESUI.INSTRUCTION:
                        primButton.GetComponent<UnityEngine.UI.Image>().color = PRIMColor.instColor;
                        break;
                }
            }
            else
            {
                primButton.GetComponent<UnityEngine.UI.Image>().color = PRIMColor.noneAttach;
            }
            //false일 경우 붙이기 취소 with color만 바꾸기
        }

        

        //asset image 바꾸기
        public virtual void setAssetImg(Texture2D _image)
        {
            assetImg.texture = _image;
            
        }
        //asset image 지우기
        public virtual void deleteAssetImg(int assetIdx)
        {
            assetImg.texture = null;
        }

        
        

    }
}