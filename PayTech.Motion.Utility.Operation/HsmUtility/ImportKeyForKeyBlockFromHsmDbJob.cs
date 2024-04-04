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
    public class ImportKeyForKeyBlockFromHsmRequest
    {
        public string KeyUsage { get; set; }
        public string ZmkUnderLmk { get; set; }
        public string KeyUnderZmk { get; set; }
    }

    public class ImportKeyForKeyBlockFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string KeyUnderLmk { get; set; }
        public string KeyKcv { get; set; }
    }

    public class ImportKeyForKeyBlockFromHsmDbJob : BaseDbOperation<ImportKeyForKeyBlockFromHsmRequest, ImportKeyForKeyBlockFromHsmResponse>
    {
        public ImportKeyForKeyBlockFromHsmResponse response = new();

        public override ImportKeyForKeyBlockFromHsmResponse OnProcess(ImportKeyForKeyBlockFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            ImportKeyForKeyBlockApiRequest hsmRequest = new ImportKeyForKeyBlockApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.ImportKeyForKeyBlock(hsmRequest);
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