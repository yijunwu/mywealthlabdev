using System;
using System.Windows.Forms;
internal delegate RadioButton Delegate10();

internal static Delegate10 delegate10_0;

static Delegate10()
{
    u003cModuleu003e.smethod_0(Delegate10.delegate10_0);
}

public extern Delegate10(object object_0, IntPtr intptr_0);

public override extern RadioButton Invoke();

internal static RadioButton smethod_0()
{
    return Delegate10.delegate10_0();
}
