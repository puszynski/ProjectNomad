# ProjectNomad

Setup:
1. Initialize local database:
  a. install Entity Framework globally via powerShell command: _dotnet tool install --global dotnet-ef_
  b. run _database update_ for each module - check [ModuleName]ModuleConfiguration.cs
  c. check local database if migration where done: (LocalDb)\MSSQLLocalDB
3. Generate map tiles: run server api and call admin to generate map tiles: https://localhost:7277/api/admin/generatenewmapsection?x=0&y=0
4. Create account and play. Enjoy.
