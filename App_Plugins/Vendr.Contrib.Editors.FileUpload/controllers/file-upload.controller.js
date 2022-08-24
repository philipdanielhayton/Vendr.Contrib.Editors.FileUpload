(function () {
  "use strict";

  function vendrFileUploadController(
    $scope,
    $http,
    Upload,
    notificationsService,
    overlayService
  ) {
    var vm = this;

    vm.value = JSON.parse($scope.model.value || "[]");
    vm.storeId = $scope.model.config.storeId;
    vm.orderId = $scope.model.config.orderId;
    vm.alias = $scope.model.alias;
    vm.files = [];
    vm.buttonState = "init";
    vm.defaultButtonText = "Choose files";
    vm.buttonText = vm.defaultButtonText;
    vm.endpoint = "/umbraco/backoffice/api/fileupload/";

    vm.handleFiles = handleFiles;
    vm.upload = upload;
    vm.deleteFile = confirmDelete;

    vm.filetypes = "image/png, image/jpeg, application/pdf";

    function handleFiles(files) {
      if (files && files.length > 0) {
        vm.files = files;
        vm.buttonText = vm.files.length + " files selected";
      } else {
        vm.buttonText = vm.defaultButtonText;
      }
    }

    function upload(files) {
      vm.buttonState = "busy";

      Upload.upload({
        url: vm.endpoint + "upload",
        fields: {
          storeId: vm.storeId,
          orderId: vm.orderId,
          alias: vm.alias,
          files,
        },
        files,
      })
        .success(function (data, status, headers, config) {
          vm.value = data;
          $scope.model.value = JSON.stringify(data);
          vm.files = [];
          vm.buttonText = vm.defaultButtonText;
          vm.buttonState = "success";
          notificationsService.success(
            "Files uploaded",
            "Don't forget to save the order!"
          );
        })
        .error(function (data, status, headers, config) {
          vm.buttonState = "error";
          notificationsService.error("Upload failed", data);
        });
    }

    function confirmDelete(file) {
      overlayService.confirmDelete({
        title: "Are you sure?",
        submit: function () {
          deleteFile(file);
          overlayService.close();
        },
      });
    }

    function deleteFile(file) {
      $http
        .post(vm.endpoint + "delete", {
          storeId: vm.storeId,
          orderId: vm.orderId,
          alias: vm.alias,
          fileName: file.name,
        })
        .then(
          function (response) {
            vm.value = response.data;
            $scope.model.value = JSON.stringify(response.data);
            notificationsService.success("File deleted");
          },
          function (response) {
            notificationsService.error("Could not delete file");
          }
        );
    }
  }

  angular
    .module("umbraco")
    .controller(
      "Vendr.Contrib.Editors.FileUploadController",
      vendrFileUploadController
    );
})();
