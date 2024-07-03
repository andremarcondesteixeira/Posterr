# How to create migrations

1. Make sure you have the `dotnet-ef` tool installed globally: `dotnet tool install --global dotnet-ef`
2. Make sure `dotnet-ef` is updated: `dotnet tool update --global dotnet-ef`
3. Enter the Persistence project: `cd ./Infrastructure/Persistence`
4. Run the following command: `dotnet ef migrations add <migration name goes here> -s ../../Presentation/Web/RestApi/EntryPoint/Posterr.Presentation.Web.RestApi.EntryPoint.csproj`
5. Done!

# How to generate test coverage reports

1. Run `dotnet test --collect:"XPlat Code Coverage"` from the root folder of the backend project to generate test coverage data
2. Take note of the test coverage data files. They are shown in the ending "Attachments" section of the previous command's output. Each test coverage data file is named `coverage.cobertura.xml` and will be present in a `TestResults` folder of each test project.
3. Make sure you have `dotnet-reportgenerator-globaltool` installed: `dotnet tool install -g dotnet-reportgenerator-globaltool`
4. run the following command, including all the generated `coverage.cobertura.xml` files in the `reports` parameter: (NOTE: run this in a single line in the terminal. In my experience, it is the only way to make it work without returning an error output caused by the command format in the terminal)
   ```
   reportgenerator
       -reports:"Path\To\TestProject\TestResults\{guid}\coverage.cobertura.xml", "Path2" ..., "PathN"
       -targetdir:"coveragereport"
       -reporttypes:Html
   ```
5. Open the generated HTML file in a browser to see the report
