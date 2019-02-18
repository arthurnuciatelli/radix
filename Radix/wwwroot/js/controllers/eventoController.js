angular.module("evento").controller("eventoController", function ($scope, $http, $timeout) {
    $scope.urlBase = "http://apiradix.azurewebsites.net/api/";
    $scope.app = "Controlador de Eventos Azure";
    $scope.reload = function () {
        $http.get(`${$scope.urlBase}evento`)
            .then(function (result) {
                $scope.eventos = result.data.eventos;
                $scope.agrupados = result.data.agrupados;
                var arraySensor = [];
                var arrayValor = [];
                $.each(result.data.charts, function (index, item) {
                    arraySensor.push(item.tag);
                    arrayValor.push(item.total);
                });

                ConstruirGrafico(arraySensor, arrayValor);
            });

        $timeout(function () {
            $scope.reload();
        }, 10000);
    };
    $scope.reload();
    $scope.gravarEvento = function (evento) {
        evento.timestamp = parseInt(evento.timestamp);
        $http.post(`${$scope.urlBase}evento`, evento)
            .then(function (result) {
                delete $scope.evento;
            });
    };

    var ConstruirGrafico = function (arraySensor, arrayValor) {
        var ctx = document.getElementById('myChart').getContext('2d');
        var chart = new Chart(ctx, {
            type: 'line',

            data: {
                labels: arraySensor,
                datasets: [{
                    label: "Período atual",
                    backgroundColor: 'rgb(141, 188, 0)',
                    borderColor: 'rgb(141, 188, 0)',
                    data: arrayValor,
                }]
            },

            options: {
                animation: {
                    duration: 0
                }
            }
        });
    };
});
