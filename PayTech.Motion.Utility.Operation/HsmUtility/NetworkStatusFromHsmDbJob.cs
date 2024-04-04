using PayTech.Core;
using System;
using DapperMapper;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PayTech.Motion.Model;
using PayTech.Switch.Core;

namespace PayTech.Motion.Utility.Operation
{
    public class NetworkStatusFromHsmRequest
    {
        public string LmkType { get; set; }
    }

    public class NetworkStatusFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
    }

    public class NetworkStatusFromHsmDbJob : BaseDbOperation<NetworkStatusFromHsmRequest, NetworkStatusFromHsmResponse>
    {
        public NetworkStatusFromHsmResponse response = new();

        public override NetworkStatusFromHsmResponse OnProcess(NetworkStatusFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            NetworkStatusApiRequest hsmRequest = new NetworkStatusApiRequest();
            hsmRequest.LmkType = request.LmkType;

            var hsmResponse = HsmWebApiClient.NetworkStatus(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}