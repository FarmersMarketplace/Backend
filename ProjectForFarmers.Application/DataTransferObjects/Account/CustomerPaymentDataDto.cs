﻿namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class CustomerPaymentDataDto
    {
        public string CardNumber { get; set; }
        public string CardExpirationYear { get; set; }
        public string CardExpirationMonth { get; set; }
        public string CVV { get; set; }
    }
}