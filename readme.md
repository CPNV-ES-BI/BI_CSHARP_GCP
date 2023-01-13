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

## Run

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

In order to build the Docker image you have to do the following commands
```sh
docker build -t gcpmicroservice .
docker run -it --rm -p 3000:80 --name gcpmicroservicecontainer gcpmicroservice
```

If you already have an existing docker, you'll need stop the container and remove the existing image before generating the new one

```sh
docker stop gcpmicroservicecontainer
docker rmi gcpmicroservice
```

## Tests

[Dotnet Test](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test)

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

## Contribute

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right"><a href="#readme-top">back to top</a></p>

## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right"><a href="#readme-top">back to top</a></p>

## Contact

- [MARECHAL Armand](https://github.com/Penfu)
- [COSTA-DOS-SANTOS Mauro-Alexandre](https://github.com/MauroWasTaken)

<p align="right"><a href="#readme-top">back to top</a></p>
