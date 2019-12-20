angular.module('UserModule', ['angularUtils.directives.dirPagination', 'ui.bootstrap'])
    .controller('UserController', ['$scope', '$filter', '$service', '$timeout', '$window', '$http', '$modal', '$log', function ($scope, $filter, $service, $timeout, $window, $http, $modal, $log) {
        $scope.url = {};

        $scope.master = {
            role: {},
            datauser: {},
            gender: {}
        };

        $scope.model = {
           
            datauser: {

                ROLE_ID: {
                    val: "1" ,
                    series: [],
                    fix: false,
                    clear: function () { if (!$scope.model.datauser.ROLE_ID.fix) { $scope.model.datauser.ROLE_ID.val = ''; $scope.model.datauser.ROLE_ID = []; } }
                },

                USER_ID: {
                    val: '',
                    series: [],
                    fix: false,
                    clear: function () { if (!$scope.model.datauser.USER_ID.fix) { $scope.model.datauser.USER_ID.val = ''; $scope.model.datauser.USER_ID.series = []; } }
                },

                EMAIL: {
                    val: '',
                    series: [],
                    fix: false,
                    clear: function () { if (!$scope.model.datauser.EMAIL.fix) { $scope.model.datauser.EMAIL.val = ''; $scope.model.datauser.EMAIL.series = []; } }
                },

                USER_NAME: {
                    val: '',
                    series: [],
                    fix: false,
                    clear: function () { if (!$scope.model.datauser.USER_NAME.fix) { $scope.model.datauser.USER_NAME.val = ''; $scope.model.datauser.USER_NAME.series = []; } }
                },

                USER_SURNAME: {
                    val: '',
                    series: [],
                    fix: false,
                    clear: function () { if (!$scope.model.datauser.USER_SURNAME.fix) { $scope.model.datauser.USER_SURNAME.val = ''; $scope.model.datauser.USER_SURNAME.series = []; } }
                },

                PASSWORD: {
                    val: '',
                    series: [],
                    fix: false,
                    clear: function () { if (!$scope.model.datauser.PASSWORD.fix) { $scope.model.datauser.PASSWORD.val = ''; $scope.model.datauser.PASSWORD.series = []; } }
                }
            },

            dataperpage: "10",

            dataperpagelist: ["10", "20", "50", "100", "200"],
        };

        $scope.dataperpageChang = function () {
            $scope.pagination.size = $scope.model.dataperpage;
        };

        $scope.pageChanged = function (newPage) {
            console.log(newPage);
            $scope.pagination.current = newPage;
            $scope.pagination.begin = newPage == 1 ? 0 : ($scope.pagination.size * (newPage - 1));
        };

        $scope.pagination = {
            current: 1,
            begin: 0,
            size: $scope.model.dataperpage,
            maxSize: 10
        };

        $scope.initial = function () {
            $scope.showloading();
            $scope.geturl();
            $scope.hideloading();
        };

        $scope.geturl = function () {

            if ($('#hdGetUrl').val() != undefined && $('#hdGetUrl').val() != '') {
                $http.post($('#hdGetUrl').val())
                   .success(function (obj) {
                       $scope.url = obj;
                       if (obj != undefined && obj != '') {
                           $scope.getrole();
                           $scope.getgender();
                       }
                   });
            }
        };

        $scope.getrole = function () {
            $scope.showloading();
            $service.getrole($scope.url.GetRole).then(
                  function (success) {
                      $scope.session(success.session);
                      $scope.master.role = success.data;
                      $scope.hideloading();
                  }, function (error) {
                      if (error !== null)
                          swal('error', error, 'error');
                      $scope.hideloading();
                  });
        };

        $scope.getgender = function () {
            $scope.showloading();
            $service.getgender($scope.url.GetGender).then(
                  function (success) {
                      $scope.session(success.session);
                      $scope.master.gender = success.data;
                      $scope.hideloading();
                  }, function (error) {
                      if (error !== null)
                          swal('error', error, 'error');
                      $scope.hideloading();
                  });
        };

        $scope.role_change = function () {

        };

        $scope.search = function () {
            $scope.showloading();
            $service.search($scope.url.GET_USER_SEARCH,
                $scope.model.datauser.ROLE_ID.val,
                 $scope.model.datauser.EMAIL.val,
                  $scope.model.datauser.USER_NAME.val,
                  $scope.model.datauser.USER_SURNAME.val,
                  $scope.model.datauser.USER_ID.val).then(
                 function (success) {

                     $scope.session(success.session);
                     if (success.data.length == 0)
                         swal('infoinfo', "ไม่พบข้อมูล", 'info');
                     $scope.master.datauser = success.data;
                     $scope.hideloading();
                 }, function (error) {
                     if (error !== null)
                         swal('error', error, 'error');
                     $scope.hideloading();
                 });

        };

        $scope.adduser = function () {
            $scope.showloading();
            $service.adduser($scope.url.ADD_USER_ADMIN,
               $scope.model.datauser.ROLE_ID.val,
                $scope.model.datauser.EMAIL.val,
                 $scope.model.datauser.USER_NAME.val,
                 $scope.model.datauser.USER_SURNAME.val,
                 $scope.model.datauser.USER_ID.val,
                 $scope.model.datauser.PASSWORD.val).then(
                function (success) {
                    swal('success', "เพิ่มข้อมูลเรียบร้อย", 'success');
                    $scope.search();
                    $scope.hideloading();
                }, function (error) {
                    if (error !== null)
                        swal('error', error, 'error');
                    $scope.hideloading();
                });
        };

        $scope.removeConfirm = function (data) {
            swal({
                title: "ยืนยันการลบ User ID : " + data.USER_ID,
                text: "Once deleted, you will not be able to recover this event!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
           .then((willDelete) => {
               if (willDelete) {
                   $scope.remove(data);
               } else {
                   swal("ยกเลิกการลบ");
               }
           });
        };

        $scope.remove = function (data) {
            $service.remove($scope.url.RemoveUserAdmin,
             data).then(
              function (success) {
                  $scope.session(success.session);
                  if (success.session)
                      swal("ลบสำเร็จ", "ลบสำเร็จ", "success");
                  $scope.search();
              }, function (error) {
                  if (error !== null)
                      swal('error', error, 'error');
              });
        };

        $scope.clear = function () {
            $scope.model.datauser.USER_ID.clear();
            $scope.model.datauser.EMAIL.clear();
            $scope.model.datauser.USER_NAME.clear();
            $scope.model.datauser.USER_SURNAME.clear();
            $scope.model.datauser.PASSWORD.clear();
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

        $scope.manager = function (data) {
            window.location = $scope.url.managerRe +'/'+data;
        };

        $scope.showloading = function () {
            $('.divLoading').show();
        };

        $scope.hideloading = function () {
            $('.divLoading').hide();
        };

        $scope.edit = function (data) {
            $scope.showloading();
            $service.updateuser($scope.url.UpdateUser, data).then(
                  function (success) {
                      $scope.session(success.session);
                      swal("แก้ไขข้อมูลสำเร็จ", "แก้ไขข้อมูลสำเร็จ", "success");
                      $scope.search();
                      $scope.hideloading();
                  }, function (error) {
                      if (error !== null)
                          swal('error', error, 'error');
                      $scope.hideloading();
                  });
        };

        $scope.showForm = function (data) {
            $scope.userForm = data;
            var modalInstance = $modal.open({
                templateUrl: 'UserManagerEdit',
                controller: ModalInstanceCtrl,
                scope: $scope,
                resolve: {
                    userForm: function () { return $scope.userForm; }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                $scope.edit(selectedItem);
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
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
             getrole: function (url) {
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

             getgender: function (url) {
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

             search: function (url, ROLE_ID, EMAIL, USER_NAME, USER_SURNAME, USER_ID) {
                 var deferred = $q.defer();
                 $http.post(url, { ROLE_ID: ROLE_ID, EMAIL: EMAIL, USER_NAME: USER_NAME, USER_SURNAME: USER_SURNAME, USER_ID: USER_ID })
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

             adduser: function (url, ROLE_ID, EMAIL, USER_NAME, USER_SURNAME, USER_ID, PASSWORD) {
                 var deferred = $q.defer();
                 $http.post(url, { ROLE_ID: ROLE_ID, EMAIL: EMAIL, USER_NAME: USER_NAME, USER_SURNAME: USER_SURNAME, USER_ID: USER_ID, PASSWORD: PASSWORD })
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

             remove: function (url, data) {
                 var deferred = $q.defer();
                 $http.post(url, { data })
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

             updateuser: function (url, data) {
                 var deferred = $q.defer();
                 $http.post(url, { data })
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
         };
     })

var ModalInstanceCtrl = function ($scope, $modalInstance, userForm) {

    $scope.item = userForm;

    $scope.gen = $scope.$parent.master.gender;

    $scope.save = function () {

        $scope.item.BIRTH_DATE_STR = $("#datetimepicker").val();
        console.log($scope.item.BIRTH_DATE_STR);
        $modalInstance.close($scope.item);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

};
