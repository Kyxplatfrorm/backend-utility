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
    public class EncryptDataFromHsmRequest
    {
        public string EncryptionKeyUnderLmk { get; set; }
        public string Data { get; set; }
    }

    public class EncryptDataFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string EncryptedData { get; set; }
    }

    public class EncryptDataFromHsmDbJob : BaseDbOperation<EncryptDataFromHsmRequest, EncryptDataFromHsmResponse>
    {
        public EncryptDataFromHsmResponse response = new();

        public override EncryptDataFromHsmResponse OnProcess(EncryptDataFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            var hsmResponse = HsmWebApiClient.EncryptData(request.EncryptionKeyUnderLmk, request.Data);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.EncryptedData = hsmResponse.EncryptedData;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}
