using CSharpFunctionalExtensions;
using SachkovTech.Core.ValueObjects.Ids;

namespace SachkovTech.Domain.IssueSolvingManagement.ValueObjects;

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