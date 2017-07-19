using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    public class AuthorableEvent : AuthorableTemplate
    {
        public AuthorableEvent()
        {
            type = "Event";

            parameterName.Add("MyEventName");
            parameterName.Add("EndConditionTemplate");
            parameterName.Add("NextEvent");
            parameterName.Add("AttachedAsset");
        }
    }
}