using TrackerApp.Core.Services.Static;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Tests.Tests
{
    [TestClass]
    public sealed class UserTests
    {
        private static AddUserViewModel[] GetBadAddUserViewModels()
        {
            return
            [
                new AddUserViewModel 
                {
                    UserID = "ab"
                },
                new AddUserViewModel 
                {
                    UserID = "abc$123"
                },
                new AddUserViewModel 
                {
                    UserID = "ValidUser123",
                    Name = "Al"
                },
                new AddUserViewModel 
                {
                    UserID = "ValidUser123",
                    Name = "John123"
                },
                new AddUserViewModel 
                {
                    UserID = "ValidUser123",
                    Name = "Mary#Jane"
                },
                new AddUserViewModel 
                {
                    UserID = "ValidUser123",
                    Email = "averyveryverylongemailaddressmorethan50characters@example.com"
                },
                new AddUserViewModel 
                {
                    UserID = "ValidUser123",
                    Email = "notanemail"
                },
                new AddUserViewModel 
                {
                    UserID = "ValidUser123",
                    ProfileImageSrc = "http://example.com/image.jpg"
                },
                new AddUserViewModel 
                {
                    UserID = "ValidUser123",
                    ProfileImageSrc = "https://" + new string('a', 245) + ".com"
                },
                new AddUserViewModel 
                {
                    UserID = null!
                }
            ];
        }

        private static AddUserViewModel[] GetGoodAddUserViewModels()
        {
            return
            [
                new AddUserViewModel
                {
                    UserID = "User123",
                    Name = "Alice Johnson",
                    Email = "alice@example.com",
                    ProfileImageSrc = "https://example.com/image1.jpg"
                },
                new AddUserViewModel
                {
                    UserID = "ValidUserOnly"
                },
                new AddUserViewModel
                {
                    UserID = "User456",
                    Name = "O'Connor Smith"
                },
                new AddUserViewModel
                {
                    UserID = "XYZ789",
                    Name = "Joe",
                    Email = "joe@mail.com"
                },
                new AddUserViewModel
                {
                    UserID = "ProfilePicGuy",
                    ProfileImageSrc = "https://cdn.example.com/images/users/pic123.jpg"
                },
                new AddUserViewModel
                {
                    UserID = "john.doe-1987",
                    Name = "John Doe",
                    Email = "john.doe87@example.com",
                    ProfileImageSrc = "https://images.example.com/john.jpg"
                },
                new AddUserViewModel
                {
                    UserID = "ADMIN|USER",
                    Name = "Sam Admin",
                    Email = "sam.admin@example.org"
                },
                new AddUserViewModel
                {
                    UserID = "ABC",
                    Name = "Tim",
                    Email = "tim@t.co"
                },
                new AddUserViewModel
                {
                    UserID = new string('A', 250),
                    Name = new string('B', 50),
                    Email = "max.length.email@longdomain.com",
                    ProfileImageSrc = "https://" + new string('x', 237) + ".com"
                },
                new AddUserViewModel
                {
                    UserID = "user-hybrid",
                    Name = "Jean-Luc Picard",
                    Email = "captain@enterprise.org",
                    ProfileImageSrc = "https://startrek.example.com/jeanluc.png"
                }
            ];
        }

        [TestMethod]
        public void TestAddUserViewModel_BadData()
        {
            var viewModels = GetBadAddUserViewModels();

            ViewModelValidationResult validationResult;

            foreach (var viewModel in viewModels)
            {
                validationResult = Valid.ViewModel(viewModel);

                Assert.IsFalse(validationResult.IsValid);
                Assert.IsTrue(validationResult.ErrorResults.Count > 0);
            }
        }

        [TestMethod]
        public void TestAddUserViewModel_GoodData()
        {
            var viewModels = GetGoodAddUserViewModels();

            ViewModelValidationResult validationResult;

            foreach (var viewModel in viewModels)
            {
                validationResult = Valid.ViewModel(viewModel);

                Assert.IsTrue(validationResult.IsValid);
                Assert.IsTrue(validationResult.ErrorResults.Count == 0);
            }
        }
    }
}
