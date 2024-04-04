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
    public class VerifyArqcAndGenerateArpcFromHsmRequest
    {
        public string AcUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string PanSequenceNumber { get; set; }
        public string Arc { get; set; }
        public string EmvData { get; set; }
    }

    public class VerifyArqcAndGenerateArpcFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string ArpcData { get; set; }
    }

    public class VerifyArqcAndGenerateArpcFromHsmDbJob : BaseDbOperation<VerifyArqcAndGenerateArpcFromHsmRequest, VerifyArqcAndGenerateArpcFromHsmResponse>
    {
        public VerifyArqcAndGenerateArpcFromHsmResponse response = new();

        public override VerifyArqcAndGenerateArpcFromHsmResponse OnProcess(VerifyArqcAndGenerateArpcFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            VerifyArqcAndGenerateArpcApiRequest hsmRequest = new VerifyArqcAndGenerateArpcApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.VerifyArqcAndGenerateArpc(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.ArpcData = hsmResponse.ArpcData;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}