var apiRangeUrl = "http://localhost:2002/api/rest";
var ngApp = angular.module("Ang", []);

ngApp.controller("ListRecords", function ($scope, $http)
{
	$scope.Adding = false;
	$scope.FilterFirstName = null;
	$scope.FilterLastName = null;

	$scope.ListRecords = function ()
	{
		var param = "";

		if ($scope.FilterFirstName || $scope.FilterLastName)
			param = "?" + $.param({ firstName: $scope.FilterFirstName, lastName: $scope.FilterLastName });

		$http.get(apiRangeUrl + param).then(
		function (result)
		{
			if (result.data.Success)
				$scope.Records = result.data.Records;
			else
				alert(result.data.Message);
		},
		function (result)
		{
			console.log(result);
		});
	}

	$scope.SearchRecords = function ()
	{
		$scope.FilterFirstName = $("#txtSearchFirstName").val();
		$scope.FilterLastName = $("#txtSearchLastName").val();

		$scope.ListRecords();
	}

	$scope.FormClear = function ()
	{
		$scope.Form = { ID: "0", FirstName: "", LastName: "", Age: 1 }
	}

	$scope.AddRecord = function ()
	{
		event.preventDefault();

		$scope.FormClear();
		$scope.Adding = true;
	}

	$scope.UpdRecord = function (id)
	{
		event.preventDefault();

		$scope.Adding = true;
		$http.get(apiRangeUrl + "/" + id).then(function (result)
		{
			if (result.data.Success)
				$scope.Form = result.data.Record;
			else
				alert(result.data.Message);
		});
	}

	$scope.SaveRecord = function ()
	{
		event.preventDefault();

		$.post(apiRangeUrl, $scope.Form, function (result)
		{
			if (result.Success)
			{
				$scope.CancelRecord();

				if (($scope.Records != null) && ($scope.Records.length > 0))
				{
					for (var i = 0; i < $scope.Records.length; i++)
						if ($scope.Records[i].ID === result.Record.ID)
							break;

					$scope.Records[i] = result.Record;
				}
				else
					$scope.Records = [result.Record];

				$scope.$apply();
			}
			else
				alert(result.Message);
		});
	}

	$scope.DelRecord = function (id)
	{
		event.preventDefault();

		$http.delete(apiRangeUrl + "/" + id).then(function (result)
		{
			if (result.data.Success)
			{
				$scope.Records.forEach(function (element, index, array)
				{
					if (element.ID === id)
						$scope.Records.splice(index, 1);
				});

				if ($scope.Adding && ($scope.Form.ID == id))
					$scope.CancelRecord();
			}
			else
				alert(result.data.Message);
		},
		function (result)
		{
			console.log(result);
		});
	}

	$scope.CancelRecord = function ()
	{
		$scope.FormClear();
		$scope.Adding = false;
	}

	$scope.FormClear();
	$scope.ListRecords();
});