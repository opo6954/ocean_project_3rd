using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * role information in authoring level
 * 
 * 
 * 
 * */

namespace vrat
{

    public class RoleInfoTemplate : MonoBehaviour
    {

        //Role Name
        static Dictionary<string, PlayerTemplate> roleNPlayerMappingTable = new Dictionary<string,PlayerTemplate>();

        static List<string> roleNameList = new List<string>();


        public static void setRoleList(string[] _roleNameList)
        {
            for (int i = 0; i < _roleNameList.Length; i++)
            {
                if(roleNameList.Contains(_roleNameList[i]) == false)
                    roleNameList.Add(_roleNameList[i]);
            }
        }

        public static bool isRoleNameExist(string _roleName)
        {
            return roleNameList.Contains(_roleName);
        }

        public static bool addPlayerWithRoleInfo(string roleName, PlayerTemplate pt)
        {
            //만일 role이랑 player가 없을 경우 mapping table에 넣기
            if (roleNPlayerMappingTable.ContainsKey(roleName) == false && roleNPlayerMappingTable.ContainsValue(pt) == false)
            {
                roleNPlayerMappingTable[roleName] = pt;
                return true;
            }
            return false;
        }

        public static PlayerTemplate getPlayerWithRoleInfo(string roleName)
        {
            if (roleNPlayerMappingTable.ContainsKey(roleName) == true)
            {
                return roleNPlayerMappingTable[roleName];
            }
            else
            {
                return null;
            }
        }
    }
}
