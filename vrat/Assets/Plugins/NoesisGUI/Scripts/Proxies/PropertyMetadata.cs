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

public partial class PropertyMetadata : BaseComponent {
  internal new static PropertyMetadata CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new PropertyMetadata(cPtr, cMemoryOwn);
  }

  internal PropertyMetadata(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(PropertyMetadata obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public object DefaultValue {
    get {
      return GetDefaultValueHelper();
    }
  }

  public PropertyMetadata() {
  }

  protected override System.IntPtr CreateCPtr(System.Type type, out bool registerExtend) {
    registerExtend = false;
    return NoesisGUI_PINVOKE.new_PropertyMetadata__SWIG_0();
  }

  public bool HasDefaultValue {
    get {
      bool ret = NoesisGUI_PINVOKE.PropertyMetadata_HasDefaultValue_get(swigCPtr);
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
      return ret;
    } 
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.PropertyMetadata_GetStaticType();
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return ret;
  }

  private object GetDefaultValueHelper() {
    IntPtr cPtr = NoesisGUI_PINVOKE.PropertyMetadata_GetDefaultValueHelper(swigCPtr);
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return Noesis.Extend.GetProxy(cPtr, false);
  }

}

}
