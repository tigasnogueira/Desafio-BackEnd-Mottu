# Desafio backend Mottu.
Seja muito bem-vindo ao desafio backend da Mottu, obrigado pelo interesse em fazer parte do nosso time e ajudar a melhorar a vida de milhares de pessoas.

## Instruções
- O desafio é válido para diversos níveis, portanto não se preocupe se não conseguir resolver por completo.
- A aplicação só será avaliada se estiver rodando, se necessário crie um passo a passo para isso.
- Faça um clone do repositório em seu git pessoal para iniciar o desenvolvimento e não cite nada relacionado a Mottu.
- Após finalização envie um e-mail para o recrutador informando o repositório para análise.
  
## Requisitos não funcionais 
- A aplicação deverá ser construida com .Net utilizando C#.
- Utilizar apenas os seguintes bancos de dados (Postgress, MongoDB)
    - Não utilizar PL/pgSQL
- Escolha o sistema de mensageria de sua preferencia( RabbitMq, Sqs/Sns , Kafka, Gooogle Pub/Sub ou qualquer outro)

## Aplicação a ser desenvolvida
Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. Quando um entregador estiver registrado e com uma locação ativa poderá também efetuar entregas de pedidos disponíveis na plataforma.
### Casos de uso
- Eu como usuário admin quero cadastrar uma nova moto.
  - Os dados obrigatórios da moto são Identificador, Ano, Modelo e Placa
  - A placa é um dado único e não pode se repetir.
  - Quando a moto for cadastrada a aplicação deverá gerar um evento de moto cadastrada
    - A notificação deverá ser publicada por mensageria.
    - Criar um consumidor para notificar quando o ano da moto for "2024"
    - Assim que a mensagem for recebida, deverá ser armazenada no banco de dados para consulta futura.
- Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
- Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
    - Os dados do entregador são( identificador, nome, cnpj, data de nascimento, número da CNHh, tipo da CNH, imagemCNH)
    - Os tipos de cnh válidos são A, B ou ambas A+B.
    - O cnpj é único e não pode se repetir.
    - O número da CNH é único e não pode se repetir.
- Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
    - O formato do arquivo deve ser png ou bmp.
    - A foto não poderá ser armazenada no banco de dados, você pode utilizar um serviço de storage( disco local, amazon s3, minIO ou outros).
- Eu como entregador quero alugar uma moto por um período.
    - Os planos disponíveis para locação são:
        - 7 dias com um custo de R$30,00 por dia
        - 15 dias com um custo de R$28,00 por dia
        - 30 dias com um custo de R$22,00 por dia
        - 45 dias com um custo de R$20,00 por dia
        - 50 dias com um custo de R$18,00 por dia
    - A locação obrigatóriamente tem que ter uma data de inicio e uma data de término e outra data de previsão de término.
    - O inicio da locação obrigatóriamente é o primeiro dia após a data de criação.
    - Somente entregadores habilitados na categoria A podem efetuar uma locação
- Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
    - Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
        - Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
        - Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
    - Quando a data informada for superior a data prevista do término, será cobrado um valor adicional de R$50,00 por diária adicional.
    

## Diferenciais 🚀
- Testes unitários
- Testes de integração
- EntityFramework e/ou Dapper
- Docker e Docker Compose
- Design Patterns
- Documentação
- Tratamento de erros
- Arquitetura e modelagem de dados
- Código escrito em língua inglesa
- Código limpo e organizado
- Logs bem estruturados
- Seguir convenções utilizadas pela comunidade
  

## Como Executar a API

Antes de iniciar a aplicação, é necessário configurar o Redis e gerar as migrações do banco de dados seguindo os passos abaixo:

### 1. Instalar e Executar o Redis no Windows

#### Utilizando o Docker
Uma das maneiras mais fáceis de executar o Redis no Windows é utilizando o Docker. Se você não tem o Docker instalado, pode baixá-lo e instalá-lo a partir do site oficial do Docker.

Depois de instalar o Docker, execute o seguinte comando para baixar e iniciar o Redis:

```bash
docker run -d --name redis -p 6379:6379 redis
```

Este comando faz o seguinte:

- Baixa a imagem oficial do Redis do Docker Hub.
- Cria e inicia um contêiner chamado redis que mapeia a porta 6379 do contêiner para a porta 6379 do host.

