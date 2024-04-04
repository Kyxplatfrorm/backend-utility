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
    public class GenerateCvvFromHsmRequest
    {
        public string CvvKey { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDateYYMM { get; set; }
        public string ServiceCode { get; set; }
    }

    public class GenerateCvvFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string Cvv { get; set; }
    }

    public class GenerateCvvFromHsmDbJob : BaseDbOperation<GenerateCvvFromHsmRequest, GenerateCvvFromHsmResponse>
    {
        public GenerateCvvFromHsmResponse response = new();

        public override GenerateCvvFromHsmResponse OnProcess(GenerateCvvFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            var hsmResponse = HsmWebApiClient.GenerateCVV(request.CvvKey, request.CardNumber, request.ExpiryDateYYMM, request.ServiceCode);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.Cvv = hsmResponse.Cvv;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}