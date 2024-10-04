using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Domain.ValueObjects;

public class IssueIdentifier : ValueObject
{
    public IssueIdentifier(ModuleId moduleId, Guid issueId)
    {
        ModuleId = moduleId;
        IssueId = issueId;
    }
    
    public ModuleId ModuleId { get; }
    
    public Guid IssueId { get; }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return ModuleId;
        yield return IssueId;
    }
}