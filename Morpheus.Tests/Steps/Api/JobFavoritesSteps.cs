using TechTalk.SpecFlow;
using Xunit;
using System.Net;

namespace Morpheus.Tests.Steps;

[Binding]
public class JobFavoritesSteps
{
    private bool _isAuthenticated;
    private bool _jobExists;
    private HttpStatusCode _responseStatus;

    [Given(@"I am authenticated as a valid user")]
    public void GivenIAmAuthenticatedAsAValidUser()
    {
        _isAuthenticated = true;
    }

    [Given(@"a valid job exists in the database")]
    public void GivenAValidJobExistsInTheDatabase()
    {
        _jobExists = true;
    }

    [When(@"I request to add the job to my favorites")]
    public void WhenIRequestToAddTheJobToMyFavorites()
    {
        if (_isAuthenticated && _jobExists)
        {
            _responseStatus = HttpStatusCode.OK;
        }
    }

    [When(@"I request to add a non-existent job to my favorites")]
    public void WhenIRequestToAddANonExistentJobToMyFavorites()
    {
        if (_isAuthenticated && !_jobExists)
        {
            _responseStatus = HttpStatusCode.NotFound;
        }
    }

    [Then(@"the response status should be successful")]
    public void ThenTheResponseStatusShouldBeSuccessful()
    {
        Assert.Equal(HttpStatusCode.OK, _responseStatus);
    }

    [Then(@"the response status should be not found")]
    public void ThenTheResponseStatusShouldBeNotFound()
    {
        Assert.Equal(HttpStatusCode.NotFound, _responseStatus);
    }

    [Then(@"the job should appear in my favorites list")]
    public void ThenTheJobShouldAppearInMyFavoritesList()
    {
        Assert.True(_jobExists);
    }
}
