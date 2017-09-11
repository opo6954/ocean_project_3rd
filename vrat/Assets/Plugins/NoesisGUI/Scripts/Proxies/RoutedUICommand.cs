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

public class RoutedUICommand : RoutedCommand {
  internal new static RoutedUICommand CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new RoutedUICommand(cPtr, cMemoryOwn);
  }

  internal RoutedUICommand(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(RoutedUICommand obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  protected RoutedUICommand() {
  }

  public string Text {
    set {
      NoesisGUI_PINVOKE.RoutedUICommand_Text_set(swigCPtr, value != null ? value : string.Empty);
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
    }
    get {
      IntPtr strPtr = NoesisGUI_PINVOKE.RoutedUICommand_Text_get(swigCPtr);
      #if UNITY_EDITOR || NOESIS_API
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      #endif
      string str = Noesis.Extend.StringFromNativeUtf8(strPtr);
      return str;
    }
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.RoutedUICommand_GetStaticType();
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return ret;
  }

}

}
