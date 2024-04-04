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
    public class GeneratePvvFromHsmRequest
    {
        public string CardNumber { get; set; }
        public string PinUnderLmk { get; set; }
        public string PvvKeyIndex { get; set; }
        public string PvvKey { get; set; }
    }

    public class GeneratePvvFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string Pvv { get; set; }
    }

    public class GeneratePvvFromHsmDbJob : BaseDbOperation<GeneratePvvFromHsmRequest, GeneratePvvFromHsmResponse>
    {
        public GeneratePvvFromHsmResponse response = new();

        public override GeneratePvvFromHsmResponse OnProcess(GeneratePvvFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GeneratePvvApiRequest hsmRequest = new GeneratePvvApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GeneratePvv(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.Pvv = hsmResponse.Pvv;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}