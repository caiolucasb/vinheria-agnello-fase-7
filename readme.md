# Vinheria Agnello – Ambiente de Microsserviços com Docker e Jenkins

Este repositório contém a configuração utilizada para simular a arquitetura da Vinheria Agnello, incluindo dois microsserviços em Docker, comunicação via rede dedicada, autenticação JWT e um pipeline automatizado de deploy com Jenkins.

## 1. Estrutura do Ambiente

A arquitetura contém:

- AuthService: serviço responsável pela autenticação e geração de tokens JWT.
- CatalogService: serviço que lista os vinhos e valida o token enviado no header.
- Jenkins em container: executa o pipeline já incluído neste repositório.
- Rede Docker interna (`vinheria-network`): permite comunicação entre os serviços.
- Pasta de publicação (`/var/www/vinheria`) dentro do Jenkins para receber o deploy.

## 2. Criando a rede Docker

```sh
docker network create vinheria-network
```

## 3. Executando os microsserviços
AuthService

```sh
docker run -d \
  --name auth-service \
  --network vinheria-network \
  -p 3001:3001 \
  auth-service-image
```

CatalogService

```sh
docker run -d \
  --name catalog-service \
  --network vinheria-network \
  -p 3002:3002 \
  catalog-service-image
  ```

Para verificar:

```sh
docker ps
```

## 4. Subindo o Jenkins

```sh
docker run -d \
  --name jenkins \
  --network vinheria-network \
  -p 8080:8080 \
  -p 50000:50000 \
  -v jenkins_home:/var/jenkins_home \
  jenkins/jenkins:lts
```

A senha inicial do Jenkins pode ser obtida com:

```sh
docker exec -it jenkins cat /var/jenkins_home/secrets/initialAdminPassword
```

Acesse pelo navegador:
http://localhost:8080


## 5. Configuração do Jenkins
Plugins necessários
- Git Plugin
- SSH Agent Plugin
- Pipeline

A instalação é feita em:
Manage Jenkins → Plugins

### Geração da chave SSH dentro do container

Acesse o container:

```sh
docker exec -it jenkins bash
```

Gere a chave:

```sh
ssh-keygen -t ed25519 -C "jenkins-access"
```

A chave será criada em:

```sh
/var/jenkins_home/.ssh/id_ed25519
/var/jenkins_home/.ssh/id_ed25519.pub
```

### Adicionando a chave pública ao GitHub

O conteúdo da chave pública deve ser copiado para:

- GitHub → Settings → SSH and GPG Keys → New SSH Key

Adicionando a chave privada no Jenkins
No Jenkins:

- Manage Jenkins → Credentials → System → Global → Add Credentials

- Tipo: SSH Username with private key

- Username: git

- Private Key: cole o conteúdo de /var/jenkins_home/.ssh/id_ed25519

- ID: usado pelo pipeline (já configurado no repositório)
