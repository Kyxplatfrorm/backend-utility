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
    public class GeneratePinLmkFromHsmRequest
    {
        public string CardNumber { get; set; }
        public string PinLength { get; set; } = "04";
        public string LmkType { get; set; }
        public string LmkIdentifier { get; set; } = "00";
    }

    public class GeneratePinLmkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public long PinLmk { get; set; }
        public string EncryptedPin { get; set; }
    }

    public class GeneratePinLmkFromHsmDbJob : BaseDbOperation<GeneratePinLmkFromHsmRequest, GeneratePinLmkFromHsmResponse>
    {
        public GeneratePinLmkFromHsmResponse response = new();

        public override GeneratePinLmkFromHsmResponse OnProcess(GeneratePinLmkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GeneratePinLmkApiRequest hsmRequest = new GeneratePinLmkApiRequest();
            hsmRequest.CardNumber = request.CardNumber;
            hsmRequest.LmkIdentifier = request.LmkIdentifier;
            hsmRequest.LmkType = request.LmkType;
            hsmRequest.PinLength = request.PinLength;
            if (string.IsNullOrWhiteSpace(hsmRequest.PinLength))
                hsmRequest.PinLength = "04";

            var hsmResponse = HsmWebApiClient.GeneratePinLmk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.PinLmk = hsmResponse.PinLmk;
            response.EncryptedPin = hsmResponse.EncryptedPin;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}