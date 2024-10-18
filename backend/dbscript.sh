docker-compose up -d

dotnet-ef database drop -f -c IssuesWriteDbContext -p ./src/Issues/SachkovTech.Issues.Infrastructure/ -s ./src/SachkovTech.Web/

dotnet-ef migrations remove -c IssuesWriteDbContext -p ./src/Issues/SachkovTech.Issues.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations remove -c FilesWriteDbContext -p ./src/Files/SachkovTech.Files.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations remove -c AccountsWriteDbContext -p ./src/Accounts/SachkovTech.Accounts.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations remove -c IssueSolvingWriteDbContext -p ./src/IssueSolvings/SachkovTech.IssueSolving.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations remove -c IssueReviewsWriteDbContext -p ./src/IssueReviews/SachkovTech.IssueReviews.Infrastructure/ -s ./src/SachkovTech.Web/

dotnet-ef migrations add init -c IssuesWriteDbContext -p ./src/Issues/SachkovTech.Issues.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations add init -c FilesWriteDbContext -p ./src/Files/SachkovTech.Files.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations add init -c AccountsWriteDbContext -p ./src/Accounts/SachkovTech.Accounts.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations add init -c IssueSolvingWriteDbContext -p ./src/IssueSolvings/SachkovTech.IssueSolving.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef migrations add init -c IssueReviewsWriteDbContext -p ./src/IssueReviews/SachkovTech.IssueReviews.Infrastructure/ -s ./src/SachkovTech.Web/

dotnet-ef database update -c IssuesWriteDbContext -p ./src/Issues/SachkovTech.Issues.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef database update -c FilesWriteDbContext -p ./src/Files/SachkovTech.Files.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef database update -c AccountsWriteDbContext -p ./src/Accounts/SachkovTech.Accounts.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef database update -c IssueSolvingWriteDbContext -p ./src/IssueSolvings/SachkovTech.IssueSolving.Infrastructure/ -s ./src/SachkovTech.Web/
dotnet-ef database update -c IssueReviewsWriteDbContext -p ./src/IssueReviews/SachkovTech.IssueReviewsInfrastructure/ -s ./src/SachkovTech.Web/


pause