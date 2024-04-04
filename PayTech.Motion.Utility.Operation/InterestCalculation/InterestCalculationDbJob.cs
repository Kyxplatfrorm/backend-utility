using PayTech.Core;
using System;
using DapperMapper;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PayTech.Motion.Model;

namespace PayTech.Motion.Utility.Operation
{
    public class InterestCalculationRequest
    {
        public decimal TnaAmount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal BsmvRate { get; set; }
        public decimal KkdfRate { get; set; }
        public int InstallCount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class InterestCalculationResponse : ServiceResponse
    {
        public decimal TotalTnaAmount { get; set; }
        public decimal TotalInterestAmount { get; set; }
        public List<InstallmentItem> InstallmentList { get; set; }
    }

    public class InstallmentItem
    {
        public int InstallmentIndex { get; set; }
        public DateTime InstallmentDate { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal CapitalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal KkdfAmount { get; set; }
        public decimal BsmvAmount { get; set; }
        public decimal RemainingCapitalAmount { get; set; }
    }

    public class InterestCalculationDbJob : BaseDbOperation<InterestCalculationRequest, InterestCalculationResponse>
    {
        public InterestCalculationResponse response = new();

        public override InterestCalculationResponse OnProcess(InterestCalculationRequest request)
        {
            var fractionAmount = CalculateFraction(request);
            var installmentItems = CalculateInterestRate(request, fractionAmount);
            response.InstallmentList = installmentItems;
            response.TotalInterestAmount = installmentItems.Sum(x => x.InterestAmount + x.BsmvAmount + x.KkdfAmount);
            response.TotalTnaAmount = installmentItems.Sum(x => x.InstallmentAmount);
            return response;
        }

        decimal CalculateFraction(InterestCalculationRequest request)
        {
            decimal totalInterestRate = (request.InterestRate / 100) + ((request.InterestRate * (request.BsmvRate + request.KkdfRate)) / 100);
            decimal monthlyInterestRate = (decimal)Math.Pow(1 + (double)totalInterestRate, request.InstallCount);
            int totalDayDiff = request.DueDate.Subtract(request.TransactionDate).Days + 1;
            decimal installmentAmount = Math.Round((monthlyInterestRate * totalInterestRate / (monthlyInterestRate - 1)) * request.TnaAmount, 2, MidpointRounding.ToZero);

            decimal firstInstallmentItemInterestAmount = Math.Round((((request.TnaAmount * totalDayDiff) / 3000) * request.InterestRate), 2);
            decimal firstInstallmentItemInterestBsmvAmount = Math.Round((firstInstallmentItemInterestAmount * request.BsmvRate), 2);
            decimal firstInstallmentItemInterestKkdfAmount = Math.Round((firstInstallmentItemInterestAmount * request.KkdfRate), 2);
            decimal firstInstallmentAmount = installmentAmount;

            decimal interestAmount = 0;
            decimal interestBsmvAmount = 0;
            decimal interestKkdfAmount = 0;

            if (totalDayDiff <= 30)
            {
                interestAmount = CalculateDailyInterest(request, 30 - totalDayDiff);
                interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
                interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);
                firstInstallmentAmount = firstInstallmentAmount - (interestAmount + interestBsmvAmount + interestKkdfAmount);
            }
            else
            {
                interestAmount = CalculateDailyInterest(request, totalDayDiff - 30);
                interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
                interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);
                firstInstallmentAmount = firstInstallmentAmount + (interestAmount + interestBsmvAmount + interestKkdfAmount);
            }

            interestAmount = CalculateDailyInterest(request, totalDayDiff);
            interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
            interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);

            decimal totalInterestAndBsmvKkdfAmount = interestAmount + interestBsmvAmount + interestKkdfAmount;
            
            List<InstallmentItem> installmentItems = new List<InstallmentItem>();
            InstallmentItem fistInstallmentItem = new InstallmentItem();

            fistInstallmentItem.InstallmentIndex = 1;
            fistInstallmentItem.InstallmentDate = request.TransactionDate;
            fistInstallmentItem.InstallmentAmount = firstInstallmentAmount;
            fistInstallmentItem.CapitalAmount = firstInstallmentAmount - totalInterestAndBsmvKkdfAmount;
            fistInstallmentItem.InterestAmount = interestAmount;
            fistInstallmentItem.KkdfAmount = interestBsmvAmount;
            fistInstallmentItem.BsmvAmount = interestKkdfAmount;
            fistInstallmentItem.RemainingCapitalAmount = request.TnaAmount - fistInstallmentItem.CapitalAmount;

            installmentItems.Add(fistInstallmentItem);

            for (int i = 1; i < request.InstallCount; i++)
            {
                //buradayim
                InstallmentItem installmentItem = new InstallmentItem();

                installmentItem.InstallmentIndex = i + 1;
                installmentItem.InstallmentDate = request.TransactionDate.AddMonths(i);
                installmentItem.InstallmentAmount = installmentAmount;

                decimal prevRemainingCapitalAmount = installmentItems[i - 1].RemainingCapitalAmount;

                interestAmount = Math.Round((prevRemainingCapitalAmount * request.InterestRate) / 100, 2);
                interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
                interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);

                totalInterestAndBsmvKkdfAmount = interestAmount + interestBsmvAmount + interestKkdfAmount;

