ASP.NET 
======================================

You will need to enable Nuget Package Restore in Visual Studio in order to download and restore Nuget packages during build. If you are not sure how to do this, see [Keep Nuget Packages Out of Source Control with Nuget Package Manager Restore][2]

You will also need to run Entity Framework Migrations `Update-Database` command per the article. The migration files are included in the repo, so you will NOT need to `Enable-Migrations` or run `Add-Migration Init`. 

Administrator:
Username:leo
Password:12345abcd
