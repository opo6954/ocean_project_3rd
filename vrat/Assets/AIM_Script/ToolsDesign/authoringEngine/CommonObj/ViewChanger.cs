using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace vrat
{
    /*
     * 1인칭, 3인칭 시점을 바꾸는 녀석
     * 1인칭 fps에 붙어 있음
     * 일단 v키 누르면 인칭 바뀌는 걸로 임시로 넣자
     * */
    public class ViewChanger : MonoBehaviour
    {
        //3인칭 카메라
        [SerializeField]
        Camera global_camera;

        [SerializeField]
        AudioListener global_audio;

        //1인칭 카메라
        [SerializeField]
        Camera fps_camera;

        [SerializeField]
        CharacterController fps_cc;

        [SerializeField]
        FirstPersonController fps_fpc;

        [SerializeField]
        AudioListener fps_audio;

        [SerializeField]
        LocateAssetWithRay lawr;

        
        //최근의 camera
        bool isFpsCurr;

        KeyCode changeViewKey;


        public bool getInput()
        {
            
            return Input.GetKeyDown(changeViewKey);
        }

        public void turnGlobal(bool isOn)
        {
            global_camera.enabled = isOn;
            global_audio.enabled = isOn;
        }

        public void turnFPS(bool isOn)
        {
            fps_audio.enabled = isOn;
            fps_camera.enabled = isOn;
            fps_cc.enabled = isOn;
            fps_fpc.enabled = isOn;
        }

        private void Start()
        {
            changeViewKey = KeyCode.LeftControl;
            isFpsCurr = false;

            turnGlobal(true);
            turnFPS(false);

        }

        private void Update()
        {
            
            if (getInput() == true)
            {
                changeView();
                lawr.stopLocate();
            }
            
        }

        public bool changeView()
        {

            //third --> first
            if (isFpsCurr == false)
            {
                turnGlobal(false);
                turnFPS(true);
                isFpsCurr = true;
            }
            //first --> third
            else
            {
                turnFPS(false);
                turnGlobal(true);
                isFpsCurr = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            return true;
        }
    }
}