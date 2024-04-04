using Microsoft.AspNetCore.Mvc;
using PayTech.Core;
using PayTech.Motion.Utility.Operation;
using PayTech.Motion.Core.Operation;
using System.Diagnostics.CodeAnalysis;

namespace PayTech.Motion.Admin.WebApi
{
    [SuppressMessage("Microsoft.Performance", "CA1822")]
    [ApiVersionNeutral]
    [ApiVersion("1.0")]
    [Route("motion/utilityapi/v{version:apiVersion}/HsmUtility/[action]")]
    public class HsmUtilityController : Controller
    {
        [HttpGet]
        [PayAuthorizationFilter]
        public GetGenericParameterResponse GetHsmKeyZmkTmkType()
        {
            GetGenericParameterRequest getGenericParameterRequest = new GetGenericParameterRequest();
            getGenericParameterRequest.GroupCode = "HsmKeyZmkTmkType";
            GetGenericParameterDbJob dbJob = new();
            return dbJob.Process(getGenericParameterRequest);
        }

        [HttpGet]
        [PayAuthorizationFilter]
        public GetGenericParameterResponse GetHsmImportExportKeyType()
        {
            GetGenericParameterRequest getGenericParameterRequest = new GetGenericParameterRequest();
            getGenericParameterRequest.GroupCode = "HsmImportExportKeyType";
            GetGenericParameterDbJob dbJob = new();
            return dbJob.Process(getGenericParameterRequest);
        }

        [HttpGet]
        [PayAuthorizationFilter]
        public GetGenericParameterResponse GetHsmKcvKeyType()
        {
            GetGenericParameterRequest getGenericParameterRequest = new GetGenericParameterRequest();
            getGenericParameterRequest.GroupCode = "HsmKcvKeyType";
            GetGenericParameterDbJob dbJob = new();
            return dbJob.Process(getGenericParameterRequest);
        }