#### Utilizando o Redis no Windows Subsystem for Linux (WSL)
Se você estiver utilizando o WSL, pode instalar o Redis diretamente no WSL. Primeiro, abra um terminal do WSL e execute os seguintes comandos:

```bash
sudo apt update
sudo apt install redis-server
```

Configure e Inicie o Redis:

Abra o arquivo de configuração do Redis:
```bash
sudo nano /etc/redis/redis.conf
```

Encontre a linha `supervised` no arquivo e altere para `supervised systemd`.

Salve e feche o arquivo (`Ctrl+O`, `Enter`, `Ctrl+X`).

Inicie o serviço do Redis:

```bash
sudo service redis-server start
```

Verifique se o Redis está rodando:

Execute:
```bash
redis-cli ping
```

A resposta esperada é:
```bash
PONG
```

### 2. Configurar as migrações do banco de dados

1. Navegue até a pasta `Desafio-BackEnd-Mottu\src\BikeRentalSystem.Infrastructure` e execute o terminal, Command Prompt ou PowerShell, ou clique com o botão direito no projeto `BikeRentalSystem.Infrastructure` no Visual Studio e selecione a opção "Open in Terminal".

2. Execute os seguintes comandos para gerar novas migrações para as tabelas do banco de dados, caso tenham sido feitas mudanças nas entidades ou mapeamentos:

   ```sh
   dotnet ef migrations add NomeDaMigracaoDataContext --context DataContext --output-dir Context/Migrations
   dotnet ef migrations add NomeDaMigracaoApplicationDbContext --context ApplicationDbContext --output-dir Identity/Migrations
   ```

3. Execute os comandos abaixo para criar/atualizar o banco de dados com as tabelas das entidades e do Identity:

   ```sh
   dotnet ef database update --context DataContext
   dotnet ef database update --context ApplicationDbContext
   ```

4. Após aplicar as migrações, verifique no Visual Studio que o projeto `BikeRentalSystem.Api` esteja selecionado como projeto de inicialização, com a opção https selecionada, e execute a API.

## Testando a Aplicação

Há duas opções para testar a aplicação:

1. **Postman**:
   - Importe o arquivo `BikeRentalSystem.postman_collection.json` incluso no repositório do projeto, trazendo todos os endpoints configurados e com documentação.

2. **Swagger**:
   - Acesse o Swagger da aplicação, que será aberto em uma página do navegador logo ao inicializar a API.

Siga essas instruções para garantir que sua aplicação está configurada corretamente e pronta para ser avaliada.


# API Documentation

## AuthController - Register

**Method:** `POST`

**URL:** `{{baseUrl}}/api/v1.0/auth/register`

**Description:**

### Register User

This endpoint allows users to register with the application.

#### Request Body

- `email` (string, required): The email address of the user.
    
- `password` (string, required): The password for the user account.
    
- `confirmPassword` (string, required): The confirmation of the password.
    

#### Response

The response of this request is a JSON schema representing the structure of the response data.

**Headers:**

- Content-Type: application/json

**Body:**

```json
{
  "email": "user@example.com",
  "password": "password123",
  "confirmPassword": "password123"
}
```

---

## AuthController - Login

**Method:** `POST`

**URL:** `{{baseUrl}}/api/v1.0/auth/login`

**Description:**


This endpoint allows users to authenticate and login to the system. The HTTP POST request should be made to {{baseUrl}}/api/v1.0/auth/login with the following payload in the raw request body type:

```json
{
  "email": "",
  "password": ""
}
```

### Response
The response of this request is a JSON schema representing the authentication token and user details upon successful login. The schema for the response will include the structure and data types of the returned JSON object.


**Headers:**

- Content-Type: application/json

**Body:**

```json
{
  "email": "admin@example.com",
  "password": "P@ssw0rd!"
}
```

---

## AuthController - Add Role

**Method:** `POST`

**URL:** `{{baseUrl}}/api/v1.0/auth/add-role`

**Description:**


### Add Role
This endpoint is used to add a new role.

#### Request Body
- `roleName` (string, required): The name of the role to be added.

#### Response
The response of this request is a JSON schema.


**Headers:**

- Content-Type: application/json
- Authorization: Bearer {{accessToken}}

**Body:**

```json
{
  "roleName": "Admin"
}
```

---

## AuthController - Assign Roles and Claims

**Method:** `POST`

**URL:** `{{baseUrl}}/api/v1.0/auth/assign-roles-claims`

**Description:**


The `POST` request to `/api/v1.0/auth/assign-roles-claims` endpoint is used to assign roles and claims to a user.

### Request Body
- `userId` (string, required): The ID of the user to whom the roles and claims are being assigned.
- `roles` (array of strings, required): An array of roles to be assigned to the user.
- `claims` (array of objects, required): An array of claims containing type and value for the user.

### Response
The response of this request is a JSON schema representing the structure of the response object. This schema defines the properties and their data types that can be expected in the response.

Example JSON Schema:
```json
{
  "type": "object",
  "properties": {
    "status": {
      "type": "string"
    },
    "message": {
      "type": "string"
    }
  }
}
```


**Headers:**

- Content-Type: application/json
- Authorization: Bearer {{accessToken}}

**Body:**

```json
{
  "userId": "user-id",
  "roles": ["Admin", "User"],
  "claims": [
    {
      "type": "permission",
      "value": "read"
    },
    {
      "type": "permission",
      "value": "write"
    }
  ]
}
```

---

## CourierController - Get Courier By ID

**Method:** `GET`

**URL:** `{{baseUrl}}/api/v1.0/couriers/{{courierId}}`

**Description:**


### Get Courier Details
This endpoint retrieves details of a specific courier identified by the `courierId`.

#### Request
- Method: GET
- URL: `{{baseUrl}}/api/v1.0/couriers/{{courierId}}`

#### Response
The response of this request is a JSON object conforming to the following schema:
```json
{
  "type": "object",
  "properties": {
    "courierId": {
      "type": "string"
    },
    "name": {
      "type": "string"
    },
    "phone": {
      "type": "string"
    },
    "email": {
      "type": "string"
    },
    "status": {
      "type": "string"
    },
    "location": {
      "type": "object",
      "properties": {
        "latitude": {
          "type": "number"
        },
        "longitude": {
          "type": "number"
        }
      }
    },
    "lastUpdated": {
      "type": "string",
      "format": "date-time"
    }
  }
}
```


**Headers:**

- Authorization: Bearer {{accessToken}}

---

## CourierController - Get All Couriers

**Method:** `GET`

**URL:** `{{baseUrl}}/api/v1.0/couriers/list?page={{page}}&pageSize={{pageSize}}`

**Description:**


This endpoint makes an HTTP GET request to retrieve a list of couriers. The request includes query parameters for pagination, where "page" specifies the page number and "pageSize" specifies the number of items per page.

The response of this request is documented as a JSON schema:
```json
{
  "type": "object",
  "properties": {
    "couriers": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "courierId": {
            "type": "string"
          },
          "name": {
            "type": "string"
          },
          "contactNumber": {
            "type": "string"
          },
          "email": {
            "type": "string"
          },
          "status": {
            "type": "string"
          }
        }
      }
    },
    "totalCouriers": {
      "type": "integer"
    }
  }
}
```


---

## CourierController - Create Courier

**Method:** `POST`

**URL:** `{{baseUrl}}/api/v1.0/couriers`

**Description:**


This endpoint allows the creation of new couriers via a POST request to the specified URL. The request should include the necessary details of the courier to be created.

### Response
The response of this request is a JSON schema representing the structure of the response data that will be returned upon successful creation of a new courier.


**Headers:**

- Content-Type: multipart/form-data
- Authorization: Bearer {{accessToken}}

---

## CourierController - Update Courier

**Method:** `PUT`

**URL:** `{{baseUrl}}/api/v1.0/couriers/{{courierId}}`

**Description:**


This endpoint allows the user to update a specific courier by making an HTTP PUT request to the provided URL. The request should include the courier ID in the URL path and the payload in form-data format.

### Response
The response of this request is a JSON schema representing the structure of the data that will be returned. This schema can be used to understand the expected format of the response data.


**Headers:**

- Content-Type: multipart/form-data
- Authorization: Bearer {{accessToken}}

---

## CourierController - Soft Delete Courier

**Method:** `PATCH`

**URL:** `{{baseUrl}}/api/v1.0/couriers/{{courierId}}`

**Description:**


This API endpoint allows you to update specific details of a courier using the HTTP PATCH method. You need to provide the ID of the courier in the URL.

### Request Parameters
- `courierId`: The ID of the courier to be updated.

### Response
The response of this request is a JSON schema representing the updated courier details. The schema will outline the structure and data types of the updated courier information.


**Headers:**

- Authorization: Bearer {{accessToken}}

---

## CourierController - Add or Update CNH Image

**Method:** `PATCH`

**URL:** `{{baseUrl}}/api/v1.0/couriers/{{cnpj}}/cnh`

**Description:**


This endpoint allows you to update the CNH (National Driver's License) information for a specific courier identified by their CNPJ number. The HTTP PATCH request should be made to {{baseUrl}}/api/v1.0/couriers/{{cnpj}}/cnh.

### Request Body
The request should include a form-data body with the necessary fields to update the CNH information.

### Response
The response of this request is a JSON schema representing the structure of the response data that will be returned upon a successful update of the CNH information for the courier.

```json
{
  "type": "object",
  "properties": {
    "status": {
      "type": "string"
    },
    "message": {
      "type": "string"
    }
  }
}
```


**Headers:**

- Content-Type: multipart/form-data
- Authorization: Bearer {{accessToken}}

---

## MotorcycleController - Get Motorcycle By ID

**Method:** `GET`

**URL:** `{{baseUrl}}/api/v1.0/motorcycles/{{motorcycleId}}`

**Description:**


This endpoint retrieves information about a specific motorcycle identified by the provided motorcycleId.

### Response
The response of this request is a JSON object conforming to the following schema:

```json
{
  "type": "object",
  "properties": {
    "motorcycleId": {
      "type": "string"
    },
    "brand": {
      "type": "string"
    },
    "model": {
      "type": "string"
    },
    "year": {
      "type": "integer"
    },
    "engine": {
      "type": "object",
      "properties": {
        "type": {
          "type": "string"
        },
        "displacement": {
          "type": "number"
        }
      }
    },
    "colors": {
      "type": "array",
      "items": {
        "type": "string"
      }
    }
  }
}
```


**Headers:**

- Authorization: Bearer {{accessToken}}

---

## MotorcycleController - Get All Motorcycles

**Method:** `GET`

**URL:** `{{baseUrl}}/api/v1.0/motorcycles/list?page={{page}}&pageSize={{pageSize}}`

**Description:**


This endpoint sends an HTTP GET request to retrieve a list of motorcycles. The request includes query parameters for the page number and page size.

### Response
The response of this request is a JSON object representing the list of motorcycles. To document the response as a JSON schema, the following keys and their data types can be used:

- `motorcycles`: An array of objects representing the motorcycles.
  - `id`: (type: string) The unique identifier of the motorcycle.
  - `brand`: (type: string) The brand of the motorcycle.
  - `model`: (type: string) The model of the motorcycle.
  - `year`: (type: number) The manufacturing year of the motorcycle.
  - `engineSize`: (type: string) The engine size of the motorcycle.
  - `color`: (type: string) The color of the motorcycle.
  - `price`: (type: number) The price of the motorcycle.

Example response:
```json
{
  "motorcycles": [
    {
      "id": "abc123",
      "brand": "Honda",
      "model": "CBR600RR",
      "year": 2020,
      "engineSize": "600cc",
      "color": "Red",
      "price": 8000
    },
    {
      "id": "def456",
      "brand": "Yamaha",
      "model": "YZF-R6",
      "year": 2019,
      "engineSize": "600cc",
      "color": "Blue",
      "price": 7500
    }
  ]
}
```


---

## MotorcycleController - Create Motorcycle

**Method:** `POST`

**URL:** `{{baseUrl}}/api/v1.0/motorcycles`

**Description:**


### Create a New Motorcycle

This endpoint allows you to create a new motorcycle.

#### Request Body
- `year` (number, required): The year of the motorcycle.
- `model` (string, required): The model of the motorcycle.
- `plate` (string, required): The license plate of the motorcycle.

#### Response
The response for this request can be documented as a JSON schema.


**Headers:**

- Content-Type: application/json
- Authorization: Bearer {{accessToken}}

**Body:**

```json
{
  "year": 2024,
  "model": "Honda",
  "plate": "ABC-9007"
}
```

---

## MotorcycleController - Update Motorcycle

**Method:** `PUT`

**URL:** `{{baseUrl}}/api/v1.0/motorcycles/{{motorcycleId}}`

**Description:**


### Update Motorcycle Details

This endpoint allows the client to update the details of a specific motorcycle.

#### Request

- Method: PUT
- URL: `{{baseUrl}}/api/v1.0/motorcycles/{{motorcycleId}}`

##### Request Body
- Type: JSON
    - `year` (number): The year of the motorcycle.
    - `model` (string): The model of the motorcycle.
    - `plate` (string): The license plate of the motorcycle.

#### Response

The response of this request is a JSON schema representing the updated details of the motorcycle. The specific structure of the response will depend on the data that is updated.



**Headers:**

- Content-Type: application/json
- Authorization: Bearer {{accessToken}}

**Body:**

```json
{
  "year": 2022,
  "model": "Yamaha",
  "plate": "ABC-9001"
}
```

---

## MotorcycleController - Soft Delete Motorcycle

**Method:** `PATCH`

**URL:** `{{baseUrl}}/api/v1.0/motorcycles/{{motorcycleId}}/status`

**Description:**


This endpoint allows the user to update the status of a specific motorcycle using a PATCH request. The request should be made to the specified URL with the motorcycleId as a path parameter. Upon successful execution, the response will be in the form of a JSON schema documenting the structure of the response data.


**Headers:**

- Authorization: Bearer {{accessToken}}

---

## MotorcycleController - Get Motorcycle Notification

**Method:** `GET`

**URL:** `{{baseUrl}}/api/v1.0/motorcycles/{{motorcycleId}}/notification`

**Description:**

This endpoint retrieves the notification details for a specific motorcycle identified by the `motorcycleId`.

### Response

The response for this request is a JSON object conforming to the following schema:

``` json
{
  "type": "object",
  "properties": {
    "notificationId": {
      "type": "string",
      "description": "The unique identifier for the notification."
    },
    "message": {
      "type": "string",
      "description": "The message content of the notification."
    },
    "timestamp": {
      "type": "string",
      "format": "date-time",
      "description": "The timestamp when the notification was sent."
    }
  }
}

 ```

---

## RentalController - Get Rental By ID

**Method:** `GET`

**URL:** `{{baseUrl}}/api/v1.0/rentals/{{rentalId}}`

**Description:**


This endpoint retrieves the details of a specific rental identified by the `rentalId`.

### Response
The response for this request is a JSON object conforming to the following schema:

```json
{
  "type": "object",
  "properties": {
    "rentalId": {
      "type": "string",
      "description": "The unique identifier of the rental"
    },
    "address": {
      "type": "string",
      "description": "The address of the rental property"
    },
    "price": {
      "type": "number",
      "description": "The rental price"
    },
    "bedrooms": {
      "type": "integer",
      "description": "The number of bedrooms in the rental property"
    },
    "bathrooms": {
      "type": "integer",
      "description": "The number of bathrooms in the rental property"
    },
    "sqft": {
      "type": "integer",
      "description": "The square footage of the rental property"
    },
    "petsAllowed": {
      "type": "boolean",
      "description": "Indicates if pets are allowed in the rental property"
    }
  },
  "required": ["rentalId", "address", "price", "bedrooms", "bathrooms", "sqft", "petsAllowed"]
}
```


**Headers:**

- Authorization: Bearer {{accessToken}}

---

## RentalController - Get All Rentals

**Method:** `GET`

**URL:** `{{baseUrl}}/api/v1.0/rentals/list?page={{page}}&pageSize={{pageSize}}`

**Description:**


This endpoint makes an HTTP GET request to retrieve a list of rentals. The request includes query parameters for the page number and page size.

### Response
The response of this request is a JSON object representing the list of rentals. To document the response as a JSON schema, you can use the following structure:

```json
{
  "type": "object",
  "properties": {
    "rentals": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
        }
      }
    },
    "totalRentals": {
      "type": "integer"
    },
    "page": {
      "type": "integer"
    },
    "pageSize": {
      "type": "integer"
    }
  }
}
```

Replace "// Define the properties of each rental object here" with the specific properties of each rental object, such as id, address, price, etc.



---

## RentalController - Create Rental

**Method:** `POST`

**URL:** `{{baseUrl}}/api/v1.0/rentals`

**Description:**


This endpoint makes an HTTP POST request to create a new rental in the system. The request should include the courier ID, motorcycle ID, start date, end date, expected end date, daily rate, and the rental plan.

### Request Body
- courierId (string): The ID of the courier associated with the rental.
- motorcycleId (string): The ID of the motorcycle being rented.
- startDate (string): The start date of the rental period.
- endDate (string): The end date of the rental period.
- expectedEndDate (string): The expected end date of the rental period.
- dailyRate (number): The daily rental rate for the motorcycle.
- plan (string): The rental plan chosen for the rental.

### Response
The response of this request is a JSON schema representing the structure of the response object. The schema will define the properties and data types of the response object returned upon successful creation of the rental.



**Headers:**

- Content-Type: application/json
- Authorization: Bearer {{accessToken}}

**Body:**

```json
{
  "courierId": "{{courierId}}",
  "motorcycleId": "{{motorcycleId}}",
  "startDate": "2024-07-23T00:00:00Z",
  "endDate": "2024-07-31T00:00:00Z",
  "expectedEndDate": "2024-07-30T00:00:00Z",
  "dailyRate": 30.0,
  "plan": "SevenDays"
}
```

---

## RentalController - Update Rental

**Method:** `PUT`

**URL:** `{{baseUrl}}/api/v1.0/rentals/{{rentalId}}`

**Description:**


This endpoint allows the user to update a specific rental by sending an HTTP PUT request to the specified URL. The request should include the rental ID in the URL and a JSON payload in the request body with the fields: courierId, motorcycleId, startDate, endDate, expectedEndDate, dailyRate, and plan.

### Response
The response of this request is a JSON schema representing the structure of the data that will be returned. This schema can be used to understand the format of the response data and validate it against the expected structure.


**Headers:**

- Content-Type: application/json
- Authorization: Bearer {{accessToken}}

**Body:**

```json
{
  "courierId": "{{courierId}}",
  "motorcycleId": "{{motorcycleId}}",
  "startDate": "2024-07-18T00:00:00Z",
  "endDate": "2024-07-26T00:00:00Z",
  "expectedEndDate": "2024-07-25T00:00:00Z",
  "dailyRate": 30.0,
  "plan": "SevenDays"
}
```

---

## RentalController - Soft Delete Rental

**Method:** `PATCH`

**URL:** `{{baseUrl}}/api/v1.0/rentals/{{rentalId}}`

**Description:**


This endpoint allows the user to update specific details of a rental using the rentalId as a reference. The HTTP PATCH request should be made to the specified URL with the necessary parameters to update the rental information.

### Response
The response of this request is a JSON schema representing the updated rental details. The schema will outline the structure of the response data, including the type and format of each field returned after the successful update.


**Headers:**

- Authorization: Bearer {{accessToken}}

---

## HealthChecks - Check HealthChecks

**Method:** `GET`

**URL:** `{{baseUrl}}/health`

**Description:**


The endpoint performs an HTTP GET request to check the health status of the system components. The response is in JSON format and includes the status of different system components such as npgsql, redis, rabbitmq, and azure_blob_storage. Each component entry contains data, duration, status, and tags information.

```json
{
  "type": "object",
  "properties": {
    "status": {"type": "string"},
    "totalDuration": {"type": "string"},
    "entries": {
      "type": "object",
      "properties": {
        "npgsql": {
          "type": "object",
          "properties": {
            "data": {"type": "object"},
            "duration": {"type": "string"},
            "status": {"type": "string"},
            "tags": {"type": "array"}
          }
        },
        "redis": {
          "type": "object",
          "properties": {
            "data": {"type": "object"},
            "duration": {"type": "string"},
            "status": {"type": "string"},
            "tags": {"type": "array"}
          }
        },
        "rabbitmq": {
          "type": "object",
          "properties": {
            "data": {"type": "object"},
            "duration": {"type": "string"},
            "status": {"type": "string"},
            "tags": {"type": "array"}
          }
        },
        "azure_blob_storage": {
          "type": "object",
          "properties": {
            "data": {"type": "object"},
            "duration": {"type": "string"},
            "status": {"type": "string"},
            "tags": {"type": "array"}
          }
        }
      }
    }
  }
}
```


---

