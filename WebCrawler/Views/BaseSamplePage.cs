using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebCrawler.Models.PayPal;

namespace WebCrawler.Views
{
    public abstract class BaseSamplePage : System.Web.UI.Page
    {
        protected RequestFlow flow;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // the code that only needs to run once goes here
                // Call this so the request/response flow is recorded and displayed properly.
                this.RegisterSampleRequestFlow();
                try
                {
                    this.RunSample();
                }
                catch (Exception ex)
                {
                    this.flow.RecordException(ex);
                }
                // Server.Transfer("~/Response.aspx");
            }

        }

        /// <summary>
        /// Primary method where each sample page should run their sample code.
        /// </summary>
        protected abstract void RunSample();

        protected void RegisterSampleRequestFlow()
        {
            if (this.flow == null)
            {
                this.flow = new RequestFlow();
            }
            HttpContext.Current.Items["Flow"] = this.flow;
        }
    }
}
