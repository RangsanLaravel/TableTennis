angular.module('tourEditModule', [])
    .controller('tourEditController', ['$scope', '$filter', '$service', '$timeout', '$window', '$http', function ($scope, $filter, $service, $timeout, $window, $http) {
        $scope.url = {};

        $scope.master = {
            tournament: {},
            rangking:{}
        };

        $scope.model = {
            tournament: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.tournament.fix) { $scope.model.tournament.val = ''; $scope.model.tournament = []; } }
            },
            rangking: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.rangking.fix) { $scope.model.rangking.val = ''; $scope.model.rangking = []; } }
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

        $scope.gettournamentcode = function () {
            $scope.showloading();
            $service.gettournamentcode($scope.url.GetTournamentCode).then(
                   function (success) {
                       $scope.master.tournament = success.data[1];
                       $scope.master.rangking = success.data[2];
                       $scope.hideloading();
                   }, function (error) {
                       if (error !== null)
                           swal('error', error, 'error');
                       $scope.hideloading();
                   });
        };

        $scope.getdata = function () {
            var item = $scope.master.tournament.find(function (v) { return v.ID == $scope.model.tournament.val; });           
            $scope.model.tournament.series = item;         
            $scope.model.tournament.fix = true;
            $service.setdata($scope.url.SetData, $scope.model.tournament.val).then(
               function (success) {
                   $scope.master.rangking.forEach(function (element)
                   {
                       if (element.ID == item.RANKING_ID) {
                           $scope.model.rangking.val = element;
                       }
                   })                  
                   
                   $scope.hideloading();
               },
               function (error) {
                   swal("เกิดข้อผิดพลาด", error, "error");
                   $scope.hideloading();
               }
               );
        };

        $scope.setRankID = function () {         
            $scope.model.tournament.series.RANKING_ID = $scope.model.rangking.val.ID;               
        };

        $scope.updatetournamentConfirm = function () {
            swal({
                title: "ยืนยันการแก้ไขทัวร์นาเม้น : " + $scope.model.tournament.series.CODE,
                text: "Once update, you will not be able to recover this event!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
           .then((willDelete) => {
               if (willDelete) {
                   $scope.updatetournament();
               } else {
                   swal("ยกเลิกการแก้ไข");
               }
           });
        };

        $scope.updatetournament = function () {
            $scope.showloading();
            $service.updatetournament($scope.url.UpdateTournament, $scope.model.tournament.series).then(
               function (success) {
                   $scope.session(success.session);
                   swal("แก้ไขข้อมูลสำเร็จ", "แก้ไขข้อมูลสำเร็จ", "success");
                   $scope.hideloading();
               },
               function (error) {
                   swal("เกิดข้อผิดพลาด", error, "error");
                   $scope.hideloading();
               }
               );
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

            updatetournament: function (url, data) {
                var deferref = $q.defer();
                $http.post(url, { dataObject: data })
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

            setdata: function (url, data) {
                var deferref = $q.defer();
                $http.post(url, { data: data })
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