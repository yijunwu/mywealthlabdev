namespace WealthLab.Extensions.Agent
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class RemotingUpdateAction : MarshalByRefObject
    {
        public void CloseApplication()
        {
            Class38.smethod_0().Unregister();
            Application.Exit();
        }

        public string DeleteFile(string fileName)
        {
            string message = null;
            try
            {
                File.Delete(fileName);
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public string MoveFile(string sourceFileName, string destFileName)
        {
            string message = null;
            try
            {
                File.Copy(sourceFileName, destFileName, true);
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public string RunBatchFile(string fileName)
        {
            return null;
        }
    }
}

