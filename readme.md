https://localhost:7071/swagger/index.html

http://localhost:5232/swagger/index.html

dotnet ef migrations add InitDb
dotnet ef database update
```export schema=chatdb
dotnet ef migrations script -o Common/Schenma/migrate.sql

