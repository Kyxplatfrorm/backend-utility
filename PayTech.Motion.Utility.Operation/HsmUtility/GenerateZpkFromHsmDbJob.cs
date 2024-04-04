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
    public class GenerateZpkFromHsmRequest
    {
        public string ZmkUnderLmk { get; set; }
        public string AtallaVariant { get; set; }
    }

    public class GenerateZpkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string ZpkUnderLmk { get; set; }
        public string ZpkUnderZmk { get; set; }
        public string ZpkKcv { get; set; }
    }

    public class GenerateZpkFromHsmDbJob : BaseDbOperation<GenerateZpkFromHsmRequest, GenerateZpkFromHsmResponse>
    {
        public GenerateZpkFromHsmResponse response = new();

        public override GenerateZpkFromHsmResponse OnProcess(GenerateZpkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GenerateZpkApiRequest hsmRequest = new GenerateZpkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GenerateZpk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.ZpkUnderLmk = hsmResponse.ZpkUnderLmk;
            response.ZpkUnderZmk = hsmResponse.ZpkUnderZmk;
            response.ZpkKcv = hsmResponse.ZpkKcv;

            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}