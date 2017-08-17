using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;


namespace vrat
{

    public class AssetEditor : EditorTemplate
    {
        GameObject rootUIObj;
        

        string m_textPath;

        void Start()
        {
        }

        //path 버튼 누를 때의 directory 이동
        public void OnClickPathBox(string buttonIdx)
        {
            
        }

        //file을 더블클릭 했을 때의 callback
        //File일 경우 select, directory일 경우 그 directory 탐색을 함
        //eventData에서 click count를 보면 됨
        public void OnDoubleClickFileName(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                Debug.Log("Double click...");
            }
        }


        protected override void OnClickAuthoring()
        {
            /*
            m_fileBrowser = new FileBrowser(
              new Rect(10, 10, 1000, 500),
              "Choose Text File",
              FileSelectedCallback
          );
            m_fileBrowser.SelectionPattern = "*.png";
            //m_fileBrowser.SelectionPattern = "*.txt";
             * */
        }

        protected void FileSelectedCallback(string path)
        {
            
        }
    }
}