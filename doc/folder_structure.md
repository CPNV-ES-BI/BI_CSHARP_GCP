# Folder Structure

Here is an explanation of the purpose of folders and files in the directory structure:

- `gitignore`: This file lists files and directories that should be ignored by git version control
- `dockerignore`: This file lists files and directories that should be ignored when building a Docker image for the application.
- `readm.md`: This file contains basic information about the project
- `Dockerfil`: This file contains the instructions for building a Docker image for the application.
- `startdocke.cmd` : This file is a command file that is used to start the docker container
- `doc` : This folder contains the documentation of the project
  - `uml` : This folder contains the UML diagrams of the project

- `GCPMicroservice.sln`: This is the solution file for the project that contains all the project
  - `GCPMicroservice`: This directory contains the source code for the main application.
    - `GCPMicroservice.csproj`: This file is the project file that contains information about the project, including its dependencies and build settings.
      - `env-example`: This file contains an example of the environment variable that should be set for the application to run.
      - `DotEnv.cs`: This file contains the code that loads the environment variable in the application.
      - `appsettings.Development.json` and `appsettings.json` : These files contains the configurations for the application
      - `Program.cs`: This file contains the main entry point for the application.
      - `Properties`: This folder contains the launchSettings.json, which contains the configuration for how the application runs in the development environment
      - `DataObject`: This folder contains interfaces and classes that define the data structure and representation of the application.

  - `TestGCPMicroservice`: This directory contains test code for the application.
    - `GCPMicroservice.csproj`: This file is the test project file that contains information about the test project, including its dependencies and build settings.
