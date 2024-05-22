# How to create migrations

1. Make sure you have the `dotnet-ef` tool installed globally: `dotnet tool install --global dotnet-ef`
2. Make sure `dotnet-ef` is updated: `dotnet tool update --global dotnet-ef`
3. Enter the Persistence project: `cd ./Infrastructure/Persistence`
4. Run the following command: `dotnet ef migrations add <migration name goes here> -s ../../Presentation/Web/RestApi/EntryPoint/Posterr.Presentation.Web.RestApi.EntryPoint.csproj `
5. Done!
