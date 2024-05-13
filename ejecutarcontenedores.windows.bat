:: Detener y eliminar los contenedores si ya existen
docker stop postgresql-contenedor1
docker rm postgresql-contenedor1
docker stop fin-tech-bank-contenedor1
docker rm fin-tech-bank-contenedor1

:: Iniciar el contenedor de PostgreSQL
docker run --name postgresql-contenedor1 -e POSTGRES_USER=postgresql -e POSTGRES_PASSWORD=postgresql -p 5432:5432 -d postgres

:: Construir la imagen de Docker para la aplicación FinTechBank
::docker image build -t fin-tech-bank-image:latest .
docker pull atorres2610/retos:latest

:: Iniciar el contenedor para la aplicación FinTechBank
docker run --name fin-tech-bank-contenedor1 -e ASPNETCORE_ENVIRONMENT=DevDocker -p 5080:8080 atorres2610/retos
