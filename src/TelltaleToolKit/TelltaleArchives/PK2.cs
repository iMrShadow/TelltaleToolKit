namespace TelltaleToolKit.TelltaleArchives;

// TODO: PK2 for CSI.
public class PK2Archive : ArchiveBase
{
    protected override void ReadMetadata()
    {
        throw new NotImplementedException();
    }

    public override MemoryStream ExtractFile(string fileName)
    {
        throw new NotImplementedException();
    }

    public override void ExtractAll(string destinationPath)
    {
        throw new NotImplementedException();
    }
}