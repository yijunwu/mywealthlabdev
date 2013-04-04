namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.IO;
    using System.Text;

    public class FileAppender : TextWriterAppender
    {
        private bool m_appendToFile;
        private System.Text.Encoding m_encoding;
        private string m_fileName;
        private LockingModelBase m_lockingModel;
        private log4net.Core.SecurityContext m_securityContext;
        private LockingStream m_stream;

        public FileAppender()
        {
            this.m_appendToFile = true;
            this.m_fileName = null;
            this.m_encoding = System.Text.Encoding.Default;
            this.m_stream = null;
            this.m_lockingModel = new ExclusiveLock();
        }

        [Obsolete("Instead use the default constructor and set the Layout & File properties")]
        public FileAppender(ILayout layout, string filename) : this(layout, filename, true)
        {
        }

        [Obsolete("Instead use the default constructor and set the Layout, File & AppendToFile properties")]
        public FileAppender(ILayout layout, string filename, bool append)
        {
            this.m_appendToFile = true;
            this.m_fileName = null;
            this.m_encoding = System.Text.Encoding.Default;
            this.m_stream = null;
            this.m_lockingModel = new ExclusiveLock();
            this.Layout = layout;
            this.File = filename;
            this.AppendToFile = append;
            this.ActivateOptions();
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.m_securityContext == null)
            {
                this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }
            if (this.m_lockingModel == null)
            {
                this.m_lockingModel = new ExclusiveLock();
            }
            this.m_lockingModel.CurrentAppender = this;
            using (this.SecurityContext.Impersonate(this))
            {
                this.m_fileName = ConvertToFullPath(this.m_fileName.Trim());
            }
            if (this.m_fileName != null)
            {
                this.SafeOpenFile(this.m_fileName, this.m_appendToFile);
            }
            else
            {
                LogLog.Warn("FileAppender: File option not set for appender [" + base.Name + "].");
                LogLog.Warn("FileAppender: Are you using FileAppender instead of ConsoleAppender?");
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.m_stream.AcquireLock())
            {
                try
                {
                    base.Append(loggingEvent);
                }
                finally
                {
                    this.m_stream.ReleaseLock();
                }
            }
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            if (this.m_stream.AcquireLock())
            {
                try
                {
                    base.Append(loggingEvents);
                }
                finally
                {
                    this.m_stream.ReleaseLock();
                }
            }
        }

        protected void CloseFile()
        {
            this.WriteFooterAndCloseWriter();
        }

        protected override void CloseWriter()
        {
            if (this.m_stream != null)
            {
                this.m_stream.AcquireLock();
                try
                {
                    base.CloseWriter();
                }
                finally
                {
                    this.m_stream.ReleaseLock();
                }
            }
        }

        protected static string ConvertToFullPath(string path)
        {
            return SystemInfo.ConvertToFullPath(path);
        }

        protected virtual void OpenFile(string fileName, bool append)
        {
            if (LogLog.IsErrorEnabled)
            {
                bool flag = false;
                using (this.SecurityContext.Impersonate(this))
                {
                    flag = Path.IsPathRooted(fileName);
                }
                if (!flag)
                {
                    LogLog.Error("FileAppender: INTERNAL ERROR. OpenFile(" + fileName + "): File name is not fully qualified.");
                }
            }
            lock (this)
            {
                this.Reset();
                LogLog.Debug(string.Concat(new object[] { "FileAppender: Opening file for writing [", fileName, "] append [", append, "]" }));
                this.m_fileName = fileName;
                this.m_appendToFile = append;
                this.LockingModel.CurrentAppender = this;
                this.LockingModel.OpenFile(fileName, append, this.m_encoding);
                this.m_stream = new LockingStream(this.LockingModel);
                if (this.m_stream != null)
                {
                    this.m_stream.AcquireLock();
                    try
                    {
                        this.SetQWForFiles(new StreamWriter(this.m_stream, this.m_encoding));
                    }
                    finally
                    {
                        this.m_stream.ReleaseLock();
                    }
                }
                this.WriteHeader();
            }
        }

        protected override void PrepareWriter()
        {
            this.SafeOpenFile(this.m_fileName, this.m_appendToFile);
        }

        protected override void Reset()
        {
            base.Reset();
            this.m_fileName = null;
        }

        protected virtual void SafeOpenFile(string fileName, bool append)
        {
            try
            {
                this.OpenFile(fileName, append);
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error(string.Concat(new object[] { "OpenFile(", fileName, ",", append, ") call failed." }), exception, ErrorCode.FileOpenFailure);
            }
        }

        protected virtual void SetQWForFiles(Stream fileStream)
        {
            this.SetQWForFiles(new StreamWriter(fileStream, this.m_encoding));
        }

        protected virtual void SetQWForFiles(TextWriter writer)
        {
            base.QuietWriter = new QuietTextWriter(writer, this.ErrorHandler);
        }

        protected override void WriteFooter()
        {
            if (this.m_stream != null)
            {
                this.m_stream.AcquireLock();
                try
                {
                    base.WriteFooter();
                }
                finally
                {
                    this.m_stream.ReleaseLock();
                }
            }
        }

        protected override void WriteHeader()
        {
            if ((this.m_stream != null) && this.m_stream.AcquireLock())
            {
                try
                {
                    base.WriteHeader();
                }
                finally
                {
                    this.m_stream.ReleaseLock();
                }
            }
        }

        public bool AppendToFile
        {
            get
            {
                return this.m_appendToFile;
            }
            set
            {
                this.m_appendToFile = value;
            }
        }

        public System.Text.Encoding Encoding
        {
            get
            {
                return this.m_encoding;
            }
            set
            {
                this.m_encoding = value;
            }
        }

        public virtual string File
        {
            get
            {
                return this.m_fileName;
            }
            set
            {
                this.m_fileName = value;
            }
        }

        public LockingModelBase LockingModel
        {
            get
            {
                return this.m_lockingModel;
            }
            set
            {
                this.m_lockingModel = value;
            }
        }

        public log4net.Core.SecurityContext SecurityContext
        {
            get
            {
                return this.m_securityContext;
            }
            set
            {
                this.m_securityContext = value;
            }
        }

        public class ExclusiveLock : FileAppender.LockingModelBase
        {
            private Stream m_stream = null;

            public override Stream AcquireLock()
            {
                return this.m_stream;
            }

            public override void CloseFile()
            {
                using (base.CurrentAppender.SecurityContext.Impersonate(this))
                {
                    this.m_stream.Close();
                }
            }

            public override void OpenFile(string filename, bool append, Encoding encoding)
            {
                try
                {
                    using (base.CurrentAppender.SecurityContext.Impersonate(this))
                    {
                        string directoryName = Path.GetDirectoryName(filename);
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        FileMode mode = append ? FileMode.Append : FileMode.Create;
                        this.m_stream = new FileStream(filename, mode, FileAccess.Write, FileShare.Read);
                    }
                }
                catch (Exception exception)
                {
                    base.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + filename + ". " + exception.Message);
                }
            }

            public override void ReleaseLock()
            {
            }
        }

        public abstract class LockingModelBase
        {
            private FileAppender m_appender = null;

            protected LockingModelBase()
            {
            }

            public abstract Stream AcquireLock();
            public abstract void CloseFile();
            public abstract void OpenFile(string filename, bool append, Encoding encoding);
            public abstract void ReleaseLock();

            public FileAppender CurrentAppender
            {
                get
                {
                    return this.m_appender;
                }
                set
                {
                    this.m_appender = value;
                }
            }
        }

        private sealed class LockingStream : Stream, IDisposable
        {
            private FileAppender.LockingModelBase m_lockingModel = null;
            private int m_lockLevel = 0;
            private int m_readTotal = -1;
            private Stream m_realStream = null;

            public LockingStream(FileAppender.LockingModelBase locking)
            {
                if (locking == null)
                {
                    throw new ArgumentException("Locking model may not be null", "locking");
                }
                this.m_lockingModel = locking;
            }

            public bool AcquireLock()
            {
                bool flag = false;
                lock (this)
                {
                    if (this.m_lockLevel == 0)
                    {
                        this.m_realStream = this.m_lockingModel.AcquireLock();
                    }
                    if (this.m_realStream != null)
                    {
                        this.m_lockLevel++;
                        flag = true;
                    }
                }
                return flag;
            }

            private void AssertLocked()
            {
                if (this.m_realStream == null)
                {
                    throw new LockStateException("The file is not currently locked");
                }
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                this.AssertLocked();
                IAsyncResult asyncResult = this.m_realStream.BeginRead(buffer, offset, count, callback, state);
                this.m_readTotal = this.EndRead(asyncResult);
                return asyncResult;
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                this.AssertLocked();
                IAsyncResult asyncResult = this.m_realStream.BeginWrite(buffer, offset, count, callback, state);
                this.EndWrite(asyncResult);
                return asyncResult;
            }

            public override void Close()
            {
                this.m_lockingModel.CloseFile();
            }

            public override int EndRead(IAsyncResult asyncResult)
            {
                this.AssertLocked();
                return this.m_readTotal;
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
            }

            public override void Flush()
            {
                this.AssertLocked();
                this.m_realStream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return this.m_realStream.Read(buffer, offset, count);
            }

            public override int ReadByte()
            {
                return this.m_realStream.ReadByte();
            }

            public void ReleaseLock()
            {
                lock (this)
                {
                    this.m_lockLevel--;
                    if (this.m_lockLevel == 0)
                    {
                        this.m_lockingModel.ReleaseLock();
                        this.m_realStream = null;
                    }
                }
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                this.AssertLocked();
                return this.m_realStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                this.AssertLocked();
                this.m_realStream.SetLength(value);
            }

            void IDisposable.Dispose()
            {
                this.Close();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                this.AssertLocked();
                this.m_realStream.Write(buffer, offset, count);
            }

            public override void WriteByte(byte value)
            {
                this.AssertLocked();
                this.m_realStream.WriteByte(value);
            }

            public override bool CanRead
            {
                get
                {
                    return false;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    this.AssertLocked();
                    return this.m_realStream.CanSeek;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    this.AssertLocked();
                    return this.m_realStream.CanWrite;
                }
            }

            public override long Length
            {
                get
                {
                    this.AssertLocked();
                    return this.m_realStream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    this.AssertLocked();
                    return this.m_realStream.Position;
                }
                set
                {
                    this.AssertLocked();
                    this.m_realStream.Position = value;
                }
            }

            public sealed class LockStateException : LogException
            {
                public LockStateException(string message) : base(message)
                {
                }
            }
        }

        public class MinimalLock : FileAppender.LockingModelBase
        {
            private bool m_append;
            private string m_filename;
            private Stream m_stream = null;

            public override Stream AcquireLock()
            {
                if (this.m_stream == null)
                {
                    try
                    {
                        using (base.CurrentAppender.SecurityContext.Impersonate(this))
                        {
                            string directoryName = Path.GetDirectoryName(this.m_filename);
                            if (!Directory.Exists(directoryName))
                            {
                                Directory.CreateDirectory(directoryName);
                            }
                            FileMode mode = this.m_append ? FileMode.Append : FileMode.Create;
                            this.m_stream = new FileStream(this.m_filename, mode, FileAccess.Write, FileShare.Read);
                            this.m_append = true;
                        }
                    }
                    catch (Exception exception)
                    {
                        base.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + this.m_filename + ". " + exception.Message);
                    }
                }
                return this.m_stream;
            }

            public override void CloseFile()
            {
            }

            public override void OpenFile(string filename, bool append, Encoding encoding)
            {
                this.m_filename = filename;
                this.m_append = append;
            }

            public override void ReleaseLock()
            {
                using (base.CurrentAppender.SecurityContext.Impersonate(this))
                {
                    this.m_stream.Close();
                    this.m_stream = null;
                }
            }
        }
    }
}

