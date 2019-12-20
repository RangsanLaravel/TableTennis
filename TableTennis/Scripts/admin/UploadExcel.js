angular.module('excelModule', ['angularUtils.directives.dirPagination'])
    .controller('excelController', ['$scope', '$filter', '$service', '$timeout', '$window', '$http', function ($scope, $filter, $service, $timeout, $window, $http) {
        $scope.url = {};

        $scope.master = {
            tournament: {},
        };

        $scope.model = {
            rankdata: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.rankdata.fix) { $scope.model.rankdata.val = ''; $scope.model.rankdata.series = []; } }
            },
            ranktext: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.ranktext.fix) { $scope.model.ranktext.val = ''; $scope.model.ranktext = []; } }
            },
            rankid: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.rankid.fix) { $scope.model.rankid.val = ''; $scope.model.rankid = []; } }

            },
            rankCode: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.rankCode.fix) { $scope.model.rankCode.val = ''; $scope.model.rankCode = []; } }
            },
            dataperpage: "10",
            dataperpagelist: ["10", "20", "50", "100", "200"],

        };

        $scope.initial = function () {

            $scope.geturl();

        };

        $scope.dataperpageChang = function () {
            $scope.pagination.size = $scope.model.dataperpage;
        };

        $scope.pagination = {
            current: 1,
            begin: 0,
            size: $scope.model.dataperpage,
            maxSize: 10
        };

        $scope.geturl = function () {

            if ($('#hdGetUrl').val() != undefined && $('#hdGetUrl').val() != '') {
                $http.post($('#hdGetUrl').val())
                   .success(function (obj) {
                       $scope.url = obj;
                       if (obj != undefined && obj != '') {
                           $scope.showloading();
                           $scope.getError();
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

        $scope.getError = function () {
            $scope.showloading();
            $service.getError($scope.url.GET_ERROR_IMPORT_EXCEL).then(
                   function (success) {
                       $scope.session(success.session);
                       if (success.session)
                           if (success.data == "true")
                               swal("upload Ranking สำเร็จ", "", "success");
                       $scope.hideloading();
                   }, function (error) {
                       if (error !== null)
                           swal('error', error, 'error');
                       $scope.hideloading();
                   });
        };

        $scope.getRankcode = function () {
            $scope.showloading();
            $service.getRankcode($scope.url.GET_RANK_DT, $scope.model.rankid.val).then(
                   function (success) {
                       $scope.session(success.session);
                       if (success.session) {
                           $scope.model.rankCode.series = success.data;
                       }
                       $scope.hideloading();
                   }, function (error) {
                       if (error !== null)
                           swal('error', error, 'error');
                       $scope.hideloading();
                   });
        };

        $scope.getrankdetail = function () {
            $scope.showloading();
            $service.getrankdetail($scope.url.GET_RANK_DETAIL, $scope.model.rankCode.val, $scope.model.rankid.val).then(
                   function (success) {
                       $scope.session(success.session);
                       if (success.session) {
                           $scope.model.rankdata.series = success.data;
                       }
                       $scope.hideloading();
                   }, function (error) {
                       if (error !== null)
                           swal('error', error, 'error');
                       $scope.hideloading();
                   });
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

        $scope.removerankConfirm = function () {
            swal({
                title: "ยืนยันการลบ : " + $scope.model.rankid.val,
                text: "Once deleted, you will not be able to recover this event!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
           .then((willDelete) => {
               if (willDelete) {
                   $scope.removerank($scope.model.rankid.val);
               } else {
                   swal("ยกเลิกการลบ");
               }
           });
        };

        $scope.removerank = function (data) {
            $scope.showloading();
            $service.removerank($scope.url.RemoveRank, data).then(
                function (success) {
                    $scope.session(success.session);
                    if (success.session)
                        swal("ลบสำเร็จ", "ลบสำเร็จ", "success");
                    $scope.hideloading();
                },
                function (error) {
                    swal("เกิดข้อผิดพลาด", error, "error");
                    $scope.hideloading();
                }
                );
        };

        $scope.createranking = function () {
            $scope.showloading();
            $service.createranking($scope.url.CreateRankingID, $scope.model.ranktext.val).then(
                   function (success) {
                       $scope.session(success.session);
                       if (success.session)
                           swal("สร้าง Ranking Name เรียบร้อย", "", "success");
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
            createranking: function (url, data) {
                var deferred = $q.defer();
                $http.post(url, { DESC: data })
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
            removerank: function (url, data) {
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
            getError: function (url) {
                var deferref = $q.defer();
                $http.post(url)
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
            getRankcode: function (url, data) {
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
            getrankdetail: function (url, data, data1) {
                var deferref = $q.defer();
                $http.post(url, { RANK_CODE: data, RANKING_ID: data1 })
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