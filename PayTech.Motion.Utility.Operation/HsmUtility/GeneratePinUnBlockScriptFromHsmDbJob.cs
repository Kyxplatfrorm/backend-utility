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
    public class GeneratePinUnBlockScriptFromHsmRequest
    {
        public string MacKeyUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string PanSequenceNumber { get; set; }
        public string Atc { get; set; }
        public string Arqc { get; set; }
    }

    public class GeneratePinUnBlockScriptFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string IssuerScript { get; set; }
    }

    public class GeneratePinUnBlockScriptFromHsmDbJob : BaseDbOperation<GeneratePinUnBlockScriptFromHsmRequest, GeneratePinUnBlockScriptFromHsmResponse>
    {
        public GeneratePinUnBlockScriptFromHsmResponse response = new();

        public override GeneratePinUnBlockScriptFromHsmResponse OnProcess(GeneratePinUnBlockScriptFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GeneratePinUnBlockScriptApiRequest hsmRequest = new GeneratePinUnBlockScriptApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GeneratePinUnBlockScript(hsmRequest);
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
