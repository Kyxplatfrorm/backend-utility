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
    [Route("motion/utilityapi/v{version:apiVersion}/Utility/[action]")]
    public class UtilityController : Controller
    {
        [HttpPost]
        public InterestCalculationResponse InterestCalculation([FromBody] InterestCalculationRequest request)
        {
            InterestCalculationDbJob dbJob = new();
            return dbJob.Process(request);
        }


        [HttpGet]
        [PayAuthorizationFilter]
        public GetGenericParameterResponse GetEncryptionTypes()
        {
            return GetParameterUtility.GetParameterGroup("EncryptionType");
        }

        [HttpGet]
        [PayAuthorizationFilter]
        public GetGenericParameterResponse GetEncryptionModes()
        {
            return GetParameterUtility.GetParameterGroup("EncryptionMode");
        }

        [HttpGet]
        [PayAuthorizationFilter]
        public GetGenericParameterResponse GetDataConversionTypes()
        {
            return GetParameterUtility.GetParameterGroup("DataConversionType");
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public EncryptDataResponse EncryptData([FromBody] EncryptDataRequest request)
        {
            EncryptDataDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateTokenCardNumberResponse TokenizeCardNumber([FromBody] GenerateTokenCardNumberRequest request)
        {
            GenerateTokenCardNumberDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public ExOrResponse ExOr([FromBody] ExOrRequest request)
        {
            ExOrDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public TripleDesOperationResponse TripleDes([FromBody] TripleDesOperationRequest request)
        {
            TripleDesOperationDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public AesOperationResponse Aes([FromBody] AesOperationRequest request)
        {
            AesOperationDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public GenerateKcvResponse GenerateKcv([FromBody] GenerateKcvRequest request)
        {
            GenerateKcvDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter]
        public DataConversionResponse DataConversion([FromBody] DataConversionRequest request)
        {
            DataConversionDbJob dbJob = new();
            return dbJob.Process(request);
        }
    }
}