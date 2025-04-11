using TrackerApp.Core.Services.Static;

namespace TrackerApp.Tests.Tests
{
    [TestClass]
    public sealed class RequestVerificationSecretTests
    {
        private static string[] GetBadRequestVerificationSecrets()
        {
            return
            [
                "abc123+/=abc123+/=abc123+/=abc123+/=abc123",
                "abc123+/=abc123+/=abc123+/=abc123+/=abc123456",
                "abc123+/=abc123+/=abc123+/=abc123+/=abc12!",
                "abc123+/=abc123+/=abc123+/=abc123+_abc123",
                "abc123+/=abc123+/=abc123+/=abc123+/=abc123 ",
                "abc123+/=abc123+/=abc123+/=abc123+\nabc123",
                "abc123+/=abc123+/=abc123+/=abc123+\tabc123",
                "abc123+/=abc123+/=abc123+/=abc123+/=abc12\r",
                "abc123+/=abc123+/=abc123+/=abc123+/=abc",
                ""
            ];
        }

        private static string[] GetGoodRequestVerificationSecrets()
        {
            return
            [
                "ETwM75xz1vGUmD1NW303wQ67E9shfSWk27KclptAtA4=",
                "datFRI97lbgE48SLO4oKJqX6g11CbOM5Ju9PLIw6ueE=",
                "RJo1KVnzBp/qoYfzwbwMzFdYTpzZromLKKfkIlG7V7U=",
                "m4fuHO1M3rpvIDmBXvYSxQ9W9d4mpTxrld6yee6TsDw=",
                "4HFRLy3d1jE3kBVxYCWH+nsflw/5Oq9xLKWPhdGff1A=",
                "tmH5OipL8mPgCHSVjSTBMtBaHZVF28GZvf0oH7WusxM=",
                "b/Qmcp3jr0NGFw5DJ8u4hKKAfB+PVXyPtxVqrMqx2Jc=",
                "yk/kZNyioPO1R4mJKrTYvm57V+6xJZVZU5duujZpE20=",
                "ANgRQcVKs0wDd8lSjC8SJ6h90uF9KFk5ujpUMhBAV6c=",
                "E0CowBRe4Zxx8LmBewlMebMkg6MN8LA7oPEoab5gROU="
            ];
        }

        [TestMethod]
        public void TestRequestVerificationSecret_BadData()
        {
            var badStrings = GetBadRequestVerificationSecrets();

            foreach (var str in badStrings)
                Assert.IsFalse(Valid.RequestVerificationSecret(str));
        }

        [TestMethod]
        public void TestRequestVerificationSecret_GoodData()
        {
            var goodStrings = GetGoodRequestVerificationSecrets();

            foreach (var str in goodStrings)
                Assert.IsTrue(Valid.RequestVerificationSecret(str));
        }
    }
}
