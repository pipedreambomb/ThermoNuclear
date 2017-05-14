# IO.Swagger.Api.WarheadsApi

All URIs are relative to *http://gitland.azurewebsites.net:80*

Method | HTTP request | Description
------------- | ------------- | -------------
[**WarheadsGetStatus**](WarheadsApi.md#warheadsgetstatus) | **GET** /api/warheads/status | Get the current nuclear warhead status.
[**WarheadsLaunch**](WarheadsApi.md#warheadslaunch) | **POST** /api/warheads/launch | Launch the nuclear warheads.


<a name="warheadsgetstatus"></a>
# **WarheadsGetStatus**
> WarheadStatusResult WarheadsGetStatus ()

Get the current nuclear warhead status.

This method can be used to check the current online status.              Sometimes the wi-fi can be shaky - especially when the microwave is being used.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class WarheadsGetStatusExample
    {
        public void main()
        {
            
            var apiInstance = new WarheadsApi();

            try
            {
                // Get the current nuclear warhead status.
                WarheadStatusResult result = apiInstance.WarheadsGetStatus();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WarheadsApi.WarheadsGetStatus: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**WarheadStatusResult**](WarheadStatusResult.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, text/json, application/xml, text/xml

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="warheadslaunch"></a>
# **WarheadsLaunch**
> WarheadLaunchResult WarheadsLaunch (string launchCode)

Launch the nuclear warheads.

The launch code changes every day and is in the format YYMMDD-AAAAAAAAAA (where YYMMDD is 6 digits representing the current              date and AAAAAAAAAA is the pass phrase given to the President). May return an error if somethings are not quite right.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class WarheadsLaunchExample
    {
        public void main()
        {
            
            var apiInstance = new WarheadsApi();
            var launchCode = launchCode_example;  // string | The launch code. See the implementation notes.

            try
            {
                // Launch the nuclear warheads.
                WarheadLaunchResult result = apiInstance.WarheadsLaunch(launchCode);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WarheadsApi.WarheadsLaunch: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **launchCode** | **string**| The launch code. See the implementation notes. | 

### Return type

[**WarheadLaunchResult**](WarheadLaunchResult.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, text/json, application/xml, text/xml

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

