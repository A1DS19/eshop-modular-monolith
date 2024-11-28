### Bootstrapper
- `docker-compose up`
- `dotnet watch --project ./src/Bootstrapper/API/ --launch-profile https`

### Migrations
- `dotnet ef migrations add migration-name --startup-project ./src/Bootstrapper/API/ --project 'project-path' -o ./Data/Migrations --context 'context-name'`

### Database
- `dotnet ef database update`

### Add package
- `dotnet add 'project-path' package 'package-name' --version 'version'`

#### In order for Keycloak to work, you need to create a schema in your local Postgres instance called `identity`