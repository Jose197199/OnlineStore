1- I have created the following projects:
   - OnlineStore: ASP.NET Web API project
   - OnlineStore.Models: class library containing model/data contracts
   - OnlineStore.IntegrationTests: contains API integration tests
   - OnlineStore.UnitTests: contains unit tests.
2- The integration tests connect to REST API via HttpClient with Microsoft.AspNet.WebApi.Client extensions which build on top Newtonsoft.Json.
3- Data is only held in memory in order to save time for this assignment and simplify setup.
4- I could have unit tested the controllers but they are mainly wrapping the repositories and the order processor class so decided to unit test those instead.
5- Item name is used as a key for simplicity. 
6- OrderRepository does not have any logic so not tested.
7- I could have created data transfer object instead of returning models but that seemed like overkill right now since models are very simple.
8- I had to add an Orders controller so I could follow good REST API design guidelines.
9- I had to add other operations not requested in home work assignment to facilitate testing and protected them with special token.
10- OrderProcessor could have just thrown exceptions instead of returning a result object but I found this was cleaner to write tests against.
11- Authentication is only enabled for OrdersController, following homework assignment description.I am using a custom token based authentication which for simplicity 
    all it does is check the Authorization header contains a valid token which right now just checks against a fixed token. This was done like this to avoid having to
	implement user registration, login, etc.
13- Authentication is validated using integration tests.
14- Integration tests are not as extensive as unit tests since we do not need to repeat the same detailed tests again.
15- In a real application the controllers and business logic would only depend on repository interfaces and dependency injection would be used but since this application
    does not use a real database and holds all data in memory this was not done.


Answers to questions:

3.a) Choice of data format:
     The API sends and receives HTTP requests with Json as a data serialization format.
	 Example 1 (items controller):
	 request: 
	   GET api/items
	 response:
	   200 
	   [{ "name": "Binoculars", "description": "...", "price": 50, "stock": 3}, { "name":"Telescope", "description": "...", "price": 150, "stock": 10}]
	 Example 2 (orders controller)
	 request: 
	   POST api/orders/ 
	   {"itemName": "Binoculars", "quantity": 1}
	 response:
	   200
	   {"itemName": "Binoculars", "stock": 2}

3.b) A token based authentication mechanism was chosen since basic/digest authentication is too insecure and forms authentication besides being vulnerable to 
     cros-site request forgery is not usable if API is not used from within a site. The implementation is very basic however and only validates agains a fixed token.
	 This was done to simplify infrastructure needed and to simplify integration tests. See point 10 above!

Setup notes:
-----------
Base url for service can be configured in the app.config for OnlineStore.IntegrationTests as follows:
 
  <appSettings>
    <add key="serviceUri" value="http://localhost:8000" />
  </appSettings>
