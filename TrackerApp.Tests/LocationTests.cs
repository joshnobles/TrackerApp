using TrackerApp.Core.Services.Static;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Tests
{
    [TestClass]
    public sealed class LocationTests
    {
        private AddLocationViewModel[] GetBadAddLocationViewModels()
        {
            return 
            [
                new AddLocationViewModel
                {
                    Latitude = -100,
                    Longitude = 0,
                    Altitude = 100,
                    Confidence = 10,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlrDuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 200,
                    Altitude = 100,
                    Confidence = 10,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlrDuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = -1000,
                    Confidence = 10,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlrDuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = 0.05,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlrDuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = 10,
                    RequestVerificationSecret = "short"
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = 10,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlr*DuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = 10,
                    RequestVerificationSecret = null!
                },
                new AddLocationViewModel
                {
                    Latitude = 45,
                    Longitude = 90,
                    Altitude = 100,
                    Confidence = 150,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlrDuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = 200,
                    Longitude = -300,
                    Altitude = 100,
                    Confidence = 10,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlrDuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = -999,
                    Longitude = 999,
                    Altitude = -500,
                    Confidence = 0,
                    RequestVerificationSecret = "invalid"
                }
            ];
        } 

        private AddLocationViewModel[] GetGoodAddLocationViewModels()
        {
            return
            [
                new AddLocationViewModel
                {
                    Latitude = 0,
                    Longitude = 0,
                    Altitude = 0,
                    Confidence = 0.1,
                    RequestVerificationSecret = "Cg/h88o9FkHIpBlrDuoQPVkfNvZv1Erd6KuCcx+GKVQ="
                },
                new AddLocationViewModel
                {
                    Latitude = 45.5,
                    Longitude = -75.3,
                    Altitude = 300,
                    Confidence = 80.5,
                    RequestVerificationSecret = "AbCdEfGhIjKlMnOpQrStUvWxYzsuth1234567890+/=="
                },
                new AddLocationViewModel
                {
                    Latitude = 89.99,
                    Longitude = 179.99,
                    Altitude = 29999.99,
                    Confidence = 99.99,
                    RequestVerificationSecret = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                },
                new AddLocationViewModel
                {
                    Latitude = -89.99,
                    Longitude = -179.99,
                    Altitude = -419.99,
                    Confidence = 10,
                    RequestVerificationSecret = "ZGVmYXVsdF9zZWNyZXQxMjM0NTY3ODkwMTIzNDU2Nzg5"
                },
                new AddLocationViewModel
                {
                    Latitude = 12.3456,
                    Longitude = -45.6789,
                    Altitude = 123.45,
                    Confidence = 25.5,
                    RequestVerificationSecret = "QWxwaGFOdW1lcmljU2VjcmV0QmFzZTY0MTIzNDU2Nzg="
                },
                new AddLocationViewModel
                {
                    Latitude = 1.23,
                    Longitude = 2.34,
                    Altitude = 567.89,
                    Confidence = 50,
                    RequestVerificationSecret = "MTIzNDU2Nzg5MDEyMzQ1Njc4OTAxMjM0NTY3ODkwMTI="
                },
                new AddLocationViewModel
                {
                    Latitude = 10,
                    Longitude = 20,
                    Altitude = 1000,
                    Confidence = 33.3,
                    RequestVerificationSecret = "U29tZVZhbGlkQmFzZTY0U3RyaW5nQ2hlY2tJdA==1234"
                },
                new AddLocationViewModel
                {
                    Latitude = -45,
                    Longitude = 90,
                    Altitude = 250,
                    Confidence = 90,
                    RequestVerificationSecret = "U3RyaWN0QmFzZTY0VmFsaWRSZXF1ZXN0U2VjcmV0MTI="
                },
                new AddLocationViewModel
                {
                    Latitude = 5,
                    Longitude = 15,
                    Altitude = 500,
                    Confidence = 75,
                    RequestVerificationSecret = "NzY1NDMyMTAxQmFzZTY0VmFsaWRhdGlvblRva2VuQmE="
                },
                new AddLocationViewModel
                {
                    Latitude = 32.7767,
                    Longitude = -96.7970,
                    Altitude = 430,
                    Confidence = 60.4,
                    RequestVerificationSecret = "QmFzZTY0VmFsaWRQYXNzU3RyaW5nQ2hlY2s9PQ==aabb"
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
