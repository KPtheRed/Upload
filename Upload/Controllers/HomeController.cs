using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Upload.DB.Helper;
using Upload.DB.Manager;
using Upload.DB.Models;
using Upload.Models;
using System.Xml.Serialization;

namespace Upload.Controllers
{
    public class HomeController : Controller
    {
        [Obsolete]
        private IHostingEnvironment Environment;
        private readonly ILogger<HomeController> _logger;

        [Obsolete]
        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("FileUpload")]
        [Obsolete]
        public async Task<IActionResult> Index(IFormFile files,[FromServices] IHostingEnvironment hostingenv)
        {
            try
            {
                List<DataViewModel> data = new List<DataViewModel>();
                if (files != null)
                {

                    var ext = System.IO.Path.GetExtension(files.FileName);
                    string name = $"{hostingenv.WebRootPath}\\files\\{files.FileName}";
                    using (FileStream fileStream = System.IO.File.Create(name))
                    {
                        files.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    if (ext == ".csv")
                    {
                        data = this.GetdatafromCSV(name);
                    }
                    else
                    {
                        data = this.Getdatafromxml(name);
                    }
                }
                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<DataViewModel> Getdatafromxml(string name)
        {
            try
            {
                DataModelManager dmgr = new DataModelManager();
                List<DataViewModel> data = new List<DataViewModel>();
                Transactions trans;
                var serializer = new XmlSerializer(typeof(Transactions));
                using (var reader = new FileStream(name, FileMode.Open))
                {
                    trans = (Transactions)serializer.Deserialize(reader);
                }
                data = dmgr.savetoDBFromXml(trans);
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<DataViewModel> GetdatafromCSV(string filename)
        {
            try
            {
                DataModelManager dmgr = new DataModelManager();
                List<DataModel> data = new List<DataModel>();
                List<DataViewModel> dvms = new List<DataViewModel>();
                using (var reader = new StreamReader(filename))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var d = csv.GetRecord<DataViewModel>();
                        dvms.Add(d);
                    }

                    dvms = dmgr.savetoDB(dvms);
                }
                return dvms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
