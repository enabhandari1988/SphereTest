using System;
using System.Web;
using TechTalk.SpecFlow;
using RestSharp;
using Xunit;
using System.Web.Script.Serialization;
using SphereTest.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using Utf8Json;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace SphereTest.Steps
{
    [Binding]
    public class GetBusinessPricePlans
    {
		public string response { get; set; }
		public string request { get; set; }
		public string body { get; set; }


		[Given(@"request to post sphere Business Price plans")]
		public void GivenRequestToPostSphereBusinessPricePlans()
		{
			body = ApiHelper.CreateAPIBody("reference","1");
		}

		[When(@"user client post the referenceCode to the website")]
		public void WhenUserClientPostTheReferenceCodeToTheWebsite()
		{
			APIContentType aPIContentType = APIContentType.Json;

			request = "https://testapi.sit.sphereidentity.com/priceplans/getBusinessPricePlan";
			response = ApiHelper.InvokeApiPost(request, body, aPIContentType);

		}

		[Then(@"Lists the different subscription Period are created")]
		[Then(@"Lists the different subscription volumes are created")]
		public void ThenListsTheDifferentSubscriptionVolumesAreCreated()
		{
			Assert.Equal("5", Utility.checkParentnodes(response));
			Assert.Equal("2", Utility.checkChildnodes(response));
		}



	}
}
