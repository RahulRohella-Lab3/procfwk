using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Threading.Tasks;

namespace FactoryTesting.Pipelines.Parent
{
    public class GivenNoErrorsAndSPNStoredInDatabase
    {
        private ParentHelper _helper;

        [OneTimeSetUp]
        public async Task WhenPipelineRun()
        {
            _helper = new ParentHelper()
                .WithBasicMetadata()
                .WithTenantAndSubscriptionIds()
                .WithSPNInDatabase("FrameworkFactory")
                .WithEmptyExecutionTables()
                .WithoutSimulatedError()
                .WithFailureHandling("Simple");
            await _helper.RunPipeline();
        }

        #region Integration tests

        [Test]
        public void ThenPipelineOutcomeIsSucceeded()
        {
            _helper.RunOutcome.Should().Be("Succeeded");
        }

        #endregion

        #region Functional tests

        [Test]
        public void ThenCurrentExecutionTableIsEmpty()
        {
            _helper.RowCount("procfwk.CurrentExecution").Should().Be(0);
        }

        [Test]
        public void ThenElevenExecutionLogRecords()
        {
            _helper.RowCount("procfwk.ExecutionLog").Should().Be(11);
        }

        #endregion

        [OneTimeTearDown]
        public void TearDown()
        {
            _helper?.TearDown();
        }
    }
}
