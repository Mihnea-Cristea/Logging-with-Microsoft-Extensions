# Logging-with-Microsoft-Extensions
The same Pet Shop Web API, but this time integrated logging services using the Microsoft LogMonitor tool. The difference is that with this tool we can see the background application logs through Docker

Open up a Powershell in the folder directory, then run the database container, as well as the new Dockerfile Image deploy. 

docker run -d --name petshop-db psdockernetfx/petshop-db

docker build -t petshop-api:m3 ./petshop-api

docker run -d -p 8080:80 --name api petshop-api:m3

The Dockerfile.v2 packages the configuration for the LogMonitor tool

docker build -t petshop-api:m3-v2 -f ./petshop-api/Dockerfile.v2 ./petshop-api

We are building a new version for the API, then we are going to replace the container named "api" which included the previous version with the new one 

docker rm -f api

docker run -d -p 8080:80 --name api petshop-api:m3-v2

Now we can see the logs appear in the console when accessed through the Docker CLI

docker logs -f api
