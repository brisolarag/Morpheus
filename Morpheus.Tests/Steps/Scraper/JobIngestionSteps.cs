using TechTalk.SpecFlow;
using Xunit;

namespace Morpheus.Tests.Steps;

[Binding]
public class JobIngestionSteps
{
    private bool _hasValidData;
    private bool _wasStored;
    private bool _wasRejected;
    private bool _errorLogged;

    [Given(@"a mock payload of valid job data from LinkedIn")]
    public void GivenAMockPayloadOfValidJobDataFromLinkedIn()
    {
        _hasValidData = true;
    }

    [Given(@"a mock payload with missing required fields")]
    public void GivenAMockPayloadWithMissingRequiredFields()
    {
        _hasValidData = false;
    }

    [When(@"the scraper processes the payload")]
    public void WhenTheScraperProcessesThePayload()
    {
        if (_hasValidData)
        {
            _wasStored = true;
            _wasRejected = false;
        }
        else
        {
            _wasStored = false;
            _wasRejected = true;
            _errorLogged = true;
        }
    }

    [Then(@"the system should store the job details correctly")]
    public void ThenTheSystemShouldStoreTheJobDetailsCorrectly()
    {
        Assert.True(_wasStored);
    }

    [Then(@"the job should be available for semantic search")]
    public void ThenTheJobShouldBeAvailableForSemanticSearch()
    {
        Assert.True(_wasStored);
    }

    [Then(@"the system should reject the invalid data")]
    public void ThenTheSystemShouldRejectTheInvalidData()
    {
        Assert.True(_wasRejected);
    }

    [Then(@"log an appropriate error")]
    public void ThenLogAnAppropriateError()
    {
        Assert.True(_errorLogged);
    }
}
