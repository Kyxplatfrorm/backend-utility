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
    public class TranslateZpkLmkToZpkZmkFromHsmRequest
    {
        public string ZmkUnderLmk { get; set; }
        public string ZpkUnderLmk { get; set; }
        public string AtallaVariant { get; set; }
        public string LmkIdentifier { get; set; } = "00";
    }

    public class TranslateZpkLmkToZpkZmkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string ZpkUnderZmk { get; set; }
        public string ZpkKcv { get; set; }
    }

    public class TranslateZpkLmkToZpkZmkFromHsmDbJob : BaseDbOperation<TranslateZpkLmkToZpkZmkFromHsmRequest, TranslateZpkLmkToZpkZmkFromHsmResponse>
    {
        public TranslateZpkLmkToZpkZmkFromHsmResponse response = new();

        public override TranslateZpkLmkToZpkZmkFromHsmResponse OnProcess(TranslateZpkLmkToZpkZmkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslateZpkLmkToZpkZmkApiRequest hsmRequest = new TranslateZpkLmkToZpkZmkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslateZpkLmkToZpkZmk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.ZpkUnderZmk = hsmResponse.ZpkUnderZmk;
            response.ZpkKcv = hsmResponse.ZpkKcv;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}