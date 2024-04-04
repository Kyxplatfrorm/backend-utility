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
    public class TranslatePinUnderZpk2LmkFromHsmRequest
    {
        public string ZpkUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string PinUnderZpk { get; set; }
        public string PinBlockFormat { get; set; }
    }

    public class TranslatePinUnderZpk2LmkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string PinLmk { get; set; }
    }

    public class TranslatePinUnderZpk2LmkFromHsmDbJob : BaseDbOperation<TranslatePinUnderZpk2LmkFromHsmRequest, TranslatePinUnderZpk2LmkFromHsmResponse>
    {
        public TranslatePinUnderZpk2LmkFromHsmResponse response = new();

        public override TranslatePinUnderZpk2LmkFromHsmResponse OnProcess(TranslatePinUnderZpk2LmkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslatePinUnderZpkToLmkApiRequest hsmRequest = new TranslatePinUnderZpkToLmkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslatePinUnderZpkToLmk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.PinLmk = hsmResponse.EncryptedPin;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}
