## Table of Contents
1. [Reference Authorization Microservice](#refauthmicroservice)
2. [Generate OpenSSL RSA Key Pair from the Command Line](#generateprivate&publickeys)

# Reference Authorization Microservice <a name="refauthmicroservice"> </a>
[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com) [![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](https://forthebadge.com)  
This microservice is an example of **OAuth2 authorizatoin** microservice.

## Prerequisites
You should have [.NET Core 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed

## How to run it?

1. Setup a [MSSQL](https://linux.how2shout.com/how-to-install-microsoft-sql-server-on-ubuntu-20-04-lts/)
2. Launch *application* with *migrations* by running this command in the project root:
    ```
    dotnet run --project Authorization/Authorization.Host
    ```

## Open API via Swagger:
```
http://localhost:4001/swagger/index.html
 ```

## Supported OAuth2 flows
As for now it supports only one flow - *Resource Owner Password Credential Grant Type Flow*.

![image](https://docs.oracle.com/cd/E39820_01/doc.11121/gateway_docs/content/images/oauth/oauth_username_password_flow.png)

# Generate OpenSSL RSA Key Pair from the Command Line <a name="generateprivate&publickeys"> </a>
1) Generate a 2048 bit RSA Key
   You can generate a public and private RSA key pair like this:
```bash
openssl genrsa -out private-key.pem 2048
```
That generates a 2048-bit RSA key pair, encrypts them with a password you provide and writes them to a file. You need to next extract the public key file. You will use this, for instance, on your web server to encrypt content so that it can only be read with the private key.
2) Export the RSA Public Key to a File
   This is a command that is:
```bash
openssl rsa -in private-key.pem -outform PEM -pubout -out public-key.pem
```
The -pubout flag is really important. Be sure to include it.

Next open the public-key.pem and ensure that it starts with -----BEGIN PUBLIC KEY-----. This is how you know that this file is the public key of the pair and not a private key.
To check the file from the command line you can use the less command, like this:
```bash
less public-key.pem
```
3) Visually Inspect Your Key Files
   It is important to visually inspect you private and public key files to make sure that they are what you expect. OpenSSL will clearly explain the nature of the key block with a -----BEGIN RSA PRIVATE KEY----- or -----BEGIN PUBLIC KEY-----.

You can use less to inspect each of your two files in turn:

    less private-key.pem to verify that it starts with a -----BEGIN RSA PRIVATE KEY-----
    less public-key.pem to verify that it starts with a -----BEGIN PUBLIC KEY-----


