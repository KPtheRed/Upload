using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Upload.DB.Helper;
using Upload.DB.Models;
using Upload.DB.Repositories;
using Upload.Models;

namespace Upload.DB.Manager
{
    public class DataModelManager
    {
        DataModelRepository dRepo;
        public DataModelManager()
        {
            dRepo = new DataModelRepository();
        }
        internal List<DataViewModel> savetoDB(List<DataViewModel> d)
        {
            List<DataViewModel> data = new List<DataViewModel>();
            List<DataModel> dms = new List<DataModel>();
            
            foreach(var a in d)
            {
                /*
                 I didn't clear the task ment, what kind of format provide in CSV for currency, but here is the code for text changing to 
                ISOCurrencySymbol
                 */
                string currency = "MMK";
                RegionInfo region = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(ct => new RegionInfo(ct.LCID))
                    .Where(ri => ri.ISOCurrencySymbol == currency).FirstOrDefault();
                /*
                 I didn't clear the task ment, what kind of format provide in CSV for currency, but here is the code for text changing to 
                ISOCurrencySymbol
                 */

                DataModel dm = new DataModel();
                dm = dRepo.Get().Where(x => x.TransactionIdentificator == a.TransactionIdentificator).FirstOrDefault();
                if (dm == null)
                {
                    dm = new DataModel();
                }
                dm.Amount = a.Amount;
                dm.CurrencyCode = a.CurrencyCode;
                dm.Status = (int)Enum.Parse(typeof(EnumClass.Status), a.Status);
                dm.TransactionDate = a.TransactionDate;
                dm.TransactionIdentificator = a.TransactionIdentificator;
                dm = dRepo.AddorUpdate(dm, dm.DataModelID);
                dms.Add(dm);
            }

            data = dms.Select(x => new DataViewModel {
                Amount = x.Amount,
                CurrencyCode = x.CurrencyCode,
                Status = ((EnumClass.Status)x.Status).ToString(),
                TransactionDate = x.TransactionDate,
                TransactionIdentificator = x.TransactionIdentificator
            }).ToList();



            return data;
        }

        internal List<OutPutModel> getTransactionbyDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<OutPutModel> data = new List<OutPutModel>();
                data = dRepo.Get().Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).AsEnumerable().Select(y => new OutPutModel
                {
                    id = y.TransactionIdentificator,
                    payment = y.Amount + " " + y.CurrencyCode,
                    Status = getStatus(y.Status)
                }).ToList();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal List<OutPutModel> getTransactionbyCurrencyCode(string currencyCode)
        {
            try
            {
                List<OutPutModel> rData = new List<OutPutModel>();
                rData = dRepo.Get().Where(x => x.CurrencyCode == currencyCode).AsEnumerable().Select(y => new OutPutModel
                {
                    id = y.TransactionIdentificator,
                    payment = y.Amount + " " + y.CurrencyCode,
                    Status = getStatus(y.Status)
                }).ToList();
                return rData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<OutPutModel> getTransactionbyStatus(int status)
        {
            try
            {
                List<DataModel> data = new List<DataModel>();
                List<OutPutModel> rData = new List<OutPutModel>();
                rData = dRepo.Get().Where(x => x.Status == status).AsEnumerable().Select(y => new OutPutModel
                {
                    id = y.TransactionIdentificator,
                    payment = y.Amount + " " + y.CurrencyCode,
                    Status = getStatus(y.Status)
                }).ToList();

                return rData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getStatus(int status)
        {
            string s = "";
            switch (status)
            {
                case (int)EnumClass.Status.Finished:
                    s = "D";
                    break;
                case (int)EnumClass.Status.Done:
                    s = "D";
                    break;
                case (int)EnumClass.Status.Approved:
                    s = "A";
                    break;
                case (int)EnumClass.Status.Failed:
                    s = "R";
                    break;
                case (int)EnumClass.Status.Rejected:
                    s = "R";
                    break;
                default: break;
            }
            return s;
        }

        internal List<OutPutModel> getTransaction(int type, string value)
        {
            try
            {
                List<OutPutModel> data = new List<OutPutModel>();
                if (type == 0)
                {
                    data = dRepo.Get().Where(x => x.Status == (int)Enum.Parse(typeof(EnumClass.Status), value)).AsEnumerable().Select(y => new OutPutModel
                    {
                        id = y.TransactionIdentificator,
                        payment = y.Amount + " " + y.CurrencyCode,
                        Status = getStatus(y.Status)
                    }).ToList();
                }
                else
                {
                    data = dRepo.Get().Where(x => x.CurrencyCode == value).AsEnumerable().Select(y => new OutPutModel
                    {
                        id = y.TransactionIdentificator,
                        payment = y.Amount + " " + y.CurrencyCode,
                        Status = getStatus(y.Status)
                    }).ToList();
                }
                
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<DataViewModel> savetoDBFromXml(Transactions trans)
        {
            try
            {
                List<DataViewModel> data = new List<DataViewModel>();
                List<DataModel> dms = new List<DataModel>();
                foreach (var a in trans)
                {
                    DataModel dm = new DataModel();

                    dm = dRepo.Get().Where(x => x.TransactionIdentificator == a.Transactionid).FirstOrDefault();
                    if(dm == null)
                    {
                        dm = new DataModel();
                    }

                    if (a.PaymentDetails != null)
                    {
                        dm.Amount = a.PaymentDetails.Amount.Equals(null) ? 0 : a.PaymentDetails.Amount;
                        dm.CurrencyCode = a.PaymentDetails.CurrencyCode.Equals(null) ? "" : a.PaymentDetails.CurrencyCode;
                    }
                    dm.CurrencyCode = a.PaymentDetails.CurrencyCode;
                    dm.TransactionDate = a.TransactionDate;
                    dm.TransactionIdentificator = a.Transactionid;
                    dm.Status = (int)Enum.Parse(typeof(EnumClass.Status), a.Status);
                    dm = dRepo.AddorUpdate(dm, dm.DataModelID);
                    dms.Add(dm);
                }

                data = dms.Select(x => new DataViewModel
                {
                    Amount = x.Amount,
                    CurrencyCode = x.CurrencyCode,
                    Status = ((EnumClass.Status)x.Status).ToString(),
                    TransactionDate = x.TransactionDate,
                    TransactionIdentificator = x.TransactionIdentificator
                }).ToList();

                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
