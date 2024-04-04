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
    public class TranslateZmkFromOldLmkToNewLmkFromHsmRequest
    {
        public string LmkZmkKey { get; set; }
        public string LmkIdentifier { get; set; }
    }

    public class TranslateZmkFromOldLmkToNewLmkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string ZmkKeyUnderLmk { get; set; }
    }

    public class TranslateZmkFromOldLmkToNewLmkFromHsmDbJob : BaseDbOperation<TranslateZmkFromOldLmkToNewLmkFromHsmRequest, TranslateZmkFromOldLmkToNewLmkFromHsmResponse>
    {
        public TranslateZmkFromOldLmkToNewLmkFromHsmResponse response = new();

        public override TranslateZmkFromOldLmkToNewLmkFromHsmResponse OnProcess(TranslateZmkFromOldLmkToNewLmkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslateZmkFromOldLmkToNewLmkApiRequest hsmRequest = new TranslateZmkFromOldLmkToNewLmkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslateZmkFromOldLmkToNewLmk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.ZmkKeyUnderLmk = hsmResponse.ZmkKeyUnderLmk;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}
