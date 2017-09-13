using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * 공통된 창인 AssetListWindow
     * windowTemplate으로부터 상속받는다
     * */

    /*
     * Asset list에서의 directory의 입력 및 visualization을 관리함
     * 
     * asset list에 대해 가능한 operation
     * 
     * 1. 더블클릭
     * 공통사항: 배치된 asset의 위치로 카메라가 적절히 이동하기 배치되어 있지 않은 경우 아직 배치되지 않았습니다 창 뜨기
     * environment editor일시: 그대로...
     * Asset editor일시: asset의 property를 편집할 수 있는 창의 업데이트
     * Timeline editor일시: 그대로...
     * 
     * 2. 드래그 앤 드롭
     * environment editor일시: 동작 안함
     * asset editor일시: 동작 안함
     * timeline editor일시: timeline 창의 action, trigger, instruction으로 이동시 간략한 파라미터를 수정할 수 있는 창 열기
     * 
     * 
     * 
     * 
     * 
     * */

    
         
    public class AssetListWindowHandler : FileExplorerTemplate
    {
        //모든 editor main function을 serializeField로 가지고 있어야 함
        [SerializeField]
        AssetEditor assetEditor;

        [SerializeField]
        EnvironmentEditor environmentEditor;

        [SerializeField]
        TimelineEditor timelineEditor;

        //최근에 선택된 Asset의 정보
        protected AuthorableAsset currAsset;
        
        

        //asset을 double click했을 때 각 editor로 가는 callback 함수
        public delegate void OnDoubleClickForEditor(AuthorableAsset aa);
        public OnDoubleClickForEditor callbackForDCinEnvironmentEditor;
        public OnDoubleClickForEditor callbackForDCinAssetEditor;
        public OnDoubleClickForEditor callbackForDCinTimelineEditor;

        //asset을 drag n drop했을 때 각 editor로 가는  callback 함수

        public delegate void OnDragNDropForEditor(AuthorableAsset aa);
        public OnDragNDropForEditor callbackforDNDinEnvironmentEditor;
        public OnDragNDropForEditor callbackforDNDinAssetEditor;
        public OnDragNDropForEditor callbackforDNDinTimelineEditor;

        CURREDITORTYPE currFloatingEditorWindow;

        public void setCurrEditorWindow(string _type)
        {
            CURREDITORTYPE _cet;

            _cet = CURREDITORTYPE.ENVIRONMENT_EDITOR;

            if (_type.Contains("Environment") == true)
            {
                _cet = CURREDITORTYPE.ENVIRONMENT_EDITOR;
            }
            else if (_type.Contains("Asset") == true)
            {
                _cet = CURREDITORTYPE.ASSET_EDITOR;
            }
            else if (_type.Contains("Timeline") == true)
            {
                _cet = CURREDITORTYPE.TIMELINE_EDITOR;
            }

            currFloatingEditorWindow = _cet;
        }

        //callback 함수를 설정하는 함수
        public void setDCCallback(bool isDoubleClick, CURREDITORTYPE editorIdx, OnDoubleClickForEditor _callback)
        {
            if(isDoubleClick == true)
            {
                switch (editorIdx)
                {
                    case CURREDITORTYPE.ENVIRONMENT_EDITOR:
                        callbackForDCinEnvironmentEditor = _callback;
                        break;
                    case CURREDITORTYPE.ASSET_EDITOR:
                        callbackForDCinAssetEditor = _callback;
                        break;
                    case CURREDITORTYPE.TIMELINE_EDITOR:
                        callbackForDCinTimelineEditor = _callback;
                        break;
                }
            }
        }

        //asset을 더블클릭시의 callback 함수

        public override void OnDoubleClickFile(int idx)
        {
            //double click일 경우에만 수행
            if (isDoubleClick(idx) == true)
            {

                Debug.Log("We catch double click event...");

                //선택된 asset idx를 바탕으로 asset을 testDeserialize하기

                currAsset = new AuthorableAsset();
                currAsset.initialize();

                currAsset.testDeserialize(currFileList[idx].fullFileNamePath);

                //최근 열린 editortype에 따라서 다른 callback 함수를 부르기
                switch (currFloatingEditorWindow)
                {
                    case CURREDITORTYPE.ENVIRONMENT_EDITOR:
                        callbackForDCinEnvironmentEditor(currAsset);
                        break;

                    case CURREDITORTYPE.ASSET_EDITOR:
                        callbackForDCinAssetEditor(currAsset);
                        break;

                    case CURREDITORTYPE.TIMELINE_EDITOR:
                        callbackForDCinTimelineEditor(currAsset);
                        break;
                }
            }
        }

        void tmpForSetCallback()
        {
            
            //setDCCallback(true, CURREDITORTYPE.ASSET_EDITOR, delegate (AuthorableAsset _aa) { Debug.Log("For double click, On Asset Editor callback"); });
            setDCCallback(true, CURREDITORTYPE.ENVIRONMENT_EDITOR, delegate (AuthorableAsset _aa) { Debug.Log("For double click, On Environment Editor callback"); });
            setDCCallback(true, CURREDITORTYPE.TIMELINE_EDITOR, delegate (AuthorableAsset _aa) { Debug.Log("For double click, On Timeline Editor callback"); });
        }

        public override void initialize()
        {
            //asset List를 관리하니 file type은 asset으로 정의
            fileType = "asset";

            //asset의 저장 경로를 불러오기(datapath 기반)
            fileSavePath = Application.dataPath + "/Resources/AssetFiles/";

            //임시로 다른 environment editor, timeilne editor의 callback 함수를 설정하기
            tmpForSetCallback();

            //asset editor로 건네줄 callback 함수 설정
            setDCCallback(true, CURREDITORTYPE.ASSET_EDITOR, assetEditor.OnSelectAsset);

            base.initialize();

            
        }
    }
}