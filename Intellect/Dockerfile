#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Intellect/Intellect.csproj", "Intellect/"]
COPY ["TicTacToeLib/TicTacToeLib.csproj", "TicTacToeLib/"]
RUN dotnet restore "Intellect/Intellect.csproj"
COPY . .
WORKDIR "/src/Intellect"
RUN dotnet build "Intellect.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Intellect.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#RUN apt-get update && apt-get install procps -y && apt-get install valgrind -y
ENTRYPOINT ["dotnet", "Intellect.dll"]
#ENTRYPOINT ["valgrind", "--leak-check=full", "--show-leak-kinds=all", "--track-origins=yes", "dotnet", "Intellect.dll"]