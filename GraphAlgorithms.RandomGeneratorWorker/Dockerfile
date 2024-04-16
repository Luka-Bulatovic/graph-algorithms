# Use the official .NET 6.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory in the container
WORKDIR /app

# Copy the published application to the container
COPY ./bin/Release/net6.0/publish/ /app/

# Set the command to run your app
ENTRYPOINT ["dotnet", "GraphAlgorithms.RandomGeneratorWorker.dll"]
