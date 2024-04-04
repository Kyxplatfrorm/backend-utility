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
    public class TranslatePinUnderZpkToZpk2FromHsmRequest
    {
        public string CardNumber { get; set; }
        public string SourceZpkUnderLmk { get; set; }
        public string SourcePinBlockFormat { get; set; }
        public string PinUnderSourceZpk { get; set; }
        public string DestinationZpkUnderLmk { get; set; }
        public string DestinationPinBlockFormat { get; set; }
        public string LmkType { get; set; }
        public string MaksimumPinLength { get; set; } = "12";
        public string LmkIdentifier { get; set; } = "00";
    }

    public class TranslatePinUnderZpkToZpk2FromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string LengthOfThePin { get; set; }
        public string PinUnderDestinationZpk { get; set; }
        public string DestinationPinBlockFormat { get; set; }
    }

    public class TranslatePinUnderZpkToZpk2FromHsmDbJob : BaseDbOperation<TranslatePinUnderZpkToZpk2FromHsmRequest, TranslatePinUnderZpkToZpk2FromHsmResponse>
    {
        public TranslatePinUnderZpkToZpk2FromHsmResponse response = new();

        public override TranslatePinUnderZpkToZpk2FromHsmResponse OnProcess(TranslatePinUnderZpkToZpk2FromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslatePinUnderZpkToZpk2ApiRequest hsmRequest = new TranslatePinUnderZpkToZpk2ApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslatePinUnderZpkToZpk2(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.LengthOfThePin = hsmResponse.LengthOfThePin;
            response.PinUnderDestinationZpk = hsmResponse.PinUnderDestinationZpk;
            response.DestinationPinBlockFormat = hsmResponse.DestinationPinBlockFormat;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}