                installmentItem.CapitalAmount = installmentAmount - totalInterestAndBsmvKkdfAmount;
                installmentItem.InterestAmount = interestAmount;
                installmentItem.KkdfAmount = interestBsmvAmount;
                installmentItem.BsmvAmount = interestKkdfAmount;

                installmentItem.RemainingCapitalAmount = prevRemainingCapitalAmount - installmentItem.CapitalAmount;

                installmentItems.Add(installmentItem);
            }

            var lastInstallmentItem = installmentItems[request.InstallCount - 1];

            return lastInstallmentItem.RemainingCapitalAmount;
        }

        List<InstallmentItem> CalculateInterestRate(InterestCalculationRequest request,decimal fractionAmount)
        {
            decimal totalInterestRate = (request.InterestRate / 100) + ((request.InterestRate * (request.BsmvRate + request.KkdfRate)) / 100);
            decimal monthlyInterestRate = (decimal)Math.Pow(1 + (double)totalInterestRate, request.InstallCount);
            int totalDayDiff = request.DueDate.Subtract(request.TransactionDate).Days+1;
            decimal installmentAmount = Math.Round((monthlyInterestRate * totalInterestRate / (monthlyInterestRate - 1)) * request.TnaAmount, 2,MidpointRounding.ToZero);

            decimal firstInstallmentItemInterestAmount = Math.Round((((request.TnaAmount * totalDayDiff) / 3000) * request.InterestRate), 2);
            decimal firstInstallmentItemInterestBsmvAmount = Math.Round((firstInstallmentItemInterestAmount * request.BsmvRate), 2);
            decimal firstInstallmentItemInterestKkdfAmount = Math.Round((firstInstallmentItemInterestAmount * request.KkdfRate), 2);
            decimal firstInstallmentAmount = installmentAmount;

            decimal interestAmount = 0;
            decimal interestBsmvAmount = 0;
            decimal interestKkdfAmount = 0;

            if (totalDayDiff <= 30)
            {
                interestAmount = CalculateDailyInterest(request,30 - totalDayDiff);
                interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
                interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);
                firstInstallmentAmount = firstInstallmentAmount - (interestAmount + interestBsmvAmount + interestKkdfAmount);
            }
            else
            {
                interestAmount = CalculateDailyInterest(request, totalDayDiff-30);
                interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
                interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);
                firstInstallmentAmount = firstInstallmentAmount + (interestAmount + interestBsmvAmount + interestKkdfAmount);
            }

            interestAmount = CalculateDailyInterest(request, totalDayDiff);
            interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
            interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);

            decimal totalInterestAndBsmvKkdfAmount = interestAmount + interestBsmvAmount + interestKkdfAmount;

            firstInstallmentAmount += fractionAmount;

            List<InstallmentItem> installmentItems = new List<InstallmentItem>();
            InstallmentItem fistInstallmentItem = new InstallmentItem();

            fistInstallmentItem.InstallmentIndex = 1;
            fistInstallmentItem.InstallmentDate = request.TransactionDate;
            fistInstallmentItem.InstallmentAmount = firstInstallmentAmount;
            fistInstallmentItem.CapitalAmount = firstInstallmentAmount - totalInterestAndBsmvKkdfAmount;
            fistInstallmentItem.InterestAmount = interestAmount;
            fistInstallmentItem.KkdfAmount = interestBsmvAmount;
            fistInstallmentItem.BsmvAmount = interestKkdfAmount;
            fistInstallmentItem.RemainingCapitalAmount = request.TnaAmount - fistInstallmentItem.CapitalAmount;

            installmentItems.Add(fistInstallmentItem);

            for (int i = 1; i < request.InstallCount; i++)
            {
                InstallmentItem installmentItem = new InstallmentItem();

                installmentItem.InstallmentIndex = i+1;
                installmentItem.InstallmentDate = request.TransactionDate.AddMonths(i);
                installmentItem.InstallmentAmount = installmentAmount;

                decimal prevRemainingCapitalAmount = installmentItems[i - 1].RemainingCapitalAmount;

                interestAmount = Math.Round((prevRemainingCapitalAmount * request.InterestRate) / 100, 2);
                interestBsmvAmount = Math.Round((interestAmount * request.BsmvRate), 2);
                interestKkdfAmount = Math.Round((interestAmount * request.KkdfRate), 2);

                totalInterestAndBsmvKkdfAmount = interestAmount + interestBsmvAmount + interestKkdfAmount;

                installmentItem.CapitalAmount = installmentAmount - totalInterestAndBsmvKkdfAmount;
                installmentItem.InterestAmount = interestAmount;
                installmentItem.KkdfAmount = interestBsmvAmount;
                installmentItem.BsmvAmount = interestKkdfAmount;

                installmentItem.RemainingCapitalAmount = prevRemainingCapitalAmount - installmentItem.CapitalAmount;

                installmentItems.Add(installmentItem);
            }

            var lastInstallmentItem = installmentItems[request.InstallCount - 1];

            decimal lastRemAmount = lastInstallmentItem.RemainingCapitalAmount;
            installmentItems[0].InstallmentAmount += lastRemAmount;
            lastInstallmentItem.RemainingCapitalAmount = 0;

            return installmentItems;
        }

        decimal CalculateDailyInterest(InterestCalculationRequest request,int totalDay)
        {
            return Math.Round(((request.TnaAmount * totalDay) / 3000) * request.InterestRate, 2);
        }
    }
}