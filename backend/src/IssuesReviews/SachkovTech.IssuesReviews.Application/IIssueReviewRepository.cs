﻿using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Domain;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Application;

public interface IIssueReviewRepository
{
    Task<Result<IssueReview, Error>> GetById(IssueReviewId id, CancellationToken cancellationToken = default);
    
    UnitResult<Error> Add(IssueReview issueReview, CancellationToken cancellationToken = default);
}
