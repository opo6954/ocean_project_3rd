using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * Locate the asset with raycast
     * 
     * Locate될 때만 callback으로 그 위치를 넘겨주면 됨
     * 나머지 경우는 모두 여기서 처리하자 locate of object 등
     * 
     * 일단 object 상대로 raycast 성공할 시 parent를 assetTarget으로 바꿔주기
     * 이후 target asset을 raycast의 끝부분에 위치시키면 됨
     * 
     * 
     * */
    public class LocateAssetWithRay : MonoBehaviour
    {
        GameObject targetAsset;

        private string location_button = "z";
        private string rotate_button = "c";
        private string scale_up_button = "q";
        private string scale_down_button = "e";


        [SerializeField]
        Camera myCamera;

        [SerializeField]
        GameObject concentrateMark;

        public delegate void OnCallbackLocateDone(Vector3 pos, Vector3 rot);

        public OnCallbackLocateDone callback;
        
        float range=10.0f;
        float rotYOffset = 5.0f;
        float scaleOffset = 0.01f;

        bool isStart = false;
        bool isHold = false;
        
        
        //1인칭에서 asset의 부모가 되는 gameobject --> assetTarget
        [SerializeField]
        public GameObject insituParent;

        //3인칭에서 asset의 부모가 되는 gameobject --> environment
    
        public GameObject worldParent;

        //현재 선택된 placeable의 index를 말함
        int currPlaceableIdx;

        


        void Start()
        {
            
        }

        public void setCurrPlaceableIdx(int _idx)
        {
            currPlaceableIdx = _idx;
        }

        public void clear()
        {
            isStart = false;
            isHold = false;           


            Debug.Log("Init on LocateAssetWithRay");
        }

        public void stopLocate()
        {
            isStart = false;
            isHold = false;

            

        }

        
        public void startLocate(GameObject _worldParent, GameObject _targetAsset, OnCallbackLocateDone _callback)
        {
            callback = _callback;

            worldParent = _worldParent;
            targetAsset = _targetAsset;

            Debug.Log("hihi " + _targetAsset.name);

            isStart = true;
        }

        //obj를 집을 때
        public void holdObj()
        {
            //쥘 때 1인칭 부모로 바꿔주기
            targetAsset.transform.parent = insituParent.transform;

            targetAsset.gameObject.layer = 2;

            targetAsset.transform.GetComponentInChildren<Collider>().gameObject.layer = 2;
            

            //집중 표식을 0,0,0으로 놓기
            concentrateMark.transform.position = new Vector3();



            isHold = true;
        }
        //obj를 놓을 때
        public void releaseObj()
        {
            //놓을 때 3인칭 부모로 바꿔주기
            targetAsset.transform.parent = worldParent.transform;

            targetAsset.gameObject.layer = 0;
            targetAsset.transform.GetComponentInChildren<Collider>().gameObject.layer = 0;

            //놓았으니 callback 부르기
            callback(targetAsset.transform.localPosition, targetAsset.transform.localRotation.eulerAngles);

            Debug.Log(targetAsset.transform.localPosition);
            Debug.Log(targetAsset.transform.localRotation.eulerAngles);


            isHold = false;
        }

        
        //rotate버튼 누를 시의 메소드
        public void DoRotate()
        {
            Vector3 angle = targetAsset.transform.rotation.eulerAngles;

            angle.y = angle.y + rotYOffset;

            targetAsset.transform.rotation = Quaternion.Euler(angle);
        }
        //Scale버튼 누를 시의 메소드
        public void DoScale(bool isScaleUp)
        {
            float off = scaleOffset;

            if (isScaleUp == false)
                off = off * -1;

                
            Vector3 oriScale = targetAsset.transform.localScale;

            oriScale.x = oriScale.x + off;
            oriScale.y = oriScale.y + off;
            oriScale.z = oriScale.z + off;

            targetAsset.transform.localScale = oriScale;
        }

        

        void Update()
        {
            if (isStart == true)
            {
                Vector3 rayOrigin = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

                RaycastHit hit;
                
                if(Physics.Raycast(rayOrigin, myCamera.transform.forward, out hit, range))
                {
                    //raycast 성공, 그리고들고 있을 경우
                    if (isHold == true)
                    {
                        targetAsset.transform.position = hit.point;
                    }
                    //raucast 성공, 들고 있지 않은 경우
                    else
                    {
                        
                        concentrateMark.transform.position = hit.point;
                        
                    }
                }
                else
                {
                    //raycast 실패, 들고 있을 경우
                    if (isHold == true)
                    {
                        targetAsset.transform.position = rayOrigin + (myCamera.transform.forward * range);
                    }
                    //raycast 성공, 들고 있지 않을 경우
                    else
                    {
                        
                        concentrateMark.transform.position = rayOrigin + (myCamera.transform.forward * range);
                        
                    }
                }

                //만일 locate 버튼을 눌렀을 경우
                if (Input.GetKeyDown(location_button) == true)
                {
                    
                    //잡고 있을 시 obj 풀기
                    if (isHold == true)
                    {
                        releaseObj();
                    }
                    else
                    {
                        //놓고 있을 시 제대로 바라보고 있으면 잡기
                        if (Physics.Raycast(rayOrigin, myCamera.transform.forward, out hit, range))
                        {
                            Debug.Log(hit.collider.name);
                            Debug.Log(targetAsset.transform.name);

                            if (hit.transform.name.Contains(targetAsset.transform.name) == true)
                            {
                                holdObj();
                                    
                            }                            
                        }
                    }
                    
                }

                if (Input.GetKeyDown(rotate_button) == true)
                {
                    //내가 잡고 있을 때만 rotation 하기
                    if(isHold == true)
                        DoRotate();
                }

                if (Input.GetKeyDown(scale_up_button) == true)
                {
                    if (isHold == true)
                        DoScale(true);
                }
                else if (Input.GetKeyDown(scale_down_button) == true)
                {
                    if (isHold == true)
                        DoScale(false);
                }
            }
        }

    }
}