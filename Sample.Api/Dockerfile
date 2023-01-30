FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Sample.Api/*.csproj Sample.Api/
COPY Sample.Api/*.csproj Sample.Api/ 
RUN dotnet restore  Sample.Api/*.csproj
COPY . .
RUN dotnet publish Sample.Api/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","Sample.Api.dll" ]