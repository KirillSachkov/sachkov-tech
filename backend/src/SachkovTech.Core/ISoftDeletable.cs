namespace SachkovTech.Core;

public interface ISoftDeletable
{
    void Delete();
    void Restore();
}