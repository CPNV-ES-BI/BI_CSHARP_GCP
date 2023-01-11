# GCP microservice C#

## Description

## Project structure

See the [folder structure](doc/folder_structure.md) documentation.

## Run

### Run with docker

```shell
docker stop gcpmicroservicecontainer
docker rmi gcpmicroservice
docker build -t gcpmicroservice .
docker run -it --rm -p 3000:80 --name gcpmicroservicecontainer gcpmicroservice
```
