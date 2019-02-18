radix
==============

Passo a passo para utilizar a api desenvolvida e uma breve explicação sobre o site.

## Demonstração
https://apiradix.azurewebsites.net

## Instalação

1. Faça um clone deste projeto com `git clone https://github.com/arthurnuciatelli/radix.git`
2. Abra o projeto no Visual Studio (projeto foi desenvolvido no Visual Studio 2017)
3. Caso queira garantir que todos os pacotes foram baixados, clique com o botão direito em cima do nome da Solution, no Visual Studio, e selecione o menu Restore Client-Side Libraries. Lembrando que ao clonar o projeto, automaticamente os pacotes já vem incluídos.
4. Pressione ctrl + f5 para executar a api em modo Sem Debug
5. Clique com o botáo direito do mouse sobre a pasta wwwroot > View in Browser (*Google Chrome*)

## API
<p>
  O caminho da api sugerido no exemplo seguiu a nomenclatura levando em consideração que a aplicação estará sendo executada localmente, na máquina de desenvolvimento. Em todo caso, foi liberada uma publicação no Azure para realização de testes, caso preferir, segue endereço do web site e api, respectivamente:
 </p>
  <br>`https://apiradix.azurewebsites.net`
  <br>`https://apiradix.azurewebsites.net/api/evento`


### Get
`https://localhost:{porta}/api/evento` - Retorna um array de objetos JSON no seguinte formato:<br>
```json
[
    {
        "id": 1,
        "timestamp": 1550456195,
        "tag": "brasil.sudeste.sensor01",
        "valor": "23",
        "status": "Processado ou Erro"
    }
]
```

### Get/{id}
`https://localhost:{porta}/api/evento/{id}` - Retorna um objeto JSON no seguinte formato, se acordo com o id solicitado:<br>
 ```json
 {
    "id": 1,
    "timestamp": 1550456195,
    "tag": "brasil.sudeste.sensor01",
    "valor": "",
    "status": "Erro"
}
```
Caso não encontre o id informado, retorna um objeto JSON no seguinte formato:<br>
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "8000001e-0002-ff00-b63f-84710c7967bb"
}
```
### Post
`https://localhost:{porta}/api/evento` - Inseri um registro de sensor. Deve se utilizar o seguinte formato JSON, no corpo:<br>
 ```json
 { 
    "timestamp": 1550456195,
    "tag": "brasil.sudeste.sensor01",
    "valor": "23",
}
```
E retorna um objeto JSON com o seguinte formato:<br>
```json
{
    "id": 2,
    "timestamp": 1550351789,
    "tag": "brasil.sudeste.sensor23",
    "valor": "2",
    "status": "Processado"
}
```
