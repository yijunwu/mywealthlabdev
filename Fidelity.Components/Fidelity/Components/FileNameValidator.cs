namespace Fidelity.Components
{
    using System;
    using System.IO;

    public static class FileNameValidator
    {
        public static void ValidateFileName(string fileName)
        {
            try
            {
                new FileInfo(fileName);
            }
            catch (Exception)
            {
            }
        }
    }
}

