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
    public class SendRawMessageToHsmRequest
    {
        public string HsmRawHexRequest { get; set; }
        public string LmkType { get; set; }
    }

    public class SendRawMessageToHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string HsmRawHexResponse { get; set; }
    }

    public class SendRawMessageToHsmDbJob : BaseDbOperation<SendRawMessageToHsmRequest, SendRawMessageToHsmResponse>
    {
        public SendRawMessageToHsmResponse response = new();

        public override SendRawMessageToHsmResponse OnProcess(SendRawMessageToHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            ProcessRawHsmRequestApiRequest hsmRequest = new ProcessRawHsmRequestApiRequest();
            hsmRequest.LmkType = request.LmkType;
            hsmRequest.LmkType = request.LmkType;

            var hsmResponse = HsmWebApiClient.SendReceiveHsmMessage(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.HsmRawHexResponse = hsmResponse.HsmRawHexResponse;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}
