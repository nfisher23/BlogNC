FROM microsoft/dotnet:sdk AS build-env

LABEL blognc.version="1.0.0"
LABEL maintainer="nickf@nickolasfisher.com"

COPY . ./src
WORKDIR /src/BlogNC
RUN dotnet restore

RUN dotnet publish -o out

FROM microsoft/dotnet:aspnetcore-runtime
COPY --from=build-env /src/BlogNC/out .
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 80
ENTRYPOINT [ "dotnet", "./BlogNC.dll" ]
