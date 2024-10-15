namespace SachkovTech.SharedKernel;

public interface ISoftDeletable
{
    void Delete();
    void Restore();
    
    DateTime? DeletionDate { get; }
}