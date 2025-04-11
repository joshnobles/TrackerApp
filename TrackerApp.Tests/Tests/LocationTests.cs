using TrackerApp.Core.Services.Static;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Tests.Tests
{
    [TestClass]
    public sealed class LocationTests
    {
        private static AddLocationViewModel[] GetBadAddLocationViewModels()
        {
            return
            [
                new AddLocationViewModel
                {
                    Latitude = -100,
                    Longitude = 0,
                    Altitude = 100,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 200,
                    Altitude = 100,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = -1000,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = 0.05
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 1000000,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = -90000,
                    Altitude = 100,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = -45000,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = -150
                },
                new AddLocationViewModel
                {
                    Latitude = 200,
                    Longitude = -300,
                    Altitude = 100,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = -999,
                    Longitude = 999,
                    Altitude = -500,
                    Confidence = 0
                }
            ];
        } 

        private static AddLocationViewModel[] GetGoodAddLocationViewModels()
        {
            return
            [
                new AddLocationViewModel
                {
                    Latitude = 0,
                    Longitude = 0,
                    Altitude = 0,
                    Confidence = 0.1
                },
                new AddLocationViewModel
                {
                    Latitude = 45.5,
                    Longitude = -75.3,
                    Altitude = 300,
                    Confidence = 80.5
                },
                new AddLocationViewModel
                {
                    Latitude = 89.99,
                    Longitude = 179.99,
                    Altitude = 29999.99,
                    Confidence = 99.99
                },
                new AddLocationViewModel
                {
                    Latitude = -89.99,
                    Longitude = -179.99,
                    Altitude = -419.99,
                    Confidence = 10
                },
                new AddLocationViewModel
                {
                    Latitude = 12.3456,
                    Longitude = -45.6789,
                    Altitude = 123.45,
                    Confidence = 25.5
                },
                new AddLocationViewModel
                {
                    Latitude = 1.23,
                    Longitude = 2.34,
                    Altitude = 567.89,
                    Confidence = 50
                },
                new AddLocationViewModel
                {
                    Latitude = 10,
                    Longitude = 20,
                    Altitude = 1000,
                    Confidence = 33.3
                },
                new AddLocationViewModel
                {
                    Latitude = -45,
                    Longitude = 90,
                    Altitude = 250,
                    Confidence = 90
                },
                new AddLocationViewModel
                {
                    Latitude = 5,
                    Longitude = 15,
                    Altitude = 500,
                    Confidence = 75
                },
                new AddLocationViewModel
                {
                    Latitude = 32.7767,
                    Longitude = -96.7970,
                    Altitude = 430,
                    Confidence = 60.4
                }
            ];
        }

        [TestMethod]
        public void TestAddLocationViewModel_BadData()
        {
            var invalidViewModels = GetBadAddLocationViewModels();

            ViewModelValidationResult result;

            foreach (var viewModel in invalidViewModels)
            {
                result = Valid.ViewModel(viewModel);

                Assert.IsFalse(result.IsValid);
                Assert.IsTrue(result.ErrorResults.Count > 0);
            }
        }

        [TestMethod]
        public void TestAddLocationViewModel_GoodData()
        {
            var viewModels = GetGoodAddLocationViewModels();

            ViewModelValidationResult result;

            foreach (var viewModel in viewModels)
            {
                result = Valid.ViewModel(viewModel);

                Assert.IsTrue(result.IsValid);
                Assert.IsTrue(result.ErrorResults.Count == 0);
            }
        }
    }
}
