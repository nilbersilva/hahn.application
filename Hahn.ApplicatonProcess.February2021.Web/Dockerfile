#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#RUN mkdir -p /app/wwwroot
FROM node:12.20.2-alpine3.12 AS client
WORKDIR /app
COPY ["Hahn.ApplicatonProcess.February2021.App", "ClientApp/"]
WORKDIR /app/ClientApp
RUN npm cache clean -f
RUN npm install
#RUN npm install --save tslib
RUN npm run build 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Hahn.ApplicatonProcess.February2021.Web/Hahn.ApplicatonProcess.February2021.Web.csproj", "Hahn.ApplicatonProcess.February2021.Web/"]
COPY ["Hahn.ApplicatonProcess.February2021.Data/Hahn.ApplicatonProcess.February2021.Data.csproj", "Hahn.ApplicatonProcess.February2021.Data/"]
COPY ["Hahn.ApplicatonProcess.February2021.Domain/Hahn.ApplicatonProcess.February2021.Domain.csproj", "Hahn.ApplicatonProcess.February2021.Domain/"]
RUN dotnet restore "Hahn.ApplicatonProcess.February2021.Web/Hahn.ApplicatonProcess.February2021.Web.csproj"
COPY . .
WORKDIR "/src/Hahn.ApplicatonProcess.February2021.Web"
RUN dotnet build "Hahn.ApplicatonProcess.February2021.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hahn.ApplicatonProcess.February2021.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=client /app/ClientApp/dist ./wwwroot
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hahn.ApplicatonProcess.February2021.Web.dll"]