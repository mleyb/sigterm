# setup build environment
FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# copy everything and build the project
COPY sigterm ./
RUN dotnet restore sigterm.csproj
RUN dotnet publish sigterm.csproj -c Release -o out

# build runtime image
FROM microsoft/dotnet:runtime
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "sigterm.dll"]