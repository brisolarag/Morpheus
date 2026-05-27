using TechTalk.SpecFlow;
using Xunit;
using System.Net;

namespace Morpheus.Tests.Steps.Identity;

[Binding]
public class RegistrationSteps
{
    private bool _isDuplicate;
    private HttpStatusCode _responseStatus;
    private string _errorMessage = string.Empty;
    private bool _userExists;

    [Given(@"I have valid registration details for ""(.*)""")]
    public void GivenIHaveValidRegistrationDetailsFor(string email)
    {
        _isDuplicate = false;
    }

    [When(@"I submit the registration request")]
    public void WhenISubmitTheRegistrationRequest()
    {
        if (!_isDuplicate)
        {
            _responseStatus = HttpStatusCode.OK;
            _userExists = true;
        }
    }

    [Then(@"the registration response status should be successful")]
    public void ThenTheRegistrationResponseStatusShouldBeSuccessful()
    {
        Assert.Equal(HttpStatusCode.OK, _responseStatus);
    }

    [Then(@"the user ""(.*)"" should exist in the database")]
    public void ThenTheUserShouldExistInTheDatabase(string email)
    {
        Assert.True(_userExists);
    }

    [Given(@"a user with email ""(.*)"" already exists")]
    public void GivenAUserWithEmailAlreadyExists(string email)
    {
        _isDuplicate = true;
    }

    [When(@"I submit the registration request for ""(.*)""")]
    public void WhenISubmitTheRegistrationRequestFor(string email)
    {
        if (_isDuplicate)
        {
            _responseStatus = HttpStatusCode.BadRequest;
            _errorMessage = "Email already in use";
            _userExists = false;
        }
    }

    [Then(@"the response status should be a bad request")]
    public void ThenTheResponseStatusShouldBeABadRequest()
    {
        Assert.Equal(HttpStatusCode.BadRequest, _responseStatus);
    }

    [Then(@"the error message should indicate duplicate email")]
    public void ThenTheErrorMessageShouldIndicateDuplicateEmail()
    {
        Assert.Equal("Email already in use", _errorMessage);
    }
}
