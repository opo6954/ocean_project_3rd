using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * Locate the asset with raycast
     * */
    public class LocateAssetWithRay : MonoBehaviour
    {
        GameObject targetAsset;

        [SerializeField]
        Camera myCamera;

        public delegate void OnCallbackLocateDone(Vector3 pos, Vector3 rot);

        public OnCallbackLocateDone callback;






        float range=30f;
        float rotYOffset = 5.0f;

        bool isStart = false;
        bool isLocate = false;

        void Start()
        {
            

        }
        public void clear()
        {
            isStart = false;
            isLocate = false;


            Debug.Log("Init on LocateAssetWithRay");

        }

        public void stopLocate()
        {
            isStart = false;
        }
        
        public void startLocate(string assetName, OnCallbackLocateDone _callback)
        {
            callback = _callback;



            targetAsset = transform.Find("AssetTarget").transform.Find(assetName).gameObject;


            
            Vector3 rayOrigin = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
            RaycastHit hit = new RaycastHit();

            isStart = true;

        }
        public void callbackCancel()
        {
            clear();
        }

        //locate버튼 누를 시의 callback
        public void callbackLocate()
        {
            isLocate = true;
        }
        //rotate버튼 누를 시의 callback
        public void callbackRotate()
        {
            Vector3 angle = targetAsset.transform.rotation.eulerAngles;

            angle.y = angle.y + rotYOffset;

            targetAsset.transform.rotation = Quaternion.Euler(angle);
        }

        

        void Update()
        {
            if (isStart == true)
            {
                
                Vector3 rayOrigin = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

                RaycastHit hit;

                if(Physics.Raycast(rayOrigin,myCamera.transform.forward, out hit, range))
                {
                    targetAsset.transform.position = hit.point;
                }
                else
                {
                    targetAsset.transform.position = rayOrigin + (myCamera.transform.forward * range);
                }
                if (isLocate == true)
                {
                    //callback함수 부르기
                    callback(targetAsset.transform.position, targetAsset.transform.rotation.eulerAngles);
                    

                    clear();
                    
                }

            }
        }

    }
}