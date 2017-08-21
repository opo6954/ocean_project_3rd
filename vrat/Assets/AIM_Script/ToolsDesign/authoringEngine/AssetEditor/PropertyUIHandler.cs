using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    public class PropertyUIHandler : MonoBehaviour
    {
        //선택된 asset 이름 설정하는 곳
        [SerializeField]
        UnityEngine.UI.Text currAssetName;

        //선택된 asset의 이미지 설정하는 곳
        [SerializeField]
        UnityEngine.UI.RawImage currAssetImg;

        //asset의 property가 진열되는 위치
        [SerializeField]
        GameObject propertyTemplatePosition;

        [SerializeField]
        AssetEditor assetEditor;

        //SaveAs의 파일명을 위한 게임 오브젝트
        [SerializeField]
        SubwindowManager subWindowForSaveAs;


        AuthorableAsset currAssetInfo;
        Texture2D currPreviewImg;

        string currFileName = "";

        private readonly string propertyUITemplate = "AssetEditor/propertyUITemplate";
        

        //assetSavePath = Application.dataPath + "/Resources/AssetFiles/";


        // Use this for initialization
        void Start()
        {
            subWindowForSaveAs.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        //deserialize된 asset 정보를 넘겨 받는 함수임
        public void setAuthorableAsset(string fileName, AuthorableAsset _currAssetInfo, Texture2D previewImg)
        {
            currAssetInfo = _currAssetInfo;
            currPreviewImg = previewImg;
            currFileName = fileName;


            

            Debug.Log("visualization of asset proeprty for " + currAssetInfo.ObjectName);

            //preview 이미지 설정
            currAssetImg.texture = previewImg;
            
            //object의 이름 설정
            currAssetName.text = currAssetInfo.ObjectName;




            visualizeAssetProperty();

            //set한 후로 property를 visualization하기

        }
        //editor를 통해 변경된 내용을 AuthorableAsset에 저장 후에 만일 save를 하면 넘겨준다
        public AuthorableAsset getAuthorableAsset()
        {
            return currAssetInfo;
        }




        /*
         * asset의 container를 돌면서 읽으면서 각 template에 맞는 UI를 생성하여 보여준다
         * */

        //propertyType: PrimitiveXml인지 Location인지, ListofXmlTemplate인지
        //name: 파라미터 이름
        //contents: 파라미터 값
        //offset: 기타 각 type별로 추가적으로 정의되는 요소
        //idx: property의 순서, 순서대로 쭉 가야 함


        public void clearAssetProperty()
        {
            for (int i = 0; i < propertyTemplatePosition.transform.childCount; i++)
                GameObject.Destroy(propertyTemplatePosition.transform.GetChild(i).gameObject);
        }
        
        public void visualizeAssetProperty()
        {
            clearAssetProperty();
            for(int i=0; i<currAssetInfo.variableContainer.getNumberOfParameters(); i++)
            {
                XmlTemplate xt = currAssetInfo.variableContainer.getParameters(i);

                GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(propertyUITemplate), new Vector3(), new Quaternion(), propertyTemplatePosition.transform);
                go.name = xt.Name;

                if (xt.ClassName == "PrimitiveXmlTemplate")
                {
                    var q = xt as PrimitiveXmlTemplate;
                    go.GetComponent<PropertyVisualizeHandler>().visualizeProperty(q,i);
                }
                else if(xt.ClassName == "LocationXmlTemplate")
                {
                    var q = xt as LocationXmlTemplate;
                    go.GetComponent<PropertyVisualizeHandler>().visualizeProperty(q,i);
                }
                else if (xt.ClassName == "ListOfXmlTemplate")
                {
                    var q = xt as ListOfXmlTemplate;
                    go.GetComponent<PropertyVisualizeHandler>().visualizeProperty(q,i);
                }
            }
        }

        void saveAsset(string _fileName)
        {
            currAssetInfo.testSerialize(_fileName);
            subWindowForSaveAs.displaySaveStatus(_fileName);

            assetEditor.updateDirList();

        }


        public void OnClickMakeAssetGroup()
        {
        }

        public void OnClickDelete()
        {
            System.IO.File.Delete(currFileName);

            subWindowForSaveAs.displayStatus("Asset is deleted...");

            assetEditor.updateDirList();
            clearAssetProperty();
            
        }

        public void OnClickSaveAsOK()
        {
            string fileName = subWindowForSaveAs.transform.GetChild(1).GetComponent<UnityEngine.UI.InputField>().text;

            if (System.IO.File.Exists(DirectoryUIHandler.assetSavePath + fileName + ".asset") == true)
            {
                 subWindowForSaveAs.displayStatus("Same name of asset alread exists...");
            }
            else
            {
                saveAsset(DirectoryUIHandler.assetSavePath + fileName + ".asset");
            }
        }
        public void OnClickSaveAsCancel()
        {
            subWindowForSaveAs.gameObject.SetActive(false);
        }

        public void OnClickSaveAs()
        {
            subWindowForSaveAs.gameObject.SetActive(true);
        }

       

        public void OnClickSave()
        {
            saveAsset(currFileName);
        }
    }
}
