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
    public class TranslateZpkFromOldLmkToNewLmkFromHsmRequest
    {
        public string ZpkLmkKey { get; set; }
        public string LmkIdentifier { get; set; }
    }

    public class TranslateZpkFromOldLmkToNewLmkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string ZpkUnderLmk { get; set; }
    }

    public class TranslateZpkFromOldLmkToNewLmkFromHsmDbJob : BaseDbOperation<TranslateZpkFromOldLmkToNewLmkFromHsmRequest, TranslateZpkFromOldLmkToNewLmkFromHsmResponse>
    {
        public TranslateZpkFromOldLmkToNewLmkFromHsmResponse response = new();

        public override TranslateZpkFromOldLmkToNewLmkFromHsmResponse OnProcess(TranslateZpkFromOldLmkToNewLmkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslateZpkFromOldLmkToNewLmkApiRequest hsmRequest = new TranslateZpkFromOldLmkToNewLmkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslateZpkFromOldLmkToNewLmk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.ZpkUnderLmk = hsmResponse.ZpkUnderLmk;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}