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
    public class GenerateKcvFromHsmRequest
    {
        public long KeyTypeId { get; set; }
        public long KeyLengthTypeId { get; set; }
        public string KeyUnderLmk { get; set; }
        public string LmkIdentifier { get; set; } = "00";
    }

    public class GenerateKcvFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string KeyCheckValue { get; set; }
    }

    public class GenerateKcvFromHsmDbJob : BaseDbOperation<GenerateKcvFromHsmRequest, GenerateKcvFromHsmResponse>
    {
        public GenerateKcvFromHsmResponse response = new();

        public override GenerateKcvFromHsmResponse OnProcess(GenerateKcvFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            var keyType = dbContext.FindParameter("HsmKcvKeyType", request.KeyTypeId);
            if (keyType == null)
                return response.ResultError("InvalidHsmKcvKeyType", "Invalid Key Type");

            var keyLengthType = dbContext.FindParameter("HsmKeyLengthFlagType", request.KeyLengthTypeId);
            if (keyLengthType == null)
                return response.ResultError("InvalidHsmKeyLengthFlagType", "Invalid Key Length Type");

            var hsmResponse = HsmWebApiClient.GenerateKcv(keyType.ParameterKey, keyLengthType.ParameterKey, request.KeyUnderLmk,request.LmkIdentifier);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.KeyCheckValue = hsmResponse.KeyCheckValue;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}