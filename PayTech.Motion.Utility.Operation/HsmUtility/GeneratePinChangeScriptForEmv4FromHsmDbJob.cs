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
    public class GeneratePinChangeScriptForEmv4FromHsmRequest
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
        public string SchemaId { get; set; }
        public string BranchAndHeigthMode { get; set; }
    }

    public class GeneratePinChangeScriptForEmv4FromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string IssuerScript { get; set; }
    }

    public class GeneratePinChangeScriptForEmv4FromHsmDbJob : BaseDbOperation<GeneratePinChangeScriptForEmv4FromHsmRequest, GeneratePinChangeScriptForEmv4FromHsmResponse>
    {
        public GeneratePinChangeScriptForEmv4FromHsmResponse response = new();

        public override GeneratePinChangeScriptForEmv4FromHsmResponse OnProcess(GeneratePinChangeScriptForEmv4FromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GeneratePinChangeScriptEmv4ApiRequest hsmRequest = new GeneratePinChangeScriptEmv4ApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GeneratePinChangeScriptEmv4(hsmRequest);
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