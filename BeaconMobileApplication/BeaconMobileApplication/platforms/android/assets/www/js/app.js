(function () {
    'use strict';
    var module = angular.module('app', ['onsen']);

    module.controller('AppController', function ($scope, $data) {

    });

    module.controller('DetailController', function ($scope, $data, $http) {
        //$scope.item = $data.selectedItem;

        $scope.doSomething = function () {
            var url = "http://beaconwebapplication.azurewebsites.net/api/message";
            var params = { uuid: $scope.beacon.uuid, major: $scope.beacon.major, minor: $scope.beacon.minor, proximity: $scope.beacon.proximity };
            var query = url + '?' + jQuery.param(params);
            console.log(query);

            $http.get(query).
            success(function (data, status, headers, config) {
                // this callback will be called asynchronously
                // when the response is available
                console.log(data);
                ons.notification.alert({ message: data.message });
            }).
            error(function (data, status, headers, config) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                console.log(data);
                ons.notification.alert({ message: data.message });
            });

            //$http.get(query).
            //  success(function (data) {
            //      var reply = data.message;
            //      ons.notification.alert({ message: reply });
            //  });
        };

        document.addEventListener('deviceready',
            function onDeviceReady() {
                // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
                // Specify a shortcut for the location manager holding the iBeacon functions.
                window.locationManager = cordova.plugins.locationManager;

                // Start tracking beacons!
                startScan();

                // Display refresh timer.
                updateTimer = setInterval(function () {

                    var timeNow = Date.now();

                    for (var key in beacons) {
                        var beacon = beacons[key];
                        if (beacon.timeStamp + 60000 > timeNow) {
                            // Map the RSSI value to a width in percent for the indicator.
                            var rssiWidth = 1; // Used when RSSI is zero or greater.
                            if (beacon.rssi < -100) { rssiWidth = 100; }
                            else if (beacon.rssi < 0) { rssiWidth = 100 + beacon.rssi; }

                            if ($data.selectedItem.uuid == beacon.uuid &&
                                $data.selectedItem.major == beacon.major &&
                                $data.selectedItem.minor == beacon.minor) {

                                $scope.beacon = beacon;
                                $scope.$apply();
                            }
                        }
                    }



                }, 100);
            }, false);
    });

    module.controller('MasterController', function ($scope, $data) {
        //$scope.items = $data.items;

        document.addEventListener('deviceready',
            function onDeviceReady() {
                // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
                // Specify a shortcut for the location manager holding the iBeacon functions.
                window.locationManager = cordova.plugins.locationManager;

                // Start tracking beacons!
                startScan();

                // Display refresh timer.
                updateTimer = setInterval(function () {

                    var timeNow = Date.now();
                    $scope.beacons = new Array();

                    for (var key in beacons) {
                        var beacon = beacons[key];
                        if (beacon.timeStamp + 60000 > timeNow) {
                            // Map the RSSI value to a width in percent for the indicator.
                            var rssiWidth = 1; // Used when RSSI is zero or greater.
                            if (beacon.rssi < -100) { rssiWidth = 100; }
                            else if (beacon.rssi < 0) { rssiWidth = 100 + beacon.rssi; }

                            $scope.beacons.push(beacon);

                        }
                    }

                    $scope.$apply();

                }, 500);
            }, false);

        $scope.showDetail = function (index) {
            var selectedItem = $scope.beacons[index];
            $data.selectedItem = selectedItem;
            $scope.navi.pushPage('detail.html', { uuid: selectedItem.uuid });
        };

    });

    module.factory('$data', function () {
        var data = {};

        data.items = [
            {
                title: 'Item 1 Title',
                label: '4h',
                desc: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.'
            },
            {
                title: 'Another Item Title',
                label: '6h',
                desc: 'Ut enim ad minim veniam.'
            },
            {
                title: 'Yet Another Item Title',
                label: '1day ago',
                desc: 'Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.'
            },
            {
                title: 'Yet Another Item Title',
                label: '1day ago',
                desc: 'Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.'
            }
        ];

        return data;
    });

    // Specify your beacon 128bit UUIDs here.
    var regions =
    [
        // Estimote Beacon factory UUID.
        { uuid: 'B9407F30-F5F8-466E-AFF9-25556B57FE6D' },
        // Sample UUIDs for beacons in our lab.
        { uuid: 'F7826DA6-4FA2-4E98-8024-BC5B71E0893E' },
        { uuid: '8DEEFBB9-F738-4297-8040-96668BB44281' },
        { uuid: 'A0B13730-3A9A-11E3-AA6E-0800200C9A66' },
        { uuid: 'E20A39F4-73F5-4BC4-A12F-17D1AD07A961' },
        { uuid: 'A4950001-C5B1-4B44-B512-1370F02D74DE' }
    ];

    // Dictionary of beacons.
    var beacons = {};

    // Timer that displays list of beacons.
    var updateTimer = null;

    function startScan() {
        // The delegate object holds the iBeacon callback functions
        // specified below.
        var delegate = new locationManager.Delegate();

        // Called continuously when ranging beacons.
        delegate.didRangeBeaconsInRegion = function (pluginResult) {
            //console.log('didRangeBeaconsInRegion: ' + JSON.stringify(pluginResult))
            for (var i in pluginResult.beacons) {
                // Insert beacon into table of found beacons.
                var beacon = pluginResult.beacons[i];
                beacon.timeStamp = Date.now();
                var key = beacon.uuid + ':' + beacon.major + ':' + beacon.minor;
                beacons[key] = beacon;
            }
        };

        // Called when starting to monitor a region.
        // (Not used in this example, included as a reference.)
        delegate.didStartMonitoringForRegion = function (pluginResult) {
            //console.log('didStartMonitoringForRegion:' + JSON.stringify(pluginResult))
        };

        // Called when monitoring and the state of a region changes.
        // (Not used in this example, included as a reference.)
        delegate.didDetermineStateForRegion = function (pluginResult) {
            //console.log('didDetermineStateForRegion: ' + JSON.stringify(pluginResult))
        };

        // Set the delegate object to use.
        locationManager.setDelegate(delegate);

        // Request permission from user to access location info.
        // This is needed on iOS 8.
        locationManager.requestAlwaysAuthorization();

        // Start monitoring and ranging beacons.
        for (var i in regions) {
            var beaconRegion = new locationManager.BeaconRegion(
                i + 1,
                regions[i].uuid);

            // Start ranging.
            locationManager.startRangingBeaconsInRegion(beaconRegion)
                .fail(console.error)
                .done();

            // Start monitoring.
            // (Not used in this example, included as a reference.)
            locationManager.startMonitoringForRegion(beaconRegion)
                .fail(console.error)
                .done();
        }
    }
})();