angular.module('tourModule', [])
    .controller('tourController', ['$scope', '$filter', '$service', '$timeout', '$window', '$http', function ($scope, $filter, $service, $timeout, $window, $http) {
        $scope.url = {};

        $scope.master = {
            tournament: {},         
        };

        $scope.model = {
            tournament: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.tournament.fix) { $scope.model.tournament.val = ''; $scope.model.tournament = []; } }
            },       
        };

        $scope.initial = function () {
            
            $scope.geturl();

        };

        $scope.geturl = function () {

            if ($('#hdGetUrl').val() != undefined && $('#hdGetUrl').val() != '') {
                $http.post($('#hdGetUrl').val())
                   .success(function (obj) {
                       $scope.url = obj;
                       if (obj != undefined && obj != '') {
                           $scope.showloading();
                           $scope.gettournamentcode();
                           $scope.hideloading();
                       }
                   });
            }

        };
       
        $scope.session = function (session) {
            if (!session) {
                $scope.sessionConfirm();
            }
        };

        $scope.sessionConfirm = function () {
            swal({
                title: "หมดเวลาการใช้งาน กรุณาเข้าสู่ระบบใหม่อีกครั้ง",
                text: "กด Yes เพื่อเข้าสู่ระบบใหม่ ",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
            .then((willDelete) => {
                if (willDelete) {
                    window.location.reload();
                }
            });
        };

        $scope.showloading = function () {
            $('.divLoading').show();
        };

        $scope.hideloading = function () {
            $('.divLoading').hide();
        };

        $scope.removeTourConfirm = function (data) {             
            swal({
                title: "ยืนยันการลบทัวร์นาเม้น : " + data,
                text: "Once deleted, you will not be able to recover this event!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
           .then((willDelete) => {
               if (willDelete) {
                   $scope.removeTour(data);
               } else {
                   swal("ยกเลิกการลบ");
               }
           });
        };

        $scope.removeTour = function (data) {
            $scope.showloading();
            $service.removeTour($scope.url.RemoveTour, data).then(
                function (success) {
                    swal("ลบสำเร็จ", "ลบสำเร็จ", "success");
                    var a = $scope.master.tournament.find(function (v) { return v.ID === data; });
                    var index =  $scope.master.tournament.indexOf(a);
                    $scope.master.tournament.splice(index, 1);
                    $scope.hideloading();
                },
                function (error) {
                    swal("เกิดข้อผิดพลาด", error, "error");
                    $scope.hideloading();
                }
                );
        };

        $scope.gettournamentcode = function () {
            $scope.showloading();
            $service.gettournamentcode($scope.url.GetTournamentCode).then(
                   function (success) {
                       $scope.master.tournament = success.data[1];
                       $scope.hideloading();
                   }, function (error) {
                       if (error !== null)
                           swal('error', error, 'error');
                       $scope.hideloading();
                   });
        };

    }])

    .service('$service', function ($http, $q) {

         showerror = function (html_errorpage) {
             if (root.indexOf("localhost") > -1)
                 window.open("", "_blank").document.write(html_errorpage);
             else
                 console.log(" some error founded... (see on localhost or admin policy) ");
         };

         return {
             gettournamentcode: function (url) {
                 var deferred = $q.defer();
                 $http.post(url)
                 .success(function (response) {
                     if (response.Successed)
                         deferred.resolve(response);
                     else
                         deferred.reject(response.ErrorMassage);
                 })
                 .error(function (response) {
                     showerror(response.ErrorMassage);
                     deferred.reject(response.ErrorMassage);
                 })
                 return deferred.promise;
             },
             removeTour: function (url, data) {
                 var deferref = $q.defer();
                 $http.post(url, { ID: data })
                 .success(function (response) {
                     if (response.Successed) {
                         deferref.resolve(response);
                     } else {
                         deferref.reject(response.ErrorMassage);
                     }
                 })
                 .error(function (response) {
                     showerror(response.ErrorMassage);
                     deferref.reject(response.ErrorMassage);
                 }
                 )
                 return deferref.promise;
             },

         };
     })

    .directive('repeatDone', function () {
        return function (scope, element, attrs) {
            if (scope.$last) {
                scope.$eval(attrs.repeatDone);
            }
        };
    })
    .directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive('disabledSpace', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs) {
                element.bind("keypress keydown", function (event) {
                    if (event.which === 32) {
                        var newValue = scope.$eval(attrs.ngModel);
                        scope[attrs.ngModel] = newValue.trim();
                        event.preventDefault();
                    }
                });
            }
        };
    })
    .directive('capitalize', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                var capitalize = function (inputValue) {
                    if (inputValue === undefined || inputValue === null) inputValue = '';
                    var capitalized = inputValue.toUpperCase();
                    if (capitalized !== inputValue) {
                        modelCtrl.$setViewValue(capitalized);
                        modelCtrl.$render();
                    }
                    return capitalized;
                };
                modelCtrl.$parsers.push(capitalize);
                capitalize(scope[attrs.ngModel]);
            }
        };
    });