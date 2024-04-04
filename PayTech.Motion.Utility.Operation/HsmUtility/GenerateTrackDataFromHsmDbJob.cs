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
    public class GenerateTrackDataFromHsmRequest
    {
        public string CvkUnderLmk { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDateYYMM { get; set; }
        public string ServiceCode { get; set; }
        public string EmbossName { get; set; }
        public string PvvKeyIndex { get; set; }
        public string Pvv { get; set; }
        public int CvvOffset { get; set; }
    }

    public class GenerateTrackDataFromHsmResponse : ServiceResponse
    {
        public string HsmErrorCode { get; set; }
        public string HsmErrorDescription { get; set; }
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string Track2Chip { get; set; }
        public string Cvv { get; set; }
        public string ICvv { get; set; }
        public string Cvv2 { get; set; }
    }

    public class GenerateTrackDataFromHsmDbJob : BaseDbOperation<GenerateTrackDataFromHsmRequest, GenerateTrackDataFromHsmResponse>
    {
        public GenerateTrackDataFromHsmResponse response = new();

        public override GenerateTrackDataFromHsmResponse OnProcess(GenerateTrackDataFromHsmRequest request)
        {
            if (request == null)
                return response.ResultError("RequestNull", "Request Can Not Be null");

            GenerateTrackDataApiRequest hsmRequest = new GenerateTrackDataApiRequest();
            PayClassUtility.ClassCopyShallow(hsmRequest, request);

            var hsmResponse = HsmWebApiClient.GenerateTrackData(hsmRequest);
            if (hsmResponse != null)
            {
                response.HsmErrorCode = hsmResponse.ErrorCode;
                response.HsmErrorDescription = hsmResponse.ErrorDescription;
            }

            if (hsmResponse.IsSucceeded == false)
                return response;

            response.Track1 = hsmResponse.Track1;
            response.Track2 = hsmResponse.Track2;
            response.Track2Chip = hsmResponse.Track2Chip;
            response.Cvv = hsmResponse.Cvv;
            response.ICvv = hsmResponse.ICvv;
            response.Cvv2 = hsmResponse.Cvv2;

            response.HsmErrorCode = hsmResponse.ErrorCode;
            response.HsmErrorDescription = hsmResponse.ErrorDescription;

            return response;
        }
    }
}