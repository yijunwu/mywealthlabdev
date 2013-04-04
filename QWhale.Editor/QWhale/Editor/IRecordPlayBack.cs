namespace QWhale.Editor
{
    using System;
    using System.IO;

    public interface IRecordPlayBack
    {
        void LoadMacros(Stream stream);
        void LoadMacros(TextReader reader);
        void LoadMacros(string fileName);
        void PauseMacroRecording();
        void PlayBackMacro();
        void RecordKeyData(IMacroKeyData keyData);
        void ResumeMacroRecording();
        void SaveMacros(Stream stream);
        void SaveMacros(TextWriter writer);
        void SaveMacros(string fileName);
        void StartMacroRecording();
        void StopMacroRecording();
        void ToggleMacroRecording();

        bool MacroRecording { get; set; }

        IMacroKeyList MacroRecords { get; }

        bool MacroSuspendend { get; set; }
    }
}