        [HttpGet]
        [PayAuthorizationFilter]
        public GetGenericParameterResponse GetHsmKeyLengthFlagType()
        {
            GetGenericParameterRequest getGenericParameterRequest = new GetGenericParameterRequest();
            getGenericParameterRequest.GroupCode = "HsmKeyLengthFlagType";
            GetGenericParameterDbJob dbJob = new();
            return dbJob.Process(getGenericParameterRequest);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateCvvFromHsmResponse GenerateCvv([FromBody] GenerateCvvFromHsmRequest request)
        {
            GenerateCvvFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateKcvFromHsmResponse GenerateKcv([FromBody] GenerateKcvFromHsmRequest request)
        {
            GenerateKcvFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateKeyFromHsmResponse GenerateKey([FromBody] GenerateKeyFromHsmRequest request)
        {
            GenerateKeyFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public EncryptDataFromHsmResponse EncryptData([FromBody] EncryptDataFromHsmRequest request)
        {
            EncryptDataFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public DecryptDataFromHsmResponse DecryptData([FromBody] DecryptDataFromHsmRequest request)
        {
            DecryptDataFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public ImportKeyFromHsmResponse ImportKey([FromBody] ImportKeyFromHsmRequest request)
        {
            ImportKeyFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public ExportKeyFromHsmResponse ExportKey([FromBody] ExportKeyFromHsmRequest request)
        {
            ExportKeyFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public NetworkStatusFromHsmResponse NetworkStatus([FromBody] NetworkStatusFromHsmRequest request)
        {
            NetworkStatusFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GeneratePinLmkFromHsmResponse GeneratePinLmk([FromBody] GeneratePinLmkFromHsmRequest request)
        {
            GeneratePinLmkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public SendRawMessageToHsmResponse SendRawMessage([FromBody] SendRawMessageToHsmRequest request)
        {
            SendRawMessageToHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateMacFromHsmResponse GenerateMac([FromBody] GenerateMacFromHsmRequest request)
        {
            GenerateMacFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GeneratePinChangeScriptFromHsmResponse GeneratePinChangeScript([FromBody] GeneratePinChangeScriptFromHsmRequest request)
        {
            GeneratePinChangeScriptFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GeneratePinChangeScriptForEmv4FromHsmResponse GeneratePinChangeScriptForEmv4([FromBody] GeneratePinChangeScriptForEmv4FromHsmRequest request)
        {
            GeneratePinChangeScriptForEmv4FromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GeneratePinUnBlockScriptFromHsmResponse GeneratePinUnBlockScript([FromBody] GeneratePinUnBlockScriptFromHsmRequest request)
        {
            GeneratePinUnBlockScriptFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GeneratePinUnBlockScriptEmv4FromHsmResponse GeneratePinUnBlockScriptEmv4([FromBody] GeneratePinUnBlockScriptEmv4FromHsmRequest request)
        {
            GeneratePinUnBlockScriptEmv4FromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GeneratePvvFromHsmResponse GeneratePvv([FromBody] GeneratePvvFromHsmRequest request)
        {
            GeneratePvvFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateTrackDataFromHsmResponse GenerateTrackData([FromBody] GenerateTrackDataFromHsmRequest request)
        {
            GenerateTrackDataFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateZpkFromHsmResponse GenerateZpk([FromBody] GenerateZpkFromHsmRequest request)
        {
            GenerateZpkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public ImportKeyForKeyBlockFromHsmResponse ImportKeyForKeyBlock([FromBody] ImportKeyForKeyBlockFromHsmRequest request)
        {
            ImportKeyForKeyBlockFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public ImportZpkForKeyBlockFromHsmResponse ImportZpkForKeyBlock([FromBody] ImportZpkForKeyBlockFromHsmRequest request)
        {
            ImportZpkForKeyBlockFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslateKeyFromOldLmkToNewLmkFromHsmResponse TranslateKeyFromOldLmkToNewLmk([FromBody] TranslateKeyFromOldLmkToNewLmkFromHsmRequest request)
        {
            TranslateKeyFromOldLmkToNewLmkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslateZmkFromOldLmkToNewLmkFromHsmResponse TranslateZmkFromOldLmkToNewLmk([FromBody] TranslateZmkFromOldLmkToNewLmkFromHsmRequest request)
        {
            TranslateZmkFromOldLmkToNewLmkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslateZpkFromOldLmkToNewLmkFromHsmResponse TranslateZpkFromOldLmkToNewLmk([FromBody] TranslateZpkFromOldLmkToNewLmkFromHsmRequest request)
        {
            TranslateZpkFromOldLmkToNewLmkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslateZpkLmkToZpkZmkFromHsmResponse TranslateZpkLmkToZpkZmk([FromBody] TranslateZpkLmkToZpkZmkFromHsmRequest request)
        {
            TranslateZpkLmkToZpkZmkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslateZpkZmkToZpkLmkFromHsmResponse TranslateZpkZmkToZpkLmk([FromBody] TranslateZpkZmkToZpkLmkFromHsmRequest request)
        {
            TranslateZpkZmkToZpkLmkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslatePinLmk2ZpkFromHsmResponse TranslatePinLmk2Zpk([FromBody] TranslatePinLmk2ZpkFromHsmRequest request)
        {
            TranslatePinLmk2ZpkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslatePinUnderZpk2LmkFromHsmResponse TranslatePinUnderZpk2Lmk([FromBody] TranslatePinUnderZpk2LmkFromHsmRequest request)
        {
            TranslatePinUnderZpk2LmkFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TranslatePinUnderZpkToZpk2FromHsmResponse TranslatePinUnderZpkToZpk2([FromBody] TranslatePinUnderZpkToZpk2FromHsmRequest request)
        {
            TranslatePinUnderZpkToZpk2FromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public VerifyArqcAndGenerateArpcFromHsmResponse VerifyArqcAndGenerateArpc([FromBody] VerifyArqcAndGenerateArpcFromHsmRequest request)
        {
            VerifyArqcAndGenerateArpcFromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public VerifyArqcAndGenerateArpcEmv4FromHsmResponse VerifyArqcAndGenerateArpcEmv4([FromBody] VerifyArqcAndGenerateArpcEmv4FromHsmRequest request)
        {
            VerifyArqcAndGenerateArpcEmv4FromHsmDbJob dbJob = new();
            return dbJob.Process(request);
        }
    }
}