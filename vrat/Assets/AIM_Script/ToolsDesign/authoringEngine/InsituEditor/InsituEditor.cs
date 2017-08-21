using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace vrat
{
    /*
     * In-situ authoring임
     * 
     * 필요한 기능들:
     * Location 완료기능 --> 일단 a버튼으로 해결
     * Object의 Rotation 기능 --> c버튼 누르면 rotation 진행됨(일단 한쪽 방향으로만 하자)
     * Asset보고 선택하는 기능 --> b버튼 누르면 asset 창이 나오고 거기서 c버튼으로 이동하고 a버튼으로 선택하기
     * Location 완료시에 .Asset 열어서 location 수정하는 기능
     * 
     * */
    public class InsituEditor : EditorTemplate
    {
        private readonly string locatoin_button = "z";
        private readonly string showAsset_button = "x";
        private readonly string rotate_button = "c";
        private readonly string exit_button = "v";
        private readonly string explore_button = "e";

        private readonly string characterPath = "EnvironmentEditor/FPSController";
        private readonly string prefabPath = "AssetFiles/prefab/";

        GameObject fpsChar;

        bool isSetCharacter = false;
        bool isLoadAllAsset = false;

        [SerializeField]
        GameObject mainCamera;

        [SerializeField]
        GameObject environment;

        [SerializeField]
        GameObject assetWindow;

        [SerializeField]
        SubwindowManager subWindow;

        bool isShownAssetWindow = false;
        bool isSelectAsset = false;

        int assetNumber;

        int currAssetIdxPrepare = 0;
        int currAssetIdxSelect = 0;


        DirectoryUIHandler_Insitu dirHandler;

        LocateAssetWithRay lasr;

        //전체 asset list를 가지고 있음, 일단 배치를 하기 위해서
        AuthorableAsset[] assetList;

        //전체 asset의 이름 list를 가지고 있음, 
        string[] assetFullNameList;

        //맵에 배치된 prefab list를 가지고 있음
        
        GameObject[] currLocatedAsset;
        

        //In-situ mode 시작

        public void clear()
        {
            if (isSetCharacter == false)
            {
                fpsChar = GameObject.Instantiate(Resources.Load(characterPath), new Vector3(10, 10, 10), new Quaternion(), environment.transform) as GameObject;
                isSetCharacter = true;
            }
            else
            {
                fpsChar.SetActive(true);
                lasr = fpsChar.GetComponent<LocateAssetWithRay>();
                lasr.clear();
            }


            showAssetWindow(false);

            dirHandler = assetWindow.transform.Find("AssetListView").GetComponent<DirectoryUIHandler_Insitu>();

            assetNumber = dirHandler.assetNameList.Count;

            assetWindow.SetActive(false);

            //mainCamera를 잠시동안 꺼놓는다
            mainCamera.GetComponent<AudioListener>().enabled = false;
            mainCamera.GetComponent<Camera>().enabled = false;
        }

        public void OnClickInSituMode()
        {
            Debug.Log("Enter InsituMode...");
            
            clear();
            if (isLoadAllAsset == false)
            {
                dirHandler.loadAllPlacableAsset();
                isLoadAllAsset = true;
            }
        }

        public void OnOutofSituMode()
        {
            fpsChar.SetActive(false);

            



            mainCamera.GetComponent<AudioListener>().enabled = true;
            mainCamera.GetComponent<Camera>().enabled = true;

            uiSelectorHandler.turnCameraController(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            uiSelectorHandler.OnEnableButton(true);

            uiSelectorHandler.turnOnEditorAll(true, false);
        }


        public bool isLocationPress()
        {
            if (Input.GetKeyDown(locatoin_button) == true)
                return true;
            return false;
        }

        public bool isShowAssetPress()
        {
            if (Input.GetKeyDown(showAsset_button) == true)
                return true;
            return false;
        }

        public bool isRotatePress()
        {
            if (Input.GetKeyDown(rotate_button) == true)
                return true;
            return false;
        }
        public bool isExitPress()
        {
            if (Input.GetKeyDown(exit_button) == true)
                return true;
            return false;
        }
        public bool isExplorerPress()
        {
            if (Input.GetKeyDown(explore_button) == true)
                return true;
            return false;
        }

        public void callbackFromFPS(Vector3 pos, Vector3 rot)
        {
            AuthorableAsset currAsset = assetList[currAssetIdxSelect];
            string currAssetFileName = assetFullNameList[currAssetIdxSelect];

            (currAsset.variableContainer.getParameters("Location") as LocationXmlTemplate).setParameter(new Location(pos,rot));

            //부모만 바꿔주면 됨
            currLocatedAsset[currAssetIdxSelect].transform.parent = environment.transform;

            currAsset.testSerialize(currAssetFileName);

            showAssetWindow(true);

            subWindow.displayStatus("Save location of " + currAsset.ObjectName);
        }

        public void callbackFromDirhandlerAllAsset(string[] _assetFullNameList, AuthorableAsset[] _assetList)
        {
            assetFullNameList = _assetFullNameList;
            assetList = _assetList;

            currLocatedAsset = new GameObject[assetList.Length];

            //이 부분에서 모든 asset을 맵에 배치해야 한다.
            for(int i=0; i<assetList.Length; i++)
            {
                AuthorableAsset aa = assetList[i];
                Location l = (aa.variableContainer.getParameters("Location") as LocationXmlTemplate).getVariable();
                currLocatedAsset[i] = GameObject.Instantiate(Resources.Load(prefabPath + aa.ObjectName), l.position, Quaternion.Euler(l.rotation), environment.transform) as GameObject;
            }
        }


        public void callbackFromDirHandler(int idx)
        {
            Debug.Log("Callback From dirHandler...");

            currAssetIdxSelect = idx;

            AuthorableAsset currAsset = assetList[currAssetIdxSelect];

            Transform assetParent = fpsChar.transform.Find("AssetTarget").transform;

            //clear all child of fps character
            for (int i = 0; i < assetParent.childCount; i++)
            {
                GameObject.Destroy(assetParent.GetChild(i).gameObject);
            }
            currLocatedAsset[currAssetIdxSelect].transform.parent = fpsChar.transform.Find("AssetTarget").transform;
            //locate 시작... rayCast 해서하기

            lasr = fpsChar.GetComponent<LocateAssetWithRay>();

            lasr.startLocate(currLocatedAsset[currAssetIdxSelect].name,callbackFromFPS);
        }

        void Update()
        {
            if (isExplorerPress() == true)
            {
                showAssetWindow(false);


                Location l = (assetList[currAssetIdxSelect].variableContainer.getParameters("Location") as LocationXmlTemplate).getVariable();
                currLocatedAsset[currAssetIdxSelect].transform.parent = environment.transform;

                currLocatedAsset[currAssetIdxSelect].transform.position = l.position;
                currLocatedAsset[currAssetIdxSelect].transform.rotation = Quaternion.Euler(l.rotation);


                
                if(lasr != null)
                    lasr.callbackCancel();
            }

            if (isRotatePress() == true)
            {
                if (isShownAssetWindow == true)
                {
                    currAssetIdxPrepare = currAssetIdxPrepare + 1;
                    
                    if (currAssetIdxPrepare >= assetNumber)
                        currAssetIdxPrepare = 0;
                    dirHandler.onPrepareAssetIcon(currAssetIdxPrepare);
                }
                else
                {
                    lasr.callbackRotate();
                    //asset의 rotation 하기
                }
            }

            if (isLocationPress() == true)
            {
                if (isShownAssetWindow == true)
                {
                    //asset 선택하기
                    dirHandler.onClickAssetIcon(currAssetIdxPrepare);
                    currAssetIdxSelect = currAssetIdxPrepare;

                    showAssetWindow(false);
                    
                    Debug.Log("select..." + assetNumber.ToString());
                    

                }
                else
                {
                    lasr.callbackLocate();
                    //location 놓기
                }
            }

            if (isShowAssetPress() == true)
            {
                if (isShownAssetWindow == true)
                {
                    showAssetWindow(false);
                }
                else
                {
                    dirHandler.onPrepareAssetIcon(currAssetIdxPrepare);
                    showAssetWindow(true);
                }
            }

            if (isExitPress() == true)
            {
                OnOutofSituMode();
            }


        }

        void showAssetWindow(bool isShow)
        {
            if (isShow == true)
            {
                fpsChar.GetComponent<FirstPersonController>().enabled = false;
                assetWindow.SetActive(true);
                isShownAssetWindow = true;
                
                if(lasr != null)
                    lasr.stopLocate();
            }
            else
            {
                fpsChar.GetComponent<FirstPersonController>().enabled = true;
                assetWindow.SetActive(false);
                isShownAssetWindow = false;
            }

        }



    }
}