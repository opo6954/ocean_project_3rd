using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    

    public class DirectoryUIHandler : MonoBehaviour
    {
        //.asset 파일 template이 위치하는 곳
        [SerializeField]
        GameObject fileTemplatePosition;
        [SerializeField]
        public AssetEditor_back assetEditor;

        //AutorableAsset 변수, 이 곳에 저작 관련 정보를 저장하고 주고 받음
        protected AuthorableAsset currAsset;

        
        

        //fileUItemplate prefab이 저장된 경로
        protected readonly string fileUITemplate = "AssetEditor/fileUITemplate";

        //current path name
        public static string assetSavePath;

        public List<fileStructure> assetNameList = new List<fileStructure>();
        public List<FileUITemplateManager> uitemplateList = new List<FileUITemplateManager>();
        

        protected readonly string assetType = "asset";


        //몇번째 asset이 클릭되었는지에 대한 정보를 idx로 넘겨줌



        public virtual void onClickAssetIcon(int idx)
        {
            //이 부분에서 관련 asset의 정보를 읽으면 됨
            currAsset = new AuthorableAsset();
            currAsset.initialize();

            string fileName = assetNameList[idx].fileName;

            //.asset의 정보 부르기
            currAsset.testDeserialize(assetSavePath + fileName + "." + assetType);

            assetEditor.callbackFromDirHandler(assetSavePath + fileName + "." + assetType, currAsset, assetNameList[idx].getTexture());
        }

        // Use this for initialization
        void Start()
        {
            assetSavePath = Application.dataPath + "/Resources/AssetFiles/";

            updateDirView();
        }


        //directoryView를 update한다.
        public void updateDirView()
        {

            Debug.Log("Update Directory View...");

            if (System.IO.Directory.Exists(assetSavePath) == false)
            {
                Debug.Log("wrong with path...");
            }

            assetNameList.Clear();

            for (int i = 0; i < fileTemplatePosition.transform.childCount; i++)
            {
                GameObject.Destroy(fileTemplatePosition.transform.GetChild(i).gameObject);
            }



            string[] p = System.IO.Directory.GetFiles(assetSavePath);

            //.asset file 불러오기
            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                getFileNameNExtension(ref fileName, ref extension, p[i]);

                

                if (extension != assetType)
                    continue;

                assetNameList.Add(new fileStructure(fileName, extension, assetSavePath));
            }


            //.asset과 이름이 같은 jpg, png 불러오기
            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                getFileNameNExtension(ref fileName, ref extension, p[i]);

                if (extension == "jpg" || extension == "png")
                {
                    for (int j = 0; j < assetNameList.Count; j++)
                    {

                        if (assetNameList[j].fileName == fileName)
                        {
                            assetNameList[j].addImgInfo(fileName, extension);
                        }
                    }

                }
            }

            //Raw image와 이름을 prefab 소환해서 배치하고 값 넣기
            //Raw image와 이름 추가하기

            for (int i = 0; i < assetNameList.Count; i++)
            {
                string assetName = assetNameList[i].fileName;

                GameObject go = (GameObject)Instantiate(Resources.Load(fileUITemplate),new Vector3(),new Quaternion(),fileTemplatePosition.transform);

                

                go.name = assetName;

                

                assetNameList[i].setTexture();

                FileUITemplateManager ftm = go.GetComponent<FileUITemplateManager>();

                if (ftm.setName(assetNameList[i].fileName) == false)
                    Debug.Log("No text component found...");

                if (assetNameList[i].isTextureSet == true)
                    ftm.setImg(assetNameList[i].getTexture());

                ftm.setPosition(i);
                ftm.setOnClickListener(onClickAssetIcon);

                uitemplateList.Add(ftm);

                
            }
        }


        bool getFileNameNExtension(ref string fileName,ref string extension, string wholefileName)
        {
            string[] tmpArr = wholefileName.Split('/');
            string[] tmpNameArr;

            if (tmpArr.Length == 0)
                tmpNameArr = wholefileName.Split('.');
            else
                tmpNameArr = tmpArr[tmpArr.Length - 1].Split('.');

            if (tmpNameArr.Length == 0)
                return false;
                

            extension = tmpNameArr[tmpNameArr.Length - 1];

            fileName = "";

            for (int j = 0; j < tmpNameArr.Length-1; j++)
            {
                string add = "";

                if (j > 0)
                    add = ".";
                fileName = fileName + add + tmpNameArr[j];
            }
            return true;
        }

    }
}