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
    public class ExportKeyFromHsmRequest
    {
        public long KeyTypeId { get; set; }
        public string KeyExportSchema { get; set; }
        public string ZmkUnderLmk { get; set; }
        public string KeyUnderLmk { get; set; }
    }

    public class ExportKeyFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string KeyUnderZmk { get; set; }
        public string KeyKcv { get; set; }
    }

    public class ExportKeyFromHsmDbJob : BaseDbOperation<ExportKeyFromHsmRequest, ExportKeyFromHsmResponse>
    {
        public ExportKeyFromHsmResponse response = new();

        public override ExportKeyFromHsmResponse OnProcess(ExportKeyFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            var keyType = dbContext.FindParameter("HsmImportExportKeyType", request.KeyTypeId);
            if (keyType == null)
                return response.ResultError("InvalidHsmImportExportKeyType", "Invalid Key Type");

            ExportKeyApiRequest hsmRequest = new ExportKeyApiRequest();
            hsmRequest.KeyType = keyType.ParameterKey;
            hsmRequest.ZmkUnderLmk = request.ZmkUnderLmk;
            hsmRequest.KeyUnderLmk = request.KeyUnderLmk;
            hsmRequest.KeyExportSchema = request.KeyExportSchema;

            var hsmResponse = HsmWebApiClient.ExportKey(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.KeyUnderZmk = hsmResponse.KeyUnderZmk;
            response.KeyKcv = hsmResponse.KeyKcv;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}