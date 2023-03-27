<span name="readme-top"></span>

# BI CSHARP GCP

This ASP.NET microservice whose purpose is to implement [Google Cloud Plateform](https://console.cloud.google.com/getting-started?hl=fr&pli=1) as a data source to perform some techniques related to `Business Intelligence`.

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#docker">Docker</a></li>
    <li><a href="#tests">Tests</a></li>
    <li><a href="#contribute">Contribute</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

## Requirements

| Requirement     | Version  | Link                                                                                                                                                               |
|-----------------|----------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| .NET            | 7        | [Link](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.101-windows-x64-installer)                                                             |
| Docker          | 20.10.21 | [Link](https://docs.docker.com/engine/install/)                                                                                                                    |

## Project structure

See the [folder structure](doc/folder_structure.md) documentation.

## Run

### Environment variables

Generate your token from Google Cloud Plateform

```sh
gcloud auth application-default print-access-token
```

Create a `.env` file from `.env-example` in both Microservice and Test projects.

### Locally


If you want to Run the program without using a Docker container with the following command

```sh
dotnet run
```

You can alternatively just compile the code without running it

```sh
dotnet build
```

By doing these commands the nuget packages should be automatically installed, but it is possible to do it manually with the following

```sh
dotnet restore
```

### Docker

In order to build the Docker image you have to do the following commands, keep in mind that if there are any failing tests the build will fail
```sh
docker build --target publish -t gcpmicroservice .
docker run -it --rm -p 3000:80 --name gcpmicroservicecontainer gcpmicroservice
```

If you already have an existing docker, you'll need stop the container and remove the existing image before generating the new one

```sh
docker stop gcpmicroservicecontainer
docker rmi gcpmicroservice
```
#### Docker Targets

In this project we have 3 different docker targets

- `publish` : This target is used to build the test andpublish the project. This build will fail if there are any failing tests
- `dev` : This target is the same as publish but it skips testing
- `testrunner` : Allows you to run all the tests inside of a docker container

## Tests

Official documentation [Dotnet Test](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test)

To run all the tests, run the following command

```sh
dotnet test
```

To run all tests of a specific class

```sh
dotnet test --filter ClassName=<ProjectName>.<ClassName>
```

To run a specific test

```sh
dotnet test --filter FullyQualifiedName=<ProjectName>.<ClassName>.<MethodName>
```

To run all tests inside of a docker container

```sh
docker build --target testrunner -t gcpmicroservice .
```

## Swagger

This microservice uses [Swagger](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio) to document and test the API endpoints. To access the Swagger documentation, navigate to `/swagger/index.html` in your browser after running the microservice. This will display a list of all available endpoints, their parameters, and a "Try it out" button to test the endpoint with your own parameters.

## CI CD

This project has a fully automated pipeline using Github Actions.
if you fork this project, these are the following secrets you'll need to add to your repository
- GCP_KEYFILE : This is the keyfile.json you can get from Google Cloud Plateform converted to a base64 string
- GCP_BUCKET: This is the name of the bucket you want to use
- DOCKER_USERNAME: This is your docker username
- DOCKER_PASSWORD: This is your docker password

without these secrets the pipeline will fail

### Release

To find the latest release, go to the [release page](https://hub.docker.com/repository/docker/maurowastaken/gcpmicroservice/general)

## Contribute

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right"><a href="#readme-top">back to top</a></p>

## License

Distributed under the MIT License. See [LICENSE](https://github.com/CPNV-ES-BI/BI_CSHARP_GCP/blob/develop/LICENSE) for more information.

<p align="right"><a href="#readme-top">back to top</a></p>

## Contact

- [MARECHAL Armand](https://github.com/Penfu)
- [COSTA-DOS-SANTOS Mauro-Alexandre](https://github.com/MauroWasTaken)

<p align="right"><a href="#readme-top">back to top</a></p>
