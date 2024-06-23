FROM mcr.microsoft.com/dotnet/sdk:8.0 as base
WORKDIR /source
COPY ./PosterrBackEnd.sln ./
COPY ./Core/Application/UseCases/Posterr.Core.Application.UseCases.csproj ./Core/Application/UseCases/
COPY ./Core/Application/UseCasesTests/Posterr.Core.Application.UseCasesTests.csproj ./Core/Application/UseCasesTests/
COPY ./Core/Domain/Entities/Posterr.Core.Domain.Entities.csproj ./Core/Domain/Entities/
COPY ./Core/Domain/EntitiesTests/Posterr.Core.Domain.EntitiesTests.csproj ./Core/Domain/EntitiesTests/
COPY ./Core/Shared/ConfigurationInterfaces/Posterr.Core.Shared.ConfigurationInterfaces.csproj ./Core/Shared/ConfigurationInterfaces/
COPY ./Core/Shared/EntitiesInterfaces/Posterr.Core.Shared.EntitiesInterfaces.csproj ./Core/Shared/EntitiesInterfaces/
COPY ./Core/Shared/Exceptions/Posterr.Core.Shared.Exceptions.csproj ./Core/Shared/Exceptions/
COPY ./Core/Shared/PersistenceInterfaces/Posterr.Core.Shared.PersistenceInterfaces.csproj ./Core/Shared/PersistenceInterfaces/
COPY ./Infrastructure/Persistence/Posterr.Infrastructure.Persistence.csproj ./Infrastructure/Persistence/
COPY ./Presentation/Web/RestApi/Controllers/Posterr.Presentation.Web.RestApi.Controllers.csproj ./Presentation/Web/RestApi/Controllers/
COPY ./Presentation/Web/RestApi/EntryPoint/Posterr.Presentation.Web.RestApi.EntryPoint.csproj ./Presentation/Web/RestApi/EntryPoint/
COPY ./Presentation/Web/RestApi/Tests/Posterr.Presentation.Web.RestApi.Tests.csproj ./Presentation/Web/RestApi/Tests/
RUN dotnet restore
COPY . .
RUN dotnet test
RUN dotnet publish -o /binaries

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=base /binaries .
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
ENTRYPOINT ["dotnet", "Posterr.Presentation.Web.RestApi.EntryPoint.dll"]
