docker-compose up -d

dotnet-ef database drop -f -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\


dotnet-ef migrations remove -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations remove -c FilesWriteDbContext -p .\src\Files\SachkovTech.Files.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations remove -c AccountsWriteDbContext -p .\src\Accounts\SachkovTech.Accounts.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations remove -c IssueSolvingWriteDbContext -p .\src\IssueSolvings\SachkovTech.IssueSolving.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations remove -c IssuesReviewsWriteDbContext -p .\src\IssuesReviews\SachkovTech.IssuesReviews.Infrastructure\ -s .\src\SachkovTech.Web\


dotnet-ef migrations add Issues_init -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations add Files_init -c FilesWriteDbContext -p .\src\Files\SachkovTech.Files.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations add Files_init -c AccountsWriteDbContext -p .\src\Accounts\SachkovTech.Accounts.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations add IssueSolvings_init -c IssueSolvingWriteDbContext -p .\src\IssueSolvings\SachkovTech.IssueSolving.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations add IssuesReviews_init -c IssueReviewsWriteDbContext -p .\src\IssuesReviews\SachkovTech.IssuesReviews.Infrastructure\ -s .\src\SachkovTech.Web\


dotnet-ef database update -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef database update -c FilesWriteDbContext -p .\src\Files\SachkovTech.Files.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef database update -c AccountsWriteDbContext -p .\src\Accounts\SachkovTech.Accounts.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef database update -c IssueSolvingWriteDbContext -p .\src\IssueSolvings\SachkovTech.IssueSolving.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef database update -c IssueReviewsWriteDbContext -p .\src\IssuesReviews\SachkovTech.IssuesReviews.Infrastructure\ -s .\src\SachkovTech.Web\


pause