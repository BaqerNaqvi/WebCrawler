using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Utilities;
using WebCrawler.DataCore;
using WebCrawler.DataCore.Managers;
using Excel = Microsoft.Office.Interop.Excel;
namespace WebCrawler.Controllers
{
    public class CategoryController : ApiController
    {

        private Excel.Application _app;
        string File_name = "C:\\Users/Xeven/Documents/sheets/MySheet2.xlsx";
        string resultInfo = "";
        Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
        Microsoft.Office.Interop.Excel.Workbook oWB;
        Microsoft.Office.Interop.Excel.Worksheet oSheet;



        private static BusinessCoreController controller = new BusinessCoreController();
        public int Get()
        {


            return 0;
        }


        // POST api/<controller>
        public string Post([FromBody]admin admin)
        {

           return "";
        }
        [HttpPost]
        public HttpResponseMessage AddSubCategory([FromBody]JObject data)
        {
            string results = "";
            SubCategory info = new SubCategory();

            SubCategory categoryObj = new SubCategory();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<SubCategory>(results);
                categoryObj = controller.AddSubCategory(info);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle it
                    }
                }
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonCategoryResponse jsObj = new JsonCategoryResponse();
            if (categoryObj.Id != 0)
            {
                jsObj.status = "Ok";
                jsObj.result = categoryObj;
                jsObj.error_message = "Successfully Added !";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.error_message = "Failed. Something went wrong ";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        public HttpResponseMessage RemoveSubCategory([FromBody]JObject data)
        {
            string results = "";
            JsonInputData info = new JsonInputData();

            List<SubCategory> categoryObj = new List<SubCategory>();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonInputData>(results);
                categoryObj = controller.RemoveSubCategory(int.Parse(info.categoryId.ToString()), int.Parse(info.subCategoryId.ToString()));
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle it
                    }
                }
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonCategoryListResponse jsObj = new JsonCategoryListResponse();
            jsObj.status = "Ok";
            jsObj.result = categoryObj;
            jsObj.error_message = "Successfully Removed !";

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
            return response;

        }


        public HttpResponseMessage AllCategories([FromBody]JObject data)
        {
            List<AppCategory> categoryObj = new List<AppCategory>();

            categoryObj = controller.GetAppCategories();

            JsonCategoryListResponse1 jsObj = new JsonCategoryListResponse1();
            if (categoryObj.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = categoryObj;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.result = categoryObj;
                jsObj.error_message = "No Category in System!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            

        }



        [HttpPost]
        public HttpResponseMessage CleanDataSheet([FromBody]JObject data)
        {

                string respons = SearchText();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, respons);
                return response;
            

        }

        private string SearchText()
        {
            
            try
            {
                _app = new Excel.Application();
                object missing = System.Reflection.Missing.Value;
                oWB = oXL.Workbooks.Open(File_name, missing, missing, missing, missing,
                    missing, missing, missing, missing, missing, missing,
                    missing, missing, missing, missing);
                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets[1];

                for (int i = 0; i < 10; i++)
                {
                    Microsoft.Office.Interop.Excel.Range oRng = GetSpecifiedRange("HUSSAIN", oSheet);
                    if (oRng != null)
                    {
                        resultInfo = "Text found, position is Row:" + oRng.Row + " and column:" + oRng.Column;

                        string myRow = "A" + oRng.Row + ":I" + oRng.Row + "";
                        Excel.Range range = oSheet.get_Range(myRow, Type.Missing);
                        range.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                        NAR(range);
                        NAR(oRng);
                        

                    }
                    else
                    {
                        resultInfo = "Text is not found";
                    }
                }

                NAR(oSheet);
                CloseExcelWorkbook();
                NAR(oWB);
                _app.Quit();
                NAR(_app);

                return resultInfo;
            }
            catch (Exception ex)
            {
                return resultInfo;
            }
        }
        protected void CloseExcelWorkbook()
        {
            oWB.Save();
            oWB.Close(false, Type.Missing, Type.Missing);
        }
        protected void NAR(object o)
        {

            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }

            finally
            {
                o = null;
            }
        }
        private Microsoft.Office.Interop.Excel.Range GetSpecifiedRange(string matchStr, Microsoft.Office.Interop.Excel.Worksheet objWs)
        {
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Range currentFind = null;
            Microsoft.Office.Interop.Excel.Range firstFind = null;
            currentFind = objWs.get_Range("A1", "AM1000").Find(matchStr, missing,
                           Microsoft.Office.Interop.Excel.XlFindLookIn.xlValues,
                           Microsoft.Office.Interop.Excel.XlLookAt.xlPart,
                           Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows,
                           Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext, false, missing, missing);
            return currentFind;
        }

    }

    public class JsonCategoryResponse
    {
        public string status { get; set; }
        public SubCategory result { get; set; }
        public string error_message { get; set; }

    }

    public class JsonCategoryListResponse
    {
        public string status { get; set; }
        public List<SubCategory> result { get; set; }
        public string error_message { get; set; }

    }

    public class JsonCategoryListResponse1
    {
        public string status { get; set; }
        public List<AppCategory> result { get; set; }
        public string error_message { get; set; }

    }

    public class JsonInputData
    {
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }

    }
}