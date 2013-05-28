/// <reference path="../_references.js" />

window.payMeBack.app.controller(window.payMeBack.core.dependencies.summaryActionLinkController,
    function ($scope) {

        // For the add debt link
        $scope.showAddDebtDialog = function () {
            $scope.addDebtModal = true;
        };

        $scope.showManageProfileDialog = function () {
            $scope.acctSettingsModal = true;
        };

        $scope.closeAddDebtModal = function () {
            $scope.addDebtModal = false;
        }

        //$scope.$on('debtSummaryListChanged', function () {
        //    $scope.addDebtModal = false;
        //});

        $scope.closeAcctSettingsModal = function () {
            $scope.acctSettingsModal = false;
        }

        $scope.$on("closeAllDialogs", function () {
            $scope.acctSettingsModal = false;
            $scope.addDebtModal = false;
        });


        $scope.opts = {
            backdropFade: true,
            dialogFade: true
        };

    }).$inject = ['$scope'];
