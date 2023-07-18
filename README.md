# API .NET 6 - Agenda

## Funcionalidades

A API possui as seguintes funcionalidades:

- Serviço de Médicos:
  - Consulta de consultas marcadas por médico
  - Agendamento de consultas por médico, evitando duplicações no mesmo horário

- Serviço de Pacientes:
  - Consulta da agenda de um médico específico
  - Agendamento de consultas pelos pacientes
  - Consulta das consultas agendadas por um paciente

## Requisitos

- .NET 6
- Banco de dados Entity InMemoryDatabase

## Estrutura do Projeto

O projeto segue uma arquitetura padrão de web API com a separação das camadas lógicas:

- `Controllers`: Contém os controladores da API
- `Middleware`: Contém os handdler de erros customizaveis da API
- `Services`: Implementa a lógica de negócio da aplicação
- `Repositories`: Responsável pela comunicação com o banco de dados
- `Models`: Classes de modelos utilizadas pela API
- `Tests`: Contém os testes unitários para as funcionalidades da API

## Testes

A API possui uma cobertura de testes de 80%. Os testes foram implementados utilizando um framework de teste adequado (XUnit)


## Executando a Aplicação

Siga as etapas abaixo para executar a aplicação:

1. Clone o repositório do projeto:
   ```
   git clone https://github.com/Potestino/Agenda.git
   ```

2. Abra o projeto no Visual Studio (ou outra IDE compatível com .NET Framework 6).

3. Restaure as dependências do NuGet.

4. Compile e execute a aplicação.

## Docker

A aplicação também pode ser executada em um contêiner Docker. Para isso, siga as etapas abaixo:

1. Certifique-se de ter o Docker instalado e em execução na máquina.

2. Abra o projeto no visual studio.

3. troque a opção de execução para Docker e clique em play
