using Microsoft.SharePoint.Client.Services;
using System.ServiceModel.Activation;

namespace $rootnamespace$
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class $safeitemname$ : I$safeitemname$
    {

        //IMPORTANT
        //When a web application is configured to use claims authentication (Windows claims, form-based authentication claims, or SAML claims), 
        //the IIS website is always configured to have anonymous access turned on. Your custom SOAP and WCF endpoints may receive requests 
        //from anonymous users. If you have code in your WCF service that calls the RunWithElevatedPrivileges method to access information 
        //without first checking whether the call is from an authorized user or an anonymous user, you risk returning protected SharePoint 
        //data to any anonymous user for some of your functions that use that approach.

        // To test this service, use the Visual Studio WCF Test client
        // set the endpoint to http://<Your server name>/_vti_bin/$safeitemname$.svc/mex
        public string HelloWorld()
        {
            return "Hello World from WCF and SharePoint 2010";
        }
    }
}
