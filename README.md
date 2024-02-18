# Sample Application
### *Jonathan Tiefer*

## ***Projects Contained:***
- **Developer.API**
  - An ASP.Net Core 6 back-end service that will be used in conjunction with the Developer-UI React front-end service to handle authentication and to load information into the dashboard for each user.  The ASP.Net Core 6 demonstrates authentication, authorization and the utliziation of ASP.Net Identity provider and OAuth type security.  The Developer.API and Developer-UI are containerized and a docker compose script is included to launch the web application on your local machine.

- **Developer-UI**
  - A React 16 front-end application that will be used to log in and out users, as well as register new users into the system.  Users will log into a dashboard that will be loaded from the back-end ASP.Net Core 6 service.  The React application utilizes Mobx for state management, Axios for http requests, Semantic UI and Formix for user interface effects and styling.

- **Developer.Service**
  - A set of services and classes that can be used in the Developer.API application and demonstrate a wide variety of different software engineering disciplines and coding practices.
  - The following classes are contained in the library:
    - Algorithms
    - ClassRefactoring
    - Containers
    - Syncing

- **Developer.Service.Tests**
  - A set of unit tests for each of the services in the Developer.Service.

<br>
<br>
<br>

## **Instructions to Initalize Web Application and Run Docker Compose**
The following configuration files are required with the required configuration properties are listed below.  Each will be loaded in as an environment variable or configuration setting to the appropriate application:
<br>

***Developer.API and Postgres Database Configuration Files***
<br>
<br>

  **sampleappapi.env**
  ```
  ConnectionStrings__SampleAppDbConnection="Server=postgres;Port=5432;Database=sampleappdb;User Id=<user-id>;Password=<password>;"
  
  DefaultAdminUserPassword=<password>

  JwtKey=<jwt-token-signing-key>
  ```
<br>

  **sampleappdb.env**
  ```
  POSTGRES_USER=<user>
  POSTGRES_PASSWORD=<password>
  ```
<br>
<br>

***Developer-UI Configuration Files***
<br>
<br>

  **.env.development**
  ```
  VITE_API_URL=<api-url>
  ```
<br>

  **.env.production**
  ```
  VITE_API_URL=<api-url>
  ```
<br>

***Docker Compose File***
<br>

To create and run the docker containers of the web application, take the following steps:
- Be in the root folder of the solution.
- Run the command `docker compose up -d` to launch all containers contained in the docker-compose.yml file in a detached state.

To shut down the web application and to stop and delete the container, take the following steps:
- Be in the root folder of the solution.
- Run the command `docker compose down` to stop and delete all running containers launched in the docker-compose.yml file.
  - If you wish to delete the volume associated with the postgres container that was generated, instead of running docker compose down, run the same command with -v argument: `docker compose down -v`.
- Optionally, you can delete the images of each container generated in the web application.  This can be done by the following commands:

  ```
  docker rmi sampleappapi
  docker rmi sampleappui
  docker rmi postgres
  ```
