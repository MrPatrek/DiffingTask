# DiffingTask

The application is built on the top of .NET 8 (ASP.NET Core 8).

To run it, import the project and restore the NuGet packages. Then (e.g., in Visual Studio 2022), open the Package Manager Console (default project: DiffingTask) and run `Update-Database`. After that, the application is ready to be run.

The application utilizes Entity Framework Core using the Microsoft SQL Server database provider, which should be automatically set up after following the above-mentioned instructions.

Postman collection for testing is [included](https://github.com/MrPatrek/DiffingTask/blob/main/DiffingTask.postman_collection.json).
