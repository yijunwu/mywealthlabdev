namespace WealthLab.Extensions.Agent
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public static class InterProcessCopyData
    {
        private const int int_0 = 0x4a;

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int FindWindow(string strclassName, string strWindowName);
        public static int GetHandle(string formName)
        {
            return FindWindow(null, formName);
        }

        public static string ReceiveData(Message message_0)
        {
            if (message_0.Msg == 0x4a)
            {
                COPYDATASTRUCT copydatastruct = new COPYDATASTRUCT();
                copydatastruct = (COPYDATASTRUCT) Marshal.PtrToStructure(message_0.LParam, typeof(COPYDATASTRUCT));
                if (copydatastruct.cbData > 0)
                {
                    byte[] destination = new byte[copydatastruct.cbData];
                    Marshal.Copy(copydatastruct.lpData, destination, 0, copydatastruct.cbData);
                    string str = new string(Encoding.Default.GetChars(destination));
                    str = str.Trim(new char[1]);
                    message_0.Result = (IntPtr) 1;
                    return str;
                }
            }
            return null;
        }

        public static void SendData(IntPtr handle, string string_0)
        {
            COPYDATASTRUCT copydatastruct;
            copydatastruct.dwData = IntPtr.Zero;
            string_0 = string_0 + '\0';
            copydatastruct.cbData = string_0.Length + 1;
            copydatastruct.lpData = Marshal.AllocCoTaskMem(string_0.Length);
            copydatastruct.lpData = Marshal.StringToCoTaskMemAnsi(string_0);
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(copydatastruct));
            Marshal.StructureToPtr(copydatastruct, ptr, true);
            SendMessage(handle, 0x4a, IntPtr.Zero, ptr);
            Marshal.FreeCoTaskMem(copydatastruct.lpData);
            Marshal.FreeCoTaskMem(ptr);
        }

        public static void SendData(string formName, string string_0)
        {
            int handle = GetHandle(formName);
            if (handle != 0)
            {
                SendData((IntPtr) handle, string_0);
            }
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hwnd, int int_1, IntPtr wparam, IntPtr lparam);

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }
    }
}

