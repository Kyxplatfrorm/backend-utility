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
    public class TranslateZpkZmkToZpkLmkFromHsmRequest
    {
        public string ZmkUnderLmk { get; set; }
        public string ZpkUnderZmk { get; set; }
        public string AtallaVariant { get; set; }
        public string LmkIdentifier { get; set; }
    }

    public class TranslateZpkZmkToZpkLmkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string ZpkUnderLmk { get; set; }
        public string ZpkKcv { get; set; }
    }

    public class TranslateZpkZmkToZpkLmkFromHsmDbJob : BaseDbOperation<TranslateZpkZmkToZpkLmkFromHsmRequest, TranslateZpkZmkToZpkLmkFromHsmResponse>
    {
        public TranslateZpkZmkToZpkLmkFromHsmResponse response = new();

        public override TranslateZpkZmkToZpkLmkFromHsmResponse OnProcess(TranslateZpkZmkToZpkLmkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslateZpkZmkToZpkLmkApiRequest hsmRequest = new TranslateZpkZmkToZpkLmkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslateZpkZmkToZpkLmk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.ZpkUnderLmk = hsmResponse.ZpkUnderLmk;
            response.ZpkKcv = hsmResponse.ZpkKcv;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}