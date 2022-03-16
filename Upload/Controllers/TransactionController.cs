using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upload.DB.Manager;
using Upload.Models;

namespace Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        /*
         For this method can use multiple parameters, here's parameter 'Type' value sample 
         Type { 0 = Status,1 = CurrencyCode }
         if Status will come with text value like "Approved","Failed" etc , can use this getTransaction method
         */
        [HttpGet("getTransaction")]
        public IActionResult getTransaction(int Type,string Value)
        {
            List<OutPutModel> data = new List<OutPutModel>();
            DataModelManager dm = new DataModelManager();
            data = dm.getTransaction(Type, Value);
            return Ok(data);
        }

        [HttpGet("getTransactionbyStatus")]
        public IActionResult getTransactionbyStatus(int Status)
        {
            List<OutPutModel> rData = new List<OutPutModel>();
            DataModelManager dm = new DataModelManager();
            rData = dm.getTransactionbyStatus(Status);
            return Ok(rData);
        }

        [HttpGet("getTransactionbyCurrencyCode")]
        public IActionResult getTransactionbyCurrencyCode(string CurrencyCode)
        {
            List<OutPutModel> data = new List<OutPutModel>();
            DataModelManager dm = new DataModelManager();
            data = dm.getTransactionbyCurrencyCode(CurrencyCode);
            return Ok(data);
        }

        [HttpGet("getTransactionbyDate")]
        public IActionResult getTransactionbyDate(DateTime FromDate,DateTime ToDate)
        {
            List<OutPutModel> data = new List<OutPutModel>();
            DataModelManager dm = new DataModelManager();
            data = dm.getTransactionbyDate(FromDate, ToDate);
            return Ok(data);
        }
    }
}
