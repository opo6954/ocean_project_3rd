using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        Camera mainCam;

        private readonly float xOffset = 5;
        private readonly float yOffset = 5;

        private readonly float zoomOffset = 5;

        public bool isGlobalCam;

        void Start()
        {
            isGlobalCam = true;
        }


        public void moveCameraPos(bool isXAxis, float offset)
        {
            if (isGlobalCam == true)
            {
                Vector3 oriPos = mainCam.transform.position;
                if (isXAxis == true)
                {
                    oriPos.x = oriPos.x + offset;
                }
                else
                    oriPos.z = oriPos.z + offset;

                mainCam.transform.position = oriPos;
            }
        }

        public void OnClickPosXInc()
        {
            moveCameraPos(true, xOffset);
        }

        public void OnClickPosXDec()
        {
            moveCameraPos(true, -xOffset);
        }

        public void OnClickPosYInc()
        {
            moveCameraPos(false, yOffset);
        }

        public void OnClickPosYDec()
        {
            moveCameraPos(false, -yOffset);
        }

        public void OnClickZoomIn()
        {
            Vector3 oriPos = mainCam.transform.position;
            oriPos.y = oriPos.y - zoomOffset;
            mainCam.transform.position = oriPos;
        }
        public void OnClickZoomOut()
        {
            Vector3 oriPos = mainCam.transform.position;
            oriPos.y = oriPos.y + zoomOffset;
            mainCam.transform.position = oriPos;
        }

        public void checkMouseScrollWheel()
        {
            if (isGlobalCam == true)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    OnClickZoomIn();
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    OnClickZoomOut();
            }
        }

        void Update()
        {
            checkMouseScrollWheel();
        }
    }
}