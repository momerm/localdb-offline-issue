using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace LocalDbOfflineBug
{
    [TestFixture]
    public class Tests
    {
        static SqlInstance sqlInstance = new(
            name: "StaticConstructorInstance",
            buildTemplate: TestDbBuilder.CreateTable);

        [TestCaseSource(nameof(DatabaseNames))]
        public async Task Test(string name)
        {
            await using var database = await sqlInstance.Build(name);
            await TestDbBuilder.AddData(database);
            var data = await TestDbBuilder.GetData(database);
            ClassicAssert.AreEqual(1, data.Count);
        }

        static IEnumerable<string> DatabaseNames()
        {
            for (int i = 0; i < 1000; i++)
            {
                yield return "Db" + i.ToString();
            }

        }
    }
}