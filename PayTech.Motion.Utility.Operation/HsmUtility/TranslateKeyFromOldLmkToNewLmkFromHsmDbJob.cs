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
    public class TranslateKeyFromOldLmkToNewLmkFromHsmRequest
    {
        public string KeyTypeCode { get; set; }
        public string KeyLength { get; set; }
        public string LmkKey { get; set; }
        public string KeyType { get; set; }
        public string LmkIdentifier { get; set; }
        public string KeyUsage { get; set; }
        public string ModeOfUse { get; set; }
        public string KeyVersionNumber { get; set; }
        public string Exportability { get; set; }
        public string NumberOfOptionalBlocks { get; set; }
        public string OptionalBlocks { get; set; }
    }

    public class TranslateKeyFromOldLmkToNewLmkFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string KeyUnderLmk { get; set; }
    }

    public class TranslateKeyFromOldLmkToNewLmkFromHsmDbJob : BaseDbOperation<TranslateKeyFromOldLmkToNewLmkFromHsmRequest, TranslateKeyFromOldLmkToNewLmkFromHsmResponse>
    {
        public TranslateKeyFromOldLmkToNewLmkFromHsmResponse response = new();

        public override TranslateKeyFromOldLmkToNewLmkFromHsmResponse OnProcess(TranslateKeyFromOldLmkToNewLmkFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            TranslateKeyFromOldLmkToNewLmkApiRequest hsmRequest = new TranslateKeyFromOldLmkToNewLmkApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.TranslateKeyFromOldLmkToNewLmk(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.KeyUnderLmk = hsmResponse.KeyUnderLmk;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}