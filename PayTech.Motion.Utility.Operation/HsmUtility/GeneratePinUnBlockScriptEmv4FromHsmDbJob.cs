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
    public class GeneratePinUnBlockScriptEmv4FromHsmRequest
    {
        public string MacKeyUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string PanSequenceNumber { get; set; }
        public string Atc { get; set; }
        public string Arqc { get; set; }
        public string ModeFlag { get; set; }
        public string SchemaId { get; set; }
        public string BranchAndHeigthMode { get; set; }
    }

    public class GeneratePinUnBlockScriptEmv4FromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string IssuerScript { get; set; }
    }

    public class GeneratePinUnBlockScriptEmv4FromHsmDbJob : BaseDbOperation<GeneratePinUnBlockScriptEmv4FromHsmRequest, GeneratePinUnBlockScriptEmv4FromHsmResponse>
    {
        public GeneratePinUnBlockScriptEmv4FromHsmResponse response = new();

        public override GeneratePinUnBlockScriptEmv4FromHsmResponse OnProcess(GeneratePinUnBlockScriptEmv4FromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GeneratePinUnBlockScriptEmv4ApiRequest hsmRequest = new GeneratePinUnBlockScriptEmv4ApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GeneratePinUnBlockScriptEmv4(hsmRequest);
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
