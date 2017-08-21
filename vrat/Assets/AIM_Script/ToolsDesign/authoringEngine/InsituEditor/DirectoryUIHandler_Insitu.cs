using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class DirectoryUIHandler_Insitu : DirectoryUIHandler
    {
        [SerializeField]
        public InsituEditor insituEditor;

        public override void onClickAssetIcon(int idx)
        {
            
            insituEditor.callbackFromDirHandler(idx);
        }

        public void loadAllPlacableAsset()
        {
            AuthorableAsset[] assetList = new AuthorableAsset[assetNameList.Count];
            string[] assetFullNameList = new string[assetNameList.Count];


            for (int i = 0; i < assetNameList.Count; i++)
            {
                assetList[i] = new AuthorableAsset();
                assetList[i].initialize();

                assetFullNameList[i] = assetSavePath + assetNameList[i].fileName + "." + assetType;

                assetList[i].testDeserialize(assetFullNameList[i]);
            }

            insituEditor.callbackFromDirhandlerAllAsset(assetFullNameList, assetList);
        }

        

        public void onPrepareAssetIcon(int idx)
        {
            Debug.Log("OnPrepareAssetIcon: " + idx.ToString());

            Debug.Log("OnPrepareAssetIcon: " + uitemplateList.Count);
            for (int i = 0; i < uitemplateList.Count; i++)
            {
                uitemplateList[i].turnColor(false);  
            }

            uitemplateList[idx].turnColor(true);
        }

    }
}