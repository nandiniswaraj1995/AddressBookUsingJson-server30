using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RESTSharpTest;
using System.Collections.Generic;

namespace Json_server_addressBookUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        private IRestResponse getContactList()
        {
            RestRequest request = new RestRequest("/AddressBook", Method.GET);

            //act

            IRestResponse response = client.Execute(request);
            return response;
        }
       //UC22
        [TestMethod]
        public void onCallingGETApi_ReturnTotalContactList()
        {
            IRestResponse response = getContactList();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<AddressBook> dataResponse = JsonConvert.DeserializeObject<List<AddressBook>>(response.Content);
            Assert.AreEqual(4, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "First Name: " + item.first_name + "last_name: " + item.last_name+" "+
                    "address: " + item.address + "city: " + item.city + "state: " + item.state+" "+
                    "phone_numner: " + item.phone_number + "email: " + item.email );
            }
        }

        //UC23
        [TestMethod]
        public void givenContact_OnPost_ShouldReturnAddedContact()
        {
            RestRequest request = new RestRequest("/AddressBook", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("first_name", "sneha");
            jObjectbody.Add("last_name", "singh");
            jObjectbody.Add("address", "tpr");
            jObjectbody.Add("city", "cpr");
            jObjectbody.Add("state", "bhr");
            jObjectbody.Add("zip", "654321");
            jObjectbody.Add("phone_number", "7654321890");
            jObjectbody.Add("email", "sneha@gmail.com");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            AddressBook dataResponse = JsonConvert.DeserializeObject<AddressBook>(response.Content);
            Assert.AreEqual("sneha", dataResponse.first_name);
            Assert.AreEqual("singh", dataResponse.last_name);
            Assert.AreEqual("tpr", dataResponse.address);
            Assert.AreEqual("cpr", dataResponse.city);
            Assert.AreEqual("bhr", dataResponse.state);
            Assert.AreEqual("654321", dataResponse.zip);
            Assert.AreEqual("7654321890", dataResponse.phone_number);
            Assert.AreEqual("sneha@gmail.com", dataResponse.email);


        }

        //UC24
        [TestMethod]
        public void givenContact_OnUpdate_ShouldReturnUpdatedContact()
        {
            RestRequest request = new RestRequest("/AddressBook/4", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("first_name", "rani");
            jObjectbody.Add("last_name", "singh");
            jObjectbody.Add("address", "tpr");
            jObjectbody.Add("city", "cpr");
            jObjectbody.Add("state", "bhr");
            jObjectbody.Add("zip", "654321");
            jObjectbody.Add("phone_number", "7654321890");
            jObjectbody.Add("email", "rani@gmail.com");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            AddressBook dataResponse = JsonConvert.DeserializeObject<AddressBook>(response.Content);
            Assert.AreEqual("rani", dataResponse.first_name);
            Assert.AreEqual("rani@gmail.com", dataResponse.email);



        }

        //UC25
        [TestMethod]
        public void givenPersonId_OnDelete_ShouldReturnSuccessStatus()
        {
            //arrange
            RestRequest request = new RestRequest("/AddressBook/4", Method.DELETE);

            //act
            IRestResponse response = client.Execute(request);

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            System.Console.WriteLine(response.Content);

        }




    }
}
