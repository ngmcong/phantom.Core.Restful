# phantom.Core.Restful
$\textsf{\color{#52b95e}{RestfulHelper}}$ restfulHelper = new $\textsf{\color{#52b95e}{RestfulHelper}}$();<br/>
restfulHelper.BaseUrl = "https://localhost:8001/api";<br/>
$\textsf{\color{#357edb}{var}}$ objects = await restfulHelper.GetAysnc<IEnumerable\<Order>>("tables/loadOrders");<br/>
$\textsf{\color{#357edb}{var}}$ retVal = await restfulHelper.PostAsync\<ReturnAPICode>("tables/uploadOrders", objectBody as List\<Order>);
