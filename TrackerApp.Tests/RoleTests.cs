using TrackerApp.Core.Services.Static;

namespace TrackerApp.Tests
{
    [TestClass]
    public sealed class RoleTests
    {
        private static string[] GetBadRoles()
        {
            return
            [
                "ab",
                "A1B2C3",
                "ThisIsWayTooLongForThePattern",
                "abc!",
                "hello_world",
                "John Doe",
                "mañana",
                "user-name",
                "123",
                ""
            ];
        }

        private static string[] GetGoodRoles()
        {
            return
            [
                "Admin",
                "HelloWorld",
                "SimpleTest",
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[..20],
                "upperlowerCASE",
                "TestValue",
                "AlphaBravo",
                "CodeIsFun",
                "Programming",
                "ValidInput"
            ];
        }

        [TestMethod]
        public void TestRoles_BadData()
        {
            var roles = GetBadRoles();

            foreach (var role in roles)
                Assert.IsFalse(Valid.Role(role));
        }

        [TestMethod]
        public void TestRoles_GoodData()
        {
            var roles = GetGoodRoles();

            foreach (var role in roles)
                Assert.IsTrue(Valid.Role(role));
        }
    }
}
