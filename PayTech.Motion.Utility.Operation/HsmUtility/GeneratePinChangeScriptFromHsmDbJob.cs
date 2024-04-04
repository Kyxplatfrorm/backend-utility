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
    public class GeneratePinChangeScriptFromHsmRequest
    {
        public string AcUnderLmk { get; set; }
        public string EncUnderLmk { get; set; }
        public string MacKeyUnderLmk { get; set; }
        public string ZpkUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string PanSequenceNumber { get; set; }
        public string Atc { get; set; }
        public string Arqc { get; set; }
        public string PinBlockFormat { get; set; }
        public string PinBlock { get; set; }
    }

    public class GeneratePinChangeScriptFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string IssuerScript { get; set; }
    }

    public class GeneratePinChangeScriptFromHsmDbJob : BaseDbOperation<GeneratePinChangeScriptFromHsmRequest, GeneratePinChangeScriptFromHsmResponse>
    {
        public GeneratePinChangeScriptFromHsmResponse response = new();

        public override GeneratePinChangeScriptFromHsmResponse OnProcess(GeneratePinChangeScriptFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GeneratePinChangeScriptApiRequest hsmRequest = new GeneratePinChangeScriptApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GeneratePinChangeScript(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.IssuerScript = hsmResponse.IssuerScript;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}