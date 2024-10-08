dotnet ef migrations add 1_Initial_Create --context MakDbContext --project MakApi

dotnet ef migrations add 1_Initial_Create --context AuthDbContext --project MakApi

dotnet ef database update --context MakDbContext --project MakApi

dotnet ef database update --context AuthDbContext --project MakApi