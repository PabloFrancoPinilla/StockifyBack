dotnet ef migrations add LoginTenant -p ./Data/Stockify.Data.csproj -s ./API/Stockify.API.csproj
dotnet ef database update -p ./Data/Stockify.Data.csproj -s ./API/Stockify.API.csproj