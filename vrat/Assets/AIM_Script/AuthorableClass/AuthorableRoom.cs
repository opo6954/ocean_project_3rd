using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    /*
     * Room file의 정의
     * room의 prefab 이름
     * teleportable 위치의 기록(location 형태)
     * 
     * */
    public class AuthorableRoom : ObjectTemplate
    {
        public override void initialize()
        {
            base.initialize();
            ObjectType = OBJTYPE.ROOM;

        }




        //room file 예시 serialize 함수임
        public void exampleSerialize()
        {
            ObjectName = "Cruise";

            variableContainer.addParameter(new PrimitiveXmlTemplate("PrefabName", "string"));
            ListOfXmlTemplate teleportable = new ListOfXmlTemplate("TeleportableList", "ListOfXmlTemplate", 0);

            variableContainer.addParameter(teleportable);

            (variableContainer.getParameters("PrefabName") as PrimitiveXmlTemplate).setparameter("EngineRoom");

            LocationXmlTemplate lxt1 = new LocationXmlTemplate("Teleportable1", "location", new Location(new Vector3(100, 100, 100), new Vector3(50, 50, 50)));
            LocationXmlTemplate lxt2 = new LocationXmlTemplate("Teleportable2", "location", new Location(new Vector3(1, 1, 1), new Vector3(150, 150, 150)));
            LocationXmlTemplate lxt3 = new LocationXmlTemplate("Teleportable3", "location", new Location(new Vector3(33, 33, 33), new Vector3(250, 250, 250)));

            ListOfXmlTemplate l = variableContainer.getParameters("TeleportableList") as ListOfXmlTemplate;

            l.addList(lxt1);
            l.addList(lxt2);
            l.addList(lxt3);
        }
        
    }
}