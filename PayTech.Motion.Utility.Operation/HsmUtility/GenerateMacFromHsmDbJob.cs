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
    public class GenerateMacFromHsmRequest
    {
        public string MacKeyUnderLmk { get; set; }
        public string MacData { get; set; }
    }

    public class GenerateMacFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string Mac { get; set; }
    }

    public class GenerateMacFromHsmDbJob : BaseDbOperation<GenerateMacFromHsmRequest, GenerateMacFromHsmResponse>
    {
        public GenerateMacFromHsmResponse response = new();

        public override GenerateMacFromHsmResponse OnProcess(GenerateMacFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GenerateMacApiRequest hsmRequest = new GenerateMacApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GenerateMac(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.Mac = hsmResponse.Mac;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}