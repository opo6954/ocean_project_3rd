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
    public class AssetEditor_back : EditorTemplate
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

            //Test for asset file serialize
            AuthorableAsset aa = new AuthorableAsset();
            aa.exampleExtinguisherSerialize();
            aa.testSerialize("extinguisher.asset");

            AuthorableAsset aa2 = new AuthorableAsset();
            



            /*
            

            aa.initialize();
            aa.exampleSerialize();
            aa.testSerialize("test.asset");
            AuthorableAsset aa2 = new AuthorableAsset();
            aa2.initialize();
            aa2.testDeserialize("test.asset");

            Debug.Log((aa2.variableContainer.getParameters(0) as PrimitiveXmlTemplate).getVariable());
            Debug.Log((aa2.variableContainer.getParameters(1) as PrimitiveXmlTemplate).getVariable());
            Debug.Log((aa2.variableContainer.getParameters(2) as LocationXmlTemplate).Position);
            Debug.Log((aa2.variableContainer.getParameters(3) as ListOfXmlTemplate).getXmlTemplate(0).Type);
            Debug.Log((aa2.variableContainer.getParameters(4) as ListOfXmlTemplate).getXmlTemplate(0).Type);
            */

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