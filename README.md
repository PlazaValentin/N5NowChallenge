# N5NowChallenge - Plaza Valentin

Muchas gracias por la oportunidad, espero les guste el trabajo realizado y podamos continuar con el proceso 😊

## Instrucciones de Ejecución

1. **Descargar el Repositorio:**
   ```bash
   git clone https://github.com/PlazaValentin/N5NowChallenge.git

2. **Ejecutar el docker-compose.yml**
   ```bash
   docker-compose up -d

3. **Ejecutar el script de la base de datos**
   ```bash
   docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Admin@123 -d master -i ./db-init.sql
Puede suceder que necesiten esperar unos minutos hasta que se levanten el sqlserver container para poder ejecutar el comando.

4. **Ya se puede hacer uso de la aplicacion ingresando a http://localhost:8088/swagger/index.html**
   
## Desafio

N5 company requests a Web API for registering user permissions, to carry out  
this task it is necessary to comply with the following steps:  
+ Create a **Permissions** table with the following fields:  
+ Create a PermissionTypes table with the following fields:  
+ Create relationship between Permission and PermissionType.  
+ Create a Web API using net core on Visual Studio and persist data on  
SQL Server.  
+ Make use of EntityFramework.  
+ The Web API must have 3 services “Request Permission”, “Modify Permission”
  and “Get Permissions”. Every service should persist a permission registry
  in an elasticsearch index, the register inserted in elasticsearch must contains
  the same structure of database table permission”.  
+ Create apache kafka in local environment and create new topic where persist
every operation a message with the next dto structure:
  * Id: random Guid
  * Name operation: “modify”, “request” or “get”. (desired)
+ Making use of repository pattern and Unit of Work and CQRS pattern(Desired).
Bear in mind that is required to stick to a proper service architecture so that
 creating different layers and dependency injection is a must-have.
+ Create Unit Testing and Integration Testing to call the three of the services.
+ Use good practices as much as possible.
+ Prepare the solution to be containerized in a docker image.
+ Upload exercise to some repository (github, gitlab,etc).
Resources:
   * Elasticsearch: 
   https://www.elastic.co/guide/en/elasticsearch/reference/current/docker.html ,
   https://www.elastic.co/guide/en/elasticsearch/client/netapi/7.x/nest.html
   * docker-compose.yml
   * Sql server express: https://hub.docker.com/_/microsoft-mssql-server
   * docker-compose.yml
   * Kafka: https://www.notion.so/n5now/Kafka242a5fd883bf49ffa77190fb16eb4ecf#74a1076feed24ea482c804f54483773d
   * docker-compose.yml
   * Serilog: https://serilog.net/
   * CQRS: https://docs.microsoft.com/enus/azure/architecture/patterns/cqrs
   * EF: https://docs.microsoft.com/en-us/ef/core
