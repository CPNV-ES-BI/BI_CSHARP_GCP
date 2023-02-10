FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
# copy sln and csproj files into the image
COPY GCPMicroservice.sln .
COPY GCPMicroservice/GCPMicroservice.csproj ./GCPMicroservice/GCPMicroservice.csproj
COPY TestGCPMicroservice/TestGCPMicroservice.csproj ./TestGCPMicroservice/TestGCPMicroservice.csproj
# restore the nuget packages
RUN dotnet restore
# copy full solution over
COPY . .
# build the solution
RUN dotnet build

# create a new layer using the cut-down aspnet runtime image
FROM build AS dev
# set the working directory to be the web api project
WORKDIR /src/GCPMicroservice
# publish the web api project to a directory called out
RUN dotnet publish -c Release -o out
# run the web api when the docker image is started
ENTRYPOINT ["dotnet", "run"]

# run the unit tests
FROM build AS test
# set the directory to be within the unit test project
WORKDIR /src/TestGCPMicroservice/
# run the unit tests
RUN dotnet test --logger:trx

# create a new layer from the build later
FROM test AS publish
# set the working directory to be the web api project
WORKDIR /src/GCPMicroservice
#remove test project
RUN rm -r ../TestGCPMicroservice
# publish the web api project to a directory called out
RUN dotnet publish -c Release -o out

ENTRYPOINT ["dotnet", "run"]

# create a new build target called testrunner
FROM build AS testrunner
# navigate to the unit test directory
WORKDIR /src/TestGCPMicroservice/
# when you run this build target it will run the unit tests
CMD ["dotnet", "test", "--logger:trx"]