using System.ComponentModel;

public class FileItem
{
    private string originalFileName;
    private string renamedFileName;

    public string OriginalFileName
    {
        get => originalFileName;
        set
        {
            if (originalFileName != value)
            {
                originalFileName = value;
            }
        }
    }

    public string RenamedFileName
    {
        get => renamedFileName;
        set
        {
            if (renamedFileName != value)
            {
                renamedFileName = value;
            }
        }
    }

    public FileItem(string originalFileName, string renamedFileName)
    {
        OriginalFileName = originalFileName;
        RenamedFileName = renamedFileName;
    }
}