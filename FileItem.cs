namespace AIFileRename
{
    public class FileItem(string originalFileName, string aiRenamedFileName, string regularRenamedFileName)
    {
        private string _originalFileName = originalFileName;
        private string _aiRenamedFileName = aiRenamedFileName;
        private string _regularRenamedFileName = regularRenamedFileName;

        public string OriginalFileName
        {
            get => _originalFileName;
            set
            {
                if (_originalFileName != value)
                {
                    _originalFileName = value;
                }
            }
        }

        public string AIRenamedFileName
        {
            get => _aiRenamedFileName;
            set
            {
                if (_aiRenamedFileName != value)
                {
                    _aiRenamedFileName = value;
                }
            }
        }

        public string RegularRenamedFileName
        {
            get => _regularRenamedFileName;
            set
            {
                if (_regularRenamedFileName != value)
                {
                    _regularRenamedFileName = value;
                }
            }
        }
    }
}