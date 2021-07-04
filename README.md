## About

Full CRUD Application using RepositoryPattern and UnitOfWork Pattern in ASP .NET Core 5.0 for back-end and Aurelia with localization for front-end.

## Requirements
 - Make sure you have latest docker (on linux) / docker desktop (on windows) version installed.
 
## Gettings Started  without Docker

```bash
# Clone this project
$ git clone https://github.com/nilbersilva/hahn.applicaton

# Access folder
$ cd hahn.applicaton

#Restore packages for .net core project
$ dotnet restore Hahn.ApplicatonProcess.February2021.Domain
$ dotnet restore Hahn.ApplicatonProcess.February2021.Data
$ dotnet restore Hahn.ApplicatonProcess.February2021.Web

#Restore packages for aurelia app
$ cd Hahn.ApplicatonProcess.February2021.App
$ yarn or npm install

#Start backend
$ cd ..
$ dotnet run Hahn.ApplicatonProcess.February2021.Web
# Back-end application will run on https://localhost:5001

#Start app - open another terminal
Go to project folder hahn.applicaton
$ cd Hahn.ApplicatonProcess.February2021.App
$ yarn start or npm run-script start

# The Application will run in http://localhost:8080
```

## Gettings Started with Docker

```bash
# Clone this project
$ git clone https://github.com/nilbersilva/hahn.applicaton

# Access folder
$ cd hahn.applicaton

#Docker Build
$ docker-compose build

#Docker Run
$ docker-compose up

# The Application  will initialize in http://localhost:8080
```

## Technologies

The following tools were used in this project:

- [Visual Studio](https://visualstudio.microsoft.com/)
- [Docker](https://www.docker.com/)
- [.NET Core](https://dotnet.microsoft.com/download)
- [Typescript](https://www.typescriptlang.org/)
- [WebPack](https://webpack.js.org/)
- [Aurelia](https://aurelia.io/)
 
