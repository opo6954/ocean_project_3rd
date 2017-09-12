using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * authoring mode에서의 각각의 editor의 UI의 끄고 켜짐을 수행하는 녀석
 * 
 * 얘도 일단 다시 짜자 - 170911
 * */

namespace vrat
{

    public class UISelector : MonoBehaviour
    {

        List<GameObject> UIEditorList = new List<GameObject>();
        List<GameObject> UIEditorSelectorList = new List<GameObject>();

        [SerializeField]
        GameObject cameraControllerObj;

        

        void Start()
        {
            //tag name 형태로 데이터 관리

            GameObject uiHandler = GameObject.FindGameObjectWithTag("EditorType");
            GameObject uiSelectorHandler = GameObject.FindGameObjectWithTag("EditorSelector");




            for (int i = 0; i < uiHandler.transform.childCount; i++)
            {
                UIEditorList.Add(uiHandler.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < uiSelectorHandler.transform.childCount; i++)
            {
                UIEditorSelectorList.Add(uiSelectorHandler.transform.GetChild(i).gameObject);
            }

            /*
            turnOnEditorAll(true, false);
            turnOnEditorAll(false, false);
            */
            
        }
        public void OnClickAuthoringButton(string msg)
        {
            for (int i = 0; i < UIEditorList.Count; i++)
            {
                if (UIEditorList[i].name.Contains(msg) == true)
                {
                    turnOnEditor(UIEditorList[i].name);
                }
            }

            if (msg == "In-situ")
            {
                turnCameraController(false);

                OnEnableButton(false);
                
                
            }
            else
            {
                turnCameraController(true);

                OnEnableButton(true);
            }
        }

        public void OnEnableButton(bool isTurn)
        {
            if (isTurn == true)
            {
                foreach (GameObject go in UIEditorSelectorList)
                {
                    go.GetComponent<UnityEngine.UI.Button>().interactable = true;
                }
            }
            else
            {
                foreach (GameObject go in UIEditorSelectorList)
                {
                    go.GetComponent<UnityEngine.UI.Button>().interactable = false;
                }
            }
        }

        public void turnCameraController(bool isTurn)
        {
            if (isTurn == true)
            {
                cameraControllerObj.SetActive(true);
                cameraControllerObj.GetComponent<CameraController>().isGlobalCam = true;
            }
            else
            {
                cameraControllerObj.GetComponent<CameraController>().isGlobalCam = false;
                cameraControllerObj.SetActive(false);
            }
        }

        //특정 editor 켜기(index로 받음)
        public bool turnOnEditor(int idx)
        {
            turnOnEditorAll(true, false);

            if (idx > UIEditorList.Count)
            {
                return false;
            }

            UIEditorList[idx].SetActive(true);

            return true;
        }
        //특정 editor 켜기(이름으로 받음)

        public GameObject getUIHandler(string editorName)
        {
            for (int i = 0; i < UIEditorList.Count; i++)
            {
                if (UIEditorList[i].name == editorName)
                {
                    return UIEditorList[i];
                }
            }

            return new GameObject();
        }

        public bool turnOnEditor(string editorName)
        {
            turnOnEditorAll(true, false);

            for (int i = 0; i < UIEditorList.Count; i++)
            {

                if (UIEditorList[i].name == editorName)
                {
                    UIEditorList[i].SetActive(true);
                    return true;
                }
            }
            return false;
        }

        public void turnOnEditorAll(bool isEditorUI, bool isOn)
        {
            //for turning on editorUI
            if (isEditorUI == true)
            {
                for (int i = 0; i < UIEditorList.Count; i++)
                {
                    UIEditorList[i].SetActive(isOn);
                }
            }
            //for turning on editorSelector
            else
            {
                for (int i = 0; i < UIEditorSelectorList.Count; i++)
                {
                    UIEditorSelectorList[i].SetActive(isOn);
                }
            }
        }
    }
}