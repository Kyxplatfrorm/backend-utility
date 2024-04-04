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
    public class GenerateKeyFromHsmRequest
    {
        public long KeyTypeId { get; set; }
        public string KeySchema { get; set; }
        public bool? ExportKey { get; set; }

        public long? ZmkTmkTypeId { get; set; }
        public string EncryptedMasterKey { get; set; }
        public string KeyExportSchema { get; set; }
        public string AtallaVariant { get; set; }
        public string Exportability { get; set; }
        public string KeyBlockFormat { get; set; }
    }

    public class GenerateKeyFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string KeyUnderLmk { get; set; }
        public string KeyUnderZmk { get; set; }
        public string KeyCheckValue { get; set; }
    }

    public class GenerateKeyFromHsmDbJob : BaseDbOperation<GenerateKeyFromHsmRequest, GenerateKeyFromHsmResponse>
    {
        public GenerateKeyFromHsmResponse response = new();

        public override GenerateKeyFromHsmResponse OnProcess(GenerateKeyFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            var keyType = dbContext.FindParameter("HsmImportExportKeyType", request.KeyTypeId);
            if (keyType == null)
                return response.ResultError("InvalidHsmImportExportKeyType", "Invalid Key Type");

            bool isExport = request.ExportKey ?? false;
            int zmkTmkTypeParam = 0;
            if (isExport)
            {
                long zmkTmkTypeId = request.ZmkTmkTypeId ?? 0;
                var zmkTmkType = dbContext.FindParameter("HsmKeyZmkTmkType", zmkTmkTypeId);
                if (zmkTmkType == null)
                    return response.ResultError("InvalidHsmKeyZmkTmkType", "Invalid Zmk Tmk Type");

                zmkTmkTypeParam = int.Parse(zmkTmkType.ParameterKey);
            }

            int mode = isExport ? 1 : 0;

            var hsmResponse = HsmWebApiClient.GenerateKey(mode, keyType.ParameterKey, request.KeySchema, zmkTmkTypeParam, request.EncryptedMasterKey, request.KeyExportSchema, request.AtallaVariant,request.Exportability,request.KeyBlockFormat);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.KeyUnderLmk = hsmResponse.KeyUnderLmk;
            response.KeyUnderZmk = hsmResponse.KeyUnderZmk;
            response.KeyCheckValue = hsmResponse.KeyCheckValue;
            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}