using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    class minMax
    {
        public float min;
        public float max;

        public minMax()
        {
            min = 0;
            max = 0;
        }

    }
    public class SetSliderType : SetTypeTemplate
    {
        [SerializeField]
        UnityEngine.UI.Text sliderText;

        [SerializeField]
        UnityEngine.UI.Slider slider;

        void Start()
        {
            vp = VISUALIZEPROPTYPE.SLIDER;
        }
        /*
        public override void setValue<T>(T t)
        {
            var q = t as minMax;

            setMin(q.min);
            setMax(q.max);
        }

        public override float getValue<T>()
        {
            return slider.value;
        }
        */

        void setMin(float f)
        {
            slider.minValue = f;
        }

        void setMax(float f)
        {
            slider.maxValue = f;
        }
    }
}