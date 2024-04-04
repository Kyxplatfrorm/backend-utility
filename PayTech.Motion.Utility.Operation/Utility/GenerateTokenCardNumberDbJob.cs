using PayTech.Core;
using System;
using DapperMapper;
using PayTech.Motion.Core.Model;

namespace PayTech.Motion.Utility.Operation
{
    public class GenerateTokenCardNumberRequest
    {
        public string CardNumber { get; set; }
    }

    public class GenerateTokenCardNumberResponse : ServiceResponse
    {
        public string CardTokenNumber { get; set; }
    }

    public class GenerateTokenCardNumberDbJob : BaseDbOperation<GenerateTokenCardNumberRequest, GenerateTokenCardNumberResponse>
    {
        GenerateTokenCardNumberResponse response = new();

        public GenerateTokenCardNumberDbJob() : base(false, "TokenConnection")
        {

        }

        public override GenerateTokenCardNumberResponse OnProcess(GenerateTokenCardNumberRequest request)
        {
            if (request == null)
                return response.ResultError("InvalidRequest", "Invalid Request");
            if(string.IsNullOrEmpty(request.CardNumber))
                return response.ResultError("InvalidCardNumber", "Invalid Card Number");

            request.CardNumber = request.CardNumber.Replace(" ", "").Trim();

            if (request.CardNumber.Length < 13)
                return response.ResultError("InvalidCardNumber", "Card Number Length Must Be Greater Then 12");

            long cardNumber = 0;
            bool isLong = long.TryParse(request.CardNumber, out cardNumber);
            if(isLong == false)
                return response.ResultError("InvalidCardNumber", "Card Number Must Be Numeric");

            response.CardTokenNumber = dbContext.Token(PaySession.TenantId, request.CardNumber, 6).TokenCardNumber;

            return response;
        }
    }
}