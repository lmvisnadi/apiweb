# APIWeb

## Summary

* [Arquitetura Interna](#markdown-header-arquitetura-interna)
* [Configuração de Ambiente](#markdown-header-configuracao-de-ambiente)
    * [Docker](#markdown-header-docker)
    * [Conexao com o Banco de Dados](#markdown-header-conexao-com-o-banco-de-dados)
* [Configurar o APIWeb](#markdown-header-configurar-o-APIWeb)
    * [Banco de Dados](#markdown-header-banco-de-dados) 
	* [JWT] (#markdown-JWT) 
* [API Reference ](#markdown-header-API-reference)
	* [Post Login](#markdown-header-post-login)
	* [Post UnidadeFederativa](#markdown-header-post-unidadefederativa)
	* [Put UnidadeFederativa](#markdown-header-put-unidadefederativa)
	* [Delete UnidadeFederativa](#markdown-header-delete-unidadefederativa)
	* [Get UnidadeFederativa](#markdown-header-get-unidadefederativa)
	* [Post municipio](#markdown-header-post-municipio)
	* [Put municipio](#markdown-header-put-municipio)
	* [Delete municipio](#markdown-header-delete-municipio)
	* [Get municipio](#markdown-header-get-municipio)
	* [Post cliente](#markdown-header-post-cliente)
	* [Put cliente](#markdown-header-put-cliente)
	* [Delete cliente](#markdown-header-delete-cliente)
	* [Get cliente](#markdown-header-get-cliente)


## Arquitetura Interna

![Arquitetura](ReadmeFiles/APIWeb_arq_interna.png "Arquitetura")


## Configuracao de Ambiente

Passo a passo para configuração o ambiente de desenvolvimento.

### Docker

Certifique-se de possuir o **[Docker](https://www.docker.com/products/docker-desktop)** instalado na máquina, 
com suporte ao **[WSL2](https://docs.microsoft.com/pt-br/windows/wsl/install)**, caso o OS seja Windows.


1. Abrir o terminal na pasta `docker` na raiz da solução.
2. Executar o comando abaixo subir os containers do **PostGreeSQL**  **APIWeb**.
   
```console
    docker-compose -f docker-compose-dev.yml --env-file .env.dev up
```

> Após a primeira execução, os containers podem ser administrador pela interface grafica do docker


1. Instalar o **[DBeaver](https://dbeaver.io/download/)** (ou outro software de escolha) para consultas no banco de dados. 

### Conexao com o Banco de Dados

Para a conexão e criação do usuario no banco de dados, utilizaremos o **postgres**.

#### Criar uma nova conexão

**Usuario:** postgres

**Senha:** tst2022

#### Criar usuario

**Usuario:** Admin

**Senha:** 1234

Pegar o arquivo insert.txt

### Pronto!

Agora seu ambiente está configurado! ;)

Basta executar a aplicação em Development que o sistema se encarrega de criar os 
tópicos necessários e rodar as migrations do EF.

## Configurar o APIWeb

Configurações no appsettings.Development.json

### Banco de Dados

```http
   "ConnectionStrings": "AdminDatabase": "User ID={User ID};Password={Password};Host={Host};Port={Port};Database={Database};Pooling=true;Integrated Security=true;"
```

| Parametro          | Descrição                         |
| :----------------- | :-------------------------------- |
| `User ID`          | usuario                           |
| `Password`         | senha do usuario                  |
| `Host`             | servidor                          |
| `Port`             | porta                             |
| `Database`         | database                          |

### JWT

```http
    "Jwt": {
    "Key": {Key},
    "Issuer": {Issuer}
  }
```

| Parametro          | Descrição                         |
| :----------------- | :-------------------------------- |
| `Key`              | key     (chave da criptografia)   |
| `Issuer`           | issuer  (Emissor do token)        |


## API Reference

#### Post Login

```http
  Post Login
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Email`   | `string` | **Required**. email do usuario    |
| `Password`| `string` | **Required**. senha               |
| `Timezone`| `string` | **Required**. Timezone            |

#### Post UnidadeFederativa

```http
  Post UnidadeFederativa
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Sigla`   | `string` | **Required**. sigla do estado     |
| `Nome`    | `string` | **Required**. nome do estado      |


#### Put UnidadeFederativa

```http
  Put UnidadeFederativa
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `Guid`   | **Required**. id                  |
| `Sigla`   | `string` | **Required**. sigla do estado     |
| `Nome`    | `string` | **Required**. nome do estado      |


#### Delete UnidadeFederativa

```http
  Delete UnidadeFederativa
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `Guid`   | **Required**. id                  |


#### Get UnidadeFederativa

```http
  GET UnidadeFederativa/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `Guid`   | **Optional**. id                  |


#### Post Municipio

```http
  Post Municipio
```

| Parameter             | Type     | Description                       |
| :--------             | :------- | :-------------------------------- |
| `Nome`                | `string` | **Required**. nome do municipio   |
| `IdUnidadeFederativa` | `guid`   | **Required**. id da UF            |

#### Put Municipio

```http
  Put Municipio
```

| Parameter             | Type     | Description                       |
| :--------             | :------- | :-------------------------------- |
| `Id`      			| `Guid`   | **Required**. id                  |
| `Nome`                | `string` | **Required**. nome do municipio   |
| `IdUnidadeFederativa` | `guid`   | **Required**. id da UF            |

#### Delete Municipio

```http
  Delete Municipio
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `Guid`   | **Required**  id                  |

#### Get Municipio

```http
  GET Municipio/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `Guid`   | **Optional**  id                  |


#### Post Cliente

```http
  Post Cliente
```

| Parameter                       | Type     | Description                       |
| :--------                       | :------- | :-------------------------------- |
| `Nome`                          | `string` | **Required** nome do Cliente      |
| `Documento`                     | `string` | **Required** documento            |
| `Enderecos`                     | `lista`  | **Optional** lista endereços      |
|            `TipoEndereco`       | `integer`| **Required** tipo de endereço     |
|            `Cep`                | `string` | **Required** cep                  |
|            `Logradouro`         | `string` | **Required** logradouro           |
|            `Numero`             | `string` | **Required** numero               |
|            `Complemento`        | `string` | **Optional** complemento          |
|            `Bairro`             | `string` | **Optional** Bairro               |
|            `unidadefederativaId`| `guid`   | **Required** id da uf             |
|            `municipioid`        | `guid`   | **Required** id do municipio      |


#### Put Cliente

```http
  Put Cliente
```

| Parameter                       | Type     | Description                       |
| :--------                       | :------- | :-------------------------------- |
| 'Id'                            | 'Guid'   | **Required** id do Cliente        |
| `Nome`                          | `string` | **Required** nome do Cliente      |
| `Documento`                     | `string` | **Required** documento            |
| `Enderecos`                     | `lista`  | **Optional** lista endereços      |
|            `TipoEndereco`       | `integer`| **Required** tipo de endereço     |
|            `Cep`                | `string` | **Required** cep                  |
|            `Logradouro`         | `string` | **Required** logradouro           |
|            `Numero`             | `string` | **Required** numero               |
|            `Complemento`        | `string` | **Optional** complemento          |
|            `Bairro`             | `string` | **Optional** Bairro               |
|            `unidadefederativaId`| `guid`   | **Required** id da uf             |
|            `municipioid`        | `guid`   | **Required** id do municipio      |

#### Delete Cliente

```http
  Delete Cliente
```

| Parameter                       | Type     | Description                       |
| :--------                       | :------- | :-------------------------------- |
| 'Id'                            | 'Guid'   | **Required** id do Cliente        |

#### Get Cliente

```http
  GET Cliente/{id}
```

| Parameter                       | Type     | Description                       |
| :--------                       | :------- | :-------------------------------- |
| 'Id'                            | 'Guid'   | **Optional** id do Cliente        |

