angular.module('eventModule', [])
    .controller('eventController', ['$scope', '$filter', '$service', '$timeout', '$window', '$http', function ($scope, $filter, $service, $timeout, $window, $http) {
        $scope.url = {};

        $scope.master = {
            tournament: {},
            category: {},
            gender: {},
            eventdetail: [],
            rank: {}
        };

        $scope.model = {
            tournament: {
                val: '',
                desc:'',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.tournament.fix) { $scope.model.tournament.val = ''; $scope.model.tournament = []; } }
            },

            category: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.category.fix) { $scope.model.category.val = ''; $scope.model.category = []; } }
            },

            gender: {
                val: '',
                fix: false,
                clear: function () { if (!$scope.model.gender.fix) { $scope.model.gender.val = ''; $scope.model.gender = []; } }
            },

            eventdetail: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.eventdetail.fix) { $scope.model.eventdetail.val = ''; $scope.model.eventdetail.series = []; } }
            },

            numofply: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.numofply.fix) { $scope.model.numofply.val = ''; $scope.model.numofply = []; } }
            },

            rank: {
                val: '',
                series: [],
                fix: false,
                clear: function () { if (!$scope.model.rank.fix) { $scope.model.rank.val = ''; $scope.model.rank = []; } }
            }
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
                           $scope.gettournamentcode();
                           $scope.getevent();
                           
                       }
                   });
            }

        };

        $scope.getftour = function () {
            $service.getftour($scope.url.GetFirstTour).then(
                  function (success) {
                      if (success.data != null) {
                          $scope.model.tournament.val = success.data;
                          $scope.showtournament();
                      }                     
                      $scope.hideloading();
                  }, function (error) {
                      if (error !== null)
                          swal('error', error, 'error');
                      $scope.hideloading();
                  });
        };

        $scope.catechange = function () {
            var item = $scope.master.category.find(function (v) { return v.ID == $scope.model.category.val; })
            $scope.model.numofply.val = item.NUM_OF_PLY;
            if ($scope.model.tournament.val != '') {
                $scope.getevent();
            }
        };

        $scope.gettournamentcode = function () {
            $scope.showloading();
            $service.gettournamentcode($scope.url.GetTournamentCode).then(
                  function (success) {

                      $scope.master.category = success.data[0];
                      $scope.model.numofply.val = 1;
                      $scope.master.tournament = success.data[1];
                     // $scope.getftour();
                      $scope.hideloading();
                  }, function (error) {
                      if (error !== null)
                          swal('error', error, 'error');
                      $scope.hideloading();
                  });
        };

        $scope.getevent = function () {
            $scope.showloading();
            $service.getevent($scope.url.GetEvent, $scope.model.tournament.val, $scope.model.category.val).then(
                  function (success) {
                      $scope.session(success.session);
                      $scope.master.eventdetail = success.data[0];
                      $scope.master.gender = success.data[1];
                      $scope.master.rank = success.data[3];
                      if (success.data[2] != null)
                          $scope.model.numofply.val = success.data[2].NUM_OF_PLY;
                      else {
                          if ($scope.model.category.val == '')
                              var item = $scope.master.category.find(function (v) { return v.ID == 1; })
                          else
                              var item = $scope.master.category.find(function (v) { return v.ID == $scope.model.category.val; })
                          $scope.model.numofply.val = item.NUM_OF_PLY;
                      }
                      $scope.hideloading();
                      // $scope.toAnchor('bodycontent');
                  }, function (error) {
                      if (error !== null)
                          swal('error', error, 'error');
                      $scope.hideloading();
                  });
        };

        $scope.showtournament = function () {
            var item = $scope.master.tournament.find(function (v) { return v.ID == $scope.model.tournament.val; });
            $scope.model.tournament.series = item;
            if ($scope.model.tournament.val) {
                $scope.model.tournament.fix = true;             
            }
            $scope.getevent();
        };

        $scope.addNew = function (eventdetail) {
            $scope.master.eventdetail.push({
                'ID': "",
                'EVENT_CODE': "",
                'TH_EVENT_NAME': "",
                'TOUR_SELECT': "",
                'NAME_CODE': "",
                'PAY': ""
            });
        };

        $scope.removeEventConfirm = function (idEvent) {
            swal({
                title: "ยืนยันการลบข้อมูล?",
                text: "Once deleted, you will not be able to recover this event!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
            .then((willDelete) => {
                if (willDelete) {
                    $scope.removeEvent(idEvent);
                } else {
                    swal("ยกเลิกการลบ");
                }
            });
        };
      

        $scope.removeEvent = function (idEvent) {
            $service.removevent($scope.url.RemoveEvent, idEvent).then(
               function (success) {
                   $scope.session(success.session);
                   if (success.Successed) {
                       swal("Delete Success", {
                           icon: "success",
                       });
                       $scope.getevent();
                   }
               }, function (error) {
                   if (error !== null)
                       swal('error', error, 'error');
               });
        };


        $scope.editTour = function (url) {
            $window.open(url, '_blank');
        };
    
        $scope.checkAll = function () {

            $scope.selectedAll = !$scope.selectedAll;
            angular.forEach($scope.master.eventdetail, function (eventdetail) {
                eventdetail.TOUR_SELECT = $scope.selectedAll;
            });
        };

        $scope.save = function () {
            var a = $scope.master.eventdetail;
            if (a) {
                $service.save($scope.url.AddEvent, $scope.model.tournament.val, a, $scope.model.category.val, $scope.model.numofply.val).then(
                function (success) {
                    $scope.getevent();
                    if (success.session)
                        swal("บันทึกเรียบร้อย", "บันทึกสำเร็จ", "success");
                }, function (error) {
                    if (error !== null)
                        swal('เกิดข้อผิดพลาด', error, 'error');
                });


            }

        };

        $scope.toAnchor = function (elementID) {
            var _element = $("#" + elementID);
            $('html,body').animate({ scrollTop: _element.offset().top }, 'slow');
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

             getevent: function (url, data, cate) {
                 var deferred = $q.defer();
                 $http.post(url, {
                     ID: data, category: cate
                 })
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

             removevent: function (url, data) {
                 var deferred = $q.defer();
                 $http.post(url, {
                     ID: data
                 })
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

             getftour: function (url) {
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

             save: function (url, ID, data, cate, num) {
                 var deferred = $q.defer();
                 $http.post(url, {
                     TOUR_ID: ID, dataObjects: data, category: cate, num: num
                 })
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
             }


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