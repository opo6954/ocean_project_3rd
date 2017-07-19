using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class AuthorableAsset : AuthorableTemplate
    {
        

        public AuthorableAsset()
        {
            type = "Asset";
            //Asset에 필요한 파라미터 이름 넣기
            parameterName.Add("MyAssetName");
            parameterName.Add("RoleInfo");
            parameterName.Add("TriggerList");
            parameterName.Add("AfterEffectList");
            parameterName.Add("BeforeEffectList");
            parameterName.Add("Location");
            parameterName.Add("IsInventory");
            parameterName.Add("BoundingBox");
        }
    }
}
