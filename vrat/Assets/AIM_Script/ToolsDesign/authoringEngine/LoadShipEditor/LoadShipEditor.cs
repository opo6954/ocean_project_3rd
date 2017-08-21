using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * IMPELMENT FOR LATER...
     * */
    public class LoadShipEditor : EditorTemplate
    {
        //load ship environment 버튼 누를 때의 callback 동작

        private readonly string tempEnv = "mapInfo/ShipFloor";
        

        Camera oriMainCam;
        [SerializeField]
        GameObject cameraControllerObj;

        [SerializeField]
        GameObject initEnv;

        [SerializeField]
        GameObject shipEnv;

        



        public void OnClickLoadShip()
        {
            cameraControllerObj.SetActive(true);
            cameraControllerObj.GetComponent<CameraController>().isGlobalCam = true;

            //프로토타입으로 load ship environment를 하자
            Debug.Log("Load ship environment~!~!");
            //LHW IMPLEMENTATION of loadship...


            uiSelectorHandler.turnOnEditorAll(true, false);
            uiSelectorHandler.turnOnEditorAll(false, true);

            //초기 환경 끄고
            initEnv.gameObject.SetActive(false);

            GameObject tmpEnv = GameObject.Instantiate(Resources.Load(tempEnv),shipEnv.transform) as GameObject;

            oriMainCam = Camera.main;

            Vector3 tmpRot = oriMainCam.transform.rotation.eulerAngles;

            tmpRot.x = 90.0f;

            oriMainCam.transform.rotation = Quaternion.Euler(tmpRot);
            

            //fpsChar.transform.Find("FirstPersonCharacter").GetComponent<Camera>();
            //일단 임시 선박 환경 불러오기
        }
    }
}