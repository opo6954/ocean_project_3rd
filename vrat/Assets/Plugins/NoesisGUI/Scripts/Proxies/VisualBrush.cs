/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.4
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class VisualBrush : TileBrush {
  internal new static VisualBrush CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new VisualBrush(cPtr, cMemoryOwn);
  }

  internal VisualBrush(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(VisualBrush obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public VisualBrush() {
  }

  protected override System.IntPtr CreateCPtr(System.Type type, out bool registerExtend) {
    registerExtend = false;
    return NoesisGUI_PINVOKE.new_VisualBrush();
  }

  public static DependencyProperty VisualProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.VisualBrush_VisualProperty_get();
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public Visual Visual {
    set {
      NoesisGUI_PINVOKE.VisualBrush_Visual_set(swigCPtr, Visual.getCPtr(value));
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.VisualBrush_Visual_get(swigCPtr);
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
      return (Visual)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.VisualBrush_GetStaticType();
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return ret;
  }

}

}

