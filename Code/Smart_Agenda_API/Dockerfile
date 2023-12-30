# Uses the official .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copies the main solution file
COPY ["Smart_Agenda_API.sln", "./"]

# Copies the .csproj files
COPY ["Smart_Agenda_API/Smart_Agenda_API.csproj", "Smart_Agenda_API/"]
COPY ["Smart_Agenda_DAL/Smart_Agenda_DAL.csproj", "Smart_Agenda_DAL/"]
COPY ["Smart_Agenda_Logic/Smart_Agenda_Logic.csproj", "Smart_Agenda_Logic/"]
COPY ["Logic.UnitTest/Logic.UnitTest.csproj", "Logic.UnitTest/"]
COPY ["IntegrationTest/IntegrationTest.csproj", "IntegrationTest/"]

# Clears NuGet cache
RUN dotnet nuget locals all --clear

# Restores the packages for the entire solution
RUN dotnet restore

# Copies the rest of the solution's files
COPY . .

# Builds the solution
RUN dotnet build "Smart_Agenda_API.sln" -c Release --no-restore

# Publishes the API project
FROM build AS publish
RUN dotnet publish "Smart_Agenda_API/Smart_Agenda_API.csproj" -c Release --no-restore -o /app/publish

# Uses the official .NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smart_Agenda_API.dll"]