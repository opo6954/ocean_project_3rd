using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;


namespace vrat
{
    /*
     * 가장 윗 단에서 asset editor를 관리함
     * 
     * 
     * 
     * */
    public class AssetEditor : EditorTemplate
    {
        [SerializeField]
        private PropertyUIHandler propertyHandler;

        [SerializeField]
        private DirectoryUIHandler directoryHandler;


        GameObject rootUIObj;

        


        

        void Start()
        {
            rootUIObj = this.gameObject;
            directoryHandler.assetEditor = this;

        }

        public void updateDirList()
        {
            directoryHandler.updateDirView();
        }

        //Dir로부터 pass된 asset임 이제 assetview로 보내자
        public void callbackFromDirHandler(string fileName, AuthorableAsset aa, Texture2D previewImg)
        {
            Debug.Log("Passed asset's name: " + aa.ObjectName);

            propertyHandler.setAuthorableAsset(fileName,aa,previewImg);
            
        }
    }
}