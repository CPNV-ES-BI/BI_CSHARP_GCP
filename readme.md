# GCP microservice C#

## Description

## Run

### Run with docker

```shell
docker stop gcpmicroservicecontainer
docker rmi gcpmicroservice
docker build -t gcpmicroservice .
docker run -it --rm -p 3000:80 --name gcpmicroservicecontainer gcpmicroservice
```