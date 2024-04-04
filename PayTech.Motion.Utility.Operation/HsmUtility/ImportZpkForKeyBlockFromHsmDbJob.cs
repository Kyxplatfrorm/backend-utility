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
    public class ImportZpkForKeyBlockFromHsmRequest
    {
        public string ZmkUnderLmk { get; set; }
        public string ZpkUnderZmk { get; set; }
    }

    public class ImportZpkForKeyBlockFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string KeyUnderLmk { get; set; }
        public string KeyKcv { get; set; }
    }

    public class ImportZpkForKeyBlockFromHsmDbJob : BaseDbOperation<ImportZpkForKeyBlockFromHsmRequest, ImportZpkForKeyBlockFromHsmResponse>
    {
        public ImportZpkForKeyBlockFromHsmResponse response = new();

        public override ImportZpkForKeyBlockFromHsmResponse OnProcess(ImportZpkForKeyBlockFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            ImportZpkForKeyBlockApiRequest hsmRequest = new ImportZpkForKeyBlockApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.ImportZpkForKeyBlock(hsmRequest);
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
