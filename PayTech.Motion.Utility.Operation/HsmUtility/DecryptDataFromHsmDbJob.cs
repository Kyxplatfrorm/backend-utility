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
    public class DecryptDataFromHsmRequest
    {
        public string EncryptionKeyUnderLmk { get; set; }
        public string EncryptedData { get; set; }
    }

    public class DecryptDataFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string DecryptedData { get; set; }
    }

    public class DecryptDataFromHsmDbJob : BaseDbOperation<DecryptDataFromHsmRequest, DecryptDataFromHsmResponse>
    {
        public DecryptDataFromHsmResponse response = new();

        public override DecryptDataFromHsmResponse OnProcess(DecryptDataFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            var hsmResponse = HsmWebApiClient.DecryptData(request.EncryptionKeyUnderLmk, request.EncryptedData);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.DecryptedData = hsmResponse.DecryptedData;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}