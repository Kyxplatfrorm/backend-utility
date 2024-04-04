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
    public class ImportKeyFromHsmRequest
    {
        public long KeyTypeId { get; set; }
        public string ZmkUnderLmk { get; set; }
        public string KeyUnderZmk { get; set; }
        public string KeySchema { get; set; }
        public string AtallaVariant { get; set; }
        public string LmkIdentifier { get; set; } = "00";
    }

    public class ImportKeyFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string KeyUnderLmk { get; set; }
        public string KeyKcv { get; set; }
    }

    public class ImportKeyFromHsmDbJob : BaseDbOperation<ImportKeyFromHsmRequest, ImportKeyFromHsmResponse>
    {
        public ImportKeyFromHsmResponse response = new();

        public override ImportKeyFromHsmResponse OnProcess(ImportKeyFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            var keyType = dbContext.FindParameter("HsmImportExportKeyType", request.KeyTypeId);
            if (keyType == null)
                return response.ResultError("InvalidHsmImportExportKeyType", "Invalid Key Type");

            ImportKeyApiRequest hsmRequest = new ImportKeyApiRequest();
            hsmRequest.KeyType = keyType.ParameterKey;
            hsmRequest.ZmkUnderLmk = request.ZmkUnderLmk;
            hsmRequest.KeyUnderZmk = request.KeyUnderZmk;
            hsmRequest.KeySchema = request.KeySchema;
            hsmRequest.AtallaVariant = request.AtallaVariant;
            hsmRequest.LmkIdentifier = request.LmkIdentifier;

            var hsmResponse = HsmWebApiClient.ImportKey(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.KeyUnderLmk = hsmResponse.KeyUnderLmk;
            response.KeyKcv = hsmResponse.KeyKcv;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}