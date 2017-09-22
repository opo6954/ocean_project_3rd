using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    //file explorer에서 icon 역할을 할 File structure
    public enum CURREDITORTYPE
    {
        ENVIRONMENT_EDITOR, ASSET_EDITOR, TIMELINE_EDITOR
    };
    public class fileStructure
    {
        public string fileName;
        public string extension;
        public string fileImgName;
        public string fileImgExtension;
        public string parantPath;

        public string fullFileNamePath;


        public bool isTextureSet;


        Texture2D myImgTexture;


        public fileStructure(string _fileName, string _extension, string _parentPath)
        {
            fileName = _fileName;
            extension = _extension;

            fileImgName = "";
            fileImgExtension = "";

            parantPath = _parentPath;

            fullFileNamePath = _parentPath + fileName + "." + extension;

            myImgTexture = new Texture2D(2, 2);

            isTextureSet = false;
        }

        public static bool getFileNameNExtension(string _fullFileName, ref string _extension, ref string _fileName)
        {
            string[] tmpArr = _fullFileName.Split('/');
            string[] tmpNameArr;

            if (tmpArr.Length == 0)
                tmpNameArr = _fullFileName.Split('.');
            else
                tmpNameArr = tmpArr[tmpArr.Length - 1].Split('.');

            if (tmpNameArr.Length == 0)
                return false;

            _extension = tmpNameArr[tmpNameArr.Length - 1];

            for (int j = 0; j < tmpNameArr.Length - 1; j++)
            {
                string add = "";

                if (j > 0)
                    add = ".";
                _fileName = _fileName + add + tmpNameArr[j];
            }
            return true;
        }

        public Texture2D getTexture()
        {
            return myImgTexture;
        }

        public bool setTexture()
        {
            string totImgPath = parantPath + fileImgName + "." + fileImgExtension;

            byte[] data;

            if (System.IO.File.Exists(totImgPath) == false)
            {
                data = System.IO.File.ReadAllBytes(parantPath + "nullImg.png");
            }
            else
            {
                data = System.IO.File.ReadAllBytes(totImgPath);
            }

            myImgTexture.LoadImage(data);

            isTextureSet = true;

            return true;
        }
        public void addImgInfo(string _fileImgName, string _fileImgExtension)
        {
            fileImgName = _fileImgName;
            fileImgExtension = _fileImgExtension;
        }
    }
     
    /* 
     * file explorer window의 template임
     * 주로 assetListWindow, Environment Editor에서의 roomListWindow에서 쓰임
     * */
    public class FileExplorerTemplate : WindowTemplate
    {
         
        //asset structure가 위치하는 곳(scroll view에서의 contents를 이 곳으로 설정하면 됨
        [SerializeField]
        GameObject fileTemplatePosition;
        
        protected readonly string fileUITemplatePrefabPath = "AssetListWindow/fileUITemplate";

        protected Object fileUITemplatePrefab;

        protected static string fileSavePath;
         
        protected List<fileStructure> currFileList = new List<fileStructure>();

        protected List<FileUITemplateManager> currFileUIList = new List<FileUITemplateManager>();

        protected string fileType;

        int clicked = 0;
        float clickdelay = 0.5f;
        float clicktime = 0.0f;
        int prevIdx = -1;

        protected bool isDoubleClick(int _idx)
        {
            clicked++;
            
            if (clicked == 1)
            {
                clicktime = Time.time;
                prevIdx = _idx;
            }


            if (clicked > 1 && Time.time - clicktime < clickdelay && prevIdx == _idx)
            {
                clicked = 0;
                clicktime = 0;
                prevIdx = -1;
                return true;
            }
            else if (clicked > 2 || Time.time - clicktime > 1)
            {
                clicked = 0;
                prevIdx = -1;
            }

            return false;
        }

        public virtual bool UpdateFileLists()
        {
            Debug.Log("Update Directory View...");

            if (System.IO.Directory.Exists(fileSavePath) == false)
            {
                Debug.Log("No path of " + fileSavePath + ", please check out the asset file path");

                return false;
            }

            //일단 계속 update할 떄마다 새롭게 currAssetList를 클리어하자
            currFileList.Clear();

            //scroll view의 contents에 있는 지금까지 생성되어 있던 file ui structure 모두 지우기
            for (int i = 0; i < fileTemplatePosition.transform.childCount; i++)
            {
                GameObject.Destroy(fileTemplatePosition.transform.GetChild(i).gameObject);
            }

            //assetSavePath에 있는 모든 파일 list를 가져오기
            string[] p = System.IO.Directory.GetFiles(fileSavePath);
            
            //.asset file만 추출
            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                fileStructure.getFileNameNExtension(p[i], ref extension, ref fileName);

                //.asset이 아닐 경우 제외함
                if (extension != fileType)
                    continue;

                currFileList.Add(new fileStructure(fileName, extension, fileSavePath));
                
            }

            //.asset file 중에서 image가 있는 얘들 추출하기(jpg, png만 넣었음)
            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                fileStructure.getFileNameNExtension(p[i], ref extension, ref fileName);

                if (extension == "jpg" || extension == "png")
                {
                    for (int j = 0; j < currFileList.Count; j++)
                    {
                        if (currFileList[j].fileName == fileName)
                        {
                            currFileList[j].addImgInfo(fileName, extension);
                        }
                    }
                }
            }

            //추출된 .asset의 정보를 바탕으로 prefab 소환하고 scrollview에 그림과 이름 넣기

            for (int i = 0; i < currFileList.Count; i++)
            {
                string assetName = currFileList[i].fileName;

                GameObject go = (GameObject)Instantiate(fileUITemplatePrefab, new Vector3(), new Quaternion(), fileTemplatePosition.transform);

                go.name = assetName;

                currFileList[i].setTexture();

                FileUITemplateManager ftm = go.GetComponent<FileUITemplateManager>();


                if (ftm.setName(currFileList[i].fileName) == false)
                    Debug.Log("No text component found...");

                if (currFileList[i].isTextureSet == true)
                {
                    ftm.setImg(currFileList[i].getTexture());
                }

                ftm.setPosition(i);
                ftm.setOnClickListener(OnDoubleClickFile);
                ftm.setOnDragListener(OnDragStart);

                currFileUIList.Add(ftm);
            }




            return true;
        }

        public virtual void OnDoubleClickFile(int idx)
        {
            //double click일 경우에만 수행
            //prefab 불러와보자
            if (isDoubleClick(idx) == true)
            {
                Debug.Log("We catch double click event...");
            }
        }

        public virtual void OnDragStart(int idx)
        {
            Debug.Log("We catch drag start~!");
        }

        public virtual void initialize()
        {
            //fileUITemplate prefab을 미리 읽어오기
            fileUITemplatePrefab = Resources.Load(fileUITemplatePrefabPath);
            UpdateFileLists();
        }


        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}