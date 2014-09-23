﻿using MixERP.Net.Common.Models.Core;
using MixERP.Net.Common.Models.Transactions;
using MixERP.Net.WebControls.StockTransactionFactory.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.SessionState;

namespace MixERP.Net.Core.Modules.Purchase.Services
{
    /// <summary>
    /// Summary description for DirectPurchase
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class DirectPurchase : WebService
    {
        [WebMethod(EnableSession = true)]
        public long Save(DateTime valueDate, int storeId, string partyCode, string referenceNumber, string data, string statementReference, string transactionType, int cashRepositoryId, int costCenterId, string attachmentsJSON)
        {
            Collection<StockMasterDetailModel> details = CollectionHelper.GetStockMasterDetailCollection(data, storeId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Collection<AttachmentModel> attachments = js.Deserialize<Collection<AttachmentModel>>(attachmentsJSON);

            bool isCredit = !transactionType.ToLower().Equals("cash");

            if (isCredit && cashRepositoryId > 0)
            {
                throw new InvalidOperationException("Invalid cash repository specified in credit transaction.");
            }

            return Data.Helpers.DirectPurchase.Add(valueDate, storeId, isCredit, partyCode, details, cashRepositoryId, costCenterId, referenceNumber, statementReference, attachments);
        }
    }
}