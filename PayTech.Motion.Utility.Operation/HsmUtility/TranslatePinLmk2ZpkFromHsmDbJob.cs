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
    public class TranslatePinLmk2ZpkFromHsmRequest
    {
        public string ZpkUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string PinLmk { get; set; }
        public string PinBlockFormat { get; set; }
        public string LmkIdentifier { get; set; } = "00";
    }

    public class TranslatePinLmk2ZpkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string PinUnderZpk { get; set; }
    }

    public class TranslatePinLmk2ZpkFromHsmDbJob : BaseDbOperation<TranslatePinLmk2ZpkFromHsmRequest, TranslatePinLmk2ZpkFromHsmResponse>
    {
        public TranslatePinLmk2ZpkFromHsmResponse response = new();

        public override TranslatePinLmk2ZpkFromHsmResponse OnProcess(TranslatePinLmk2ZpkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslatePinUnderLmkToZpkApiRequest hsmRequest = new TranslatePinUnderLmkToZpkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslatePinUnderLmkToZpk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.PinUnderZpk = hsmResponse.PinUnderZpk;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}
