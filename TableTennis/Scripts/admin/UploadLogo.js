var app = angular.module("app", []);
app.controller("fileUploadCtrl", ['$scope', '$service', '$http', '$window', function ($scope, $service, $http, $window) {
    $scope.datafile = [];
    $scope.url = {};

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
                       $scope.getlogo();
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

    $scope.removelogo = function (data) {
        $service.removelogo($scope.url.RemoveLogo, data).then(
            function (success) {
                $scope.session(success.session);
                swal("ลบสำเร็จ", "ลบสำเร็จ", "success");
                var index = $scope.datafile.indexOf(data);
                $scope.datafile.splice(index, 1);
                $scope.hideloading();
            },
                  function (error) {
                      swal("เกิดข้อผิดพลาด", error, "error");
                      $scope.hideloading();
                  }
            );
    };

    $scope.getlogo = function () {
        $service.getlogo($scope.url.Logo_Edit).then(
               function (success) {
                   $scope.session(success.session);
                   $scope.datafile = success.data;
                   $scope.hideloading();

               },
                  function (error) {
                      swal("เกิดข้อผิดพลาด", error, "error");
                      $scope.hideloading();
                  }
               );
    };

}]);
app.service('$service', function ($http, $q) {
    if (!window.location.origin)
        window.location.origin = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    var root = window.location.origin + "//" + "TableTennis" + "//";
    showerror = function (html_errorpage) {
        if (root.indexOf("localhost") > -1)
            window.open("", "_blank").document.write(html_errorpage);
        else
            console.log(" some error founded... (see on localhost or admin policy) ");
    };

    return {
        getlogo: function (url) {
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
        removelogo: function (url, data) {
            var deferred = $q.defer();
            $http.post(url, { data: data })
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
});
app.directive("imgUpload", function ($http, $compile, $q) {
    return {
        restrict: 'AE',
        scope: {
            url: "@",
            method: "@"
        },
        template: '<div  class"row mainbox" >' +
            '<div class="col-xs-12 col-sm-12 col-md-6 col-sm-offset-3 col-md-offset-3 wapper radius subbox">' +
            '<h2 class="col-all-center"> เพิ่ม LOGO </h2>' +
            ' <hr class="colorgraph">' +
            '<input class="fileUpload"  type="file" multiple />' +
                    '<div class="dropzone">' +
                        '<p class="msg">Click or Drag and Drop files to upload</p>' +
                   '</div>' +
                   '<div class="row preview clearfix">' +
                        '<div class="previewData clearfix" ng-repeat="data in previewData track by $index">' +
                            '<img ng-src={{data.src}}></img>' +
                            '<div class="previewDetails">' +
                                '<div class="detail"><b>Name : </b>{{data.name}}</div>' +
                                '<div class="detail"><b>Type : </b>{{data.type}}</div>' +
                                '<div class="detail"><b>Size : </b> {{data.size}}</div>' +
                            '</div>' +
                            '<div class="previewControls">' +
                                '<span ng-click="remove(data)" class="circle remove">' +
                                    '<i class="fa fa-close"></i>' +
                                '</span>' +
                            '</div>' +
                        '</div>' +
                        ' <hr class="colorgraph">' +
                        '  <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 detailbox col-all-mtop col-all-center" ><button type="button" class="btn btn-primary col-xxs-mtop" ng-click="upload()"> <i class="fa fa-upload"></i> อัพโหลด </button>' +
                   '</div>' +
                   ' </div>' +
                   '</div>' +
        '</div>',
        link: function (scope, elem, attrs) {
            var formData = new FormData();
            scope.previewData = [];
            scope.url = [];

            //ตัวสำหรับแสดงไฟล์
            function previewFile(file) {
                var reader = new FileReader();
                var obj = new FormData().append('File', file);
                reader.onload = function (data) {
                    var src = data.target.result;
                    var size = ((file.size / (1024 * 1024)) > 1) ? (file.size / (1024 * 1024)) + ' mB' : (file.size / 1024) + ' kB';
                    scope.$apply(function () {
                        scope.previewData.push({
                            'name': file.name, 'size': size, 'type': file.type,
                            'src': src, 'data': obj
                        });
                    });
                    //console.log(scope.previewData);
                }
                reader.readAsDataURL(file);
            }

            //สำหรับอัพโหลด
            function uploadFile(e, type) {
                e.preventDefault();
                var files = "";
                if (type == "formControl") {
                    files = e.target.files;
                } else if (type === "drop") {
                    files = e.originalEvent.dataTransfer.files;
                }
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    if (file.type.indexOf("image") !== -1) {
                        if (file.size < 999999) {
                            previewFile(file);
                        } else {
                            alert(file.name + " ขนาดไฟล์ต้องไม่เกิน 1 MB ");
                        }

                    } else {
                        alert(file.name + " is not supported");
                    }

                }
            }

            //กรณีมีการเปลี่ยนแปลงที่ตัว formControl
            elem.find('.fileUpload').bind('change', function (e) {
                uploadFile(e, 'formControl');
            });
            //กรณีมีการ click  formControl
            elem.find('.dropzone').bind("click", function (e) {
                $compile(elem.find('.fileUpload'))(scope).trigger('click');
            });

            //กรณีมีการ dragover  formControl
            elem.find('.dropzone').bind("dragover", function (e) {
                e.preventDefault();
            });

            //กรณีมีการ drop  formControl
            elem.find('.dropzone').bind("drop", function (e) {
                uploadFile(e, 'drop');
            });
            //กรณีกดปุ่มอัพโหลดระบบจะมาที่นี่
            scope.upload = function () {
                //$http({
                //    method: scope.method, url: scope.url, data: data,
                //    headers: { 'Content-Type': undefined }, transformRequest: angular.identity
                //}).success(function (data) {

                //});
                //'<span ng-click="upload(data)" class="circle upload">' +
                //                    '<i class="fa fa-check"></i>' +
                //                '</span>' +

                var deferred = $q.defer();
                $http.post(scope.url, {
                    data: scope.previewData
                })
                .success(function (response) {
                    if (response.Successed) {
                        deferred.resolve(response);
                        swal("อัพโหลดสำเร็จ", "อัพโหลดสำเร็จ", "success");
                    }
                    else {
                        swal("เกิดข้อผิดพลาด", response.ErrorMassage, "error");
                        deferred.reject(response.ErrorMassage);
                    }
                })
                 .error(function (response) {
                     showerror(response.ErrorMassage);
                     deferred.reject(response.ErrorMassage);
                 })
                return deferred.promise;
            }

            if (!window.location.origin)
                window.location.origin = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
            var root = window.location.origin + "//" + "TableTennis" + "//";
            //กรณีกดปุ่มลบจะเรียกใช้ตัวนี้
            scope.remove = function (data) {
                var index = scope.previewData.indexOf(data);
                scope.previewData.splice(index, 1);
                var deferred = $q.defer();
                $http.post(root + "Admin//RemoveLogo", {
                    data: data
                })
                .success(function (response) {
                    if (response.Successed) {
                        deferred.resolve(response);
                        swal("ลบสำเร็จ", "ลบสำเร็จ", "success");
                    }
                    else {
                        swal("เกิดข้อผิดพลาด", response.ErrorMassage, "error");
                        deferred.reject(response.ErrorMassage);
                    }
                })
                 .error(function (response) {
                     showerror(response.ErrorMassage);
                     deferred.reject(response.ErrorMassage);
                 })
                return deferred.promise;
            }

        }
    }
});