FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Sample/*.csproj Sample/
COPY Sample/*.csproj Sample/ 
RUN dotnet restore  Sample/*.csproj
COPY . .
RUN dotnet publish Sample/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","Sample.dll" ]