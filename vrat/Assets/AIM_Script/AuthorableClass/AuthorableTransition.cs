using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    public class AuthorableTransition : ObjectTemplate
    {
        public override void initialize()
        {
            base.initialize();
            ObjectType = OBJTYPE.TRANSITION;

            initForTransition();

        }

        void initForTransition()
        {
            
        }


    }
}