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
    public class VerifyArqcAndGenerateArpcEmv4FromHsmRequest
    {
        public string AcUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string PanSequenceNumber { get; set; }
        public string Arc { get; set; }
        public string ModeFlag { get; set; }
        public string ProprietaryAuthenticationData { get; set; }
        public string Csu { get; set; }
        public string EmvData { get; set; }
    }

    public class VerifyArqcAndGenerateArpcEmv4FromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string ArpcData { get; set; }
    }

    public class VerifyArqcAndGenerateArpcEmv4FromHsmDbJob : BaseDbOperation<VerifyArqcAndGenerateArpcEmv4FromHsmRequest, VerifyArqcAndGenerateArpcEmv4FromHsmResponse>
    {
        public VerifyArqcAndGenerateArpcEmv4FromHsmResponse response = new();

        public override VerifyArqcAndGenerateArpcEmv4FromHsmResponse OnProcess(VerifyArqcAndGenerateArpcEmv4FromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            VerifyArqcAndGenerateArpcEmv4ApiRequest hsmRequest = new VerifyArqcAndGenerateArpcEmv4ApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.VerifyArqcAndGenerateArpcEmv4(hsmRequest);
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