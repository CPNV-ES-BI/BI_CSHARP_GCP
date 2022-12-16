# BI CSHARP GCP

This ASP.NET microservice whose purpose is to implement [Google Cloud Plateform](https://console.cloud.google.com/getting-started?hl=fr&pli=1) as a data source to perform some techniques related to `Business Intelligence`.

<a name="readme-top"></a>
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#docker">Docker</a></li>
    <li><a href="#contribute">Contribute</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

## Run

### Docker

```shell
docker stop gcpmicroservicecontainer
docker rmi gcpmicroservice
docker build -t gcpmicroservice .
docker run -it --rm -p 3000:80 --name gcpmicroservicecontainer gcpmicroservice
```

## Contribute

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Contact

- [MARECHAL Armand](https://github.com/Penfu)
- [COSTA-DOS-SANTOS Mauro-Alexandre](https://github.com/MauroWasTaken)

<p align="right">(<a href="#readme-top">back to top</a>)</p>
