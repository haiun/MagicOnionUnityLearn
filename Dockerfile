#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /MagicOnionServerLearn
COPY ["MagicOnionServerLearn/Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Shared/Shared.csproj"
WORKDIR /MagicOnionUnityLearn
COPY ["MagicOnionUnityLearn/Assets/Scripts/Shared/.", "Assets/Scripts/Shared/."]

#WORKDIR "/MagicOnionServerLearn/Shared"
#RUN dotnet build "Shared.csproj" -c Release -o /app/build

WORKDIR /MagicOnionServerLearn
COPY ["MagicOnionServerLearn/GrpcService1/GrpcService1.csproj", "GrpcService1/"]
RUN dotnet restore "GrpcService1/GrpcService1.csproj"
COPY ["MagicOnionServerLearn/GrpcService1/.", "GrpcService1/."]

WORKDIR /MagicOnionServerLearn/GrpcService1
RUN dotnet build "GrpcService1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcService1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcService1.dll"]