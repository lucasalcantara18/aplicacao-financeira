
# Cubos Challange

Um projeto desenvolvido em C# que fornece uma solução para o calculo de operações financeiras.

## Pré requisitos

Para rodar essa solução são necessários:

-C# Instalado na máquina. \
-Visual studio 2022.\
-Docker Desktop


## Começando

Considerando que se tenha o ambiente já preparado para o .Net e a IDE do visual studio 2022 instalados

1. Faça o clone do projeto na maquina em uma pasta de sua escolha

2. Abra o projeto através do arquivo: WebAppCubos.sln

3. Compile o projeto

4. Para rodar a aplicação localmente:

4.1. Verifique que o item de inicialização esteja em WebAppCubos

4.2 Clique em 'https' para rodar o projeto

4.3 Após o processo estiver completo basta ir ao navegador atrávez do endereço

```bash
  https://localhost:7281/swagger/index.html
```

5. Para rodar a aplicação via docker

5.1. Verifique que o item de inicialização esteja em docker-compose

5.2 Clique em 'Docker Compose' para rodar o projeto

5.3 Após o processo estiver completo basta ir ao navegador atrávez do endereço

6. Para rodar os testes unitarios

6.1. Verifique que o item de inicialização esteja em UnitTests

6.2 Clique em 'Unitstests' para rodar os testes

Cetifique se que o Docker Desktop esteja rodando com um container do postgress em execução

Ao rodar o projeto localmente, o arquivo AppSettings.Development contem as credenciais para o banco postgres, certifique de rodar uma imagem contendo as mesmas credenciais no docker desktop

Ao rodar via docker, já irá subir automaticamente uma imagem do postgress configurada com as credenciais corretas


## Stack

**Back-end:** C#\
**Testes:** xUnit

## Observações

Para rodar a aplicação via Docker pode ser que seja nescessario rodar uma configuração de certificado https, com o seguinte comando:

```bash
  dotnet dev-certs https -ep ${USERPROFILE}\.aspnet\https\aspnetapp.pfx -p YourPasswordHere
```

Certifique que no arquivo docker-compose.override.yml as variaveis estejam com as informações que você rodou no comando, para que o certificado seja adicionado no container



## Variáveis de Ambiente

Para rodar esse projeto, você vai precisar adicionar as seguintes variáveis de ambiente no arquivo AppSettings

`ComplianceCredencials: email`

Email gerado para acesso ao Compliance Api

`ComplianceCredencials: password`

password gerado para acesso ao Compliance Api

