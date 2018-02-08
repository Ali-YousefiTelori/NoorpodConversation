using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoorpodConversation.Web.Controllers
{
    public class UploaderController : Controller
    {
        [HttpPost]
        // GET: Uploader
        public ActionResult LiveStreamUpload()
        {
            try
            {
                Request.GetBufferlessInputStream();
            }
            catch (Exception ex)
            {

            }
            return Content("End");
        }
        [HttpGet]
        public ActionResult GetLastError()
        {
            if (string.IsNullOrEmpty(MvcApplication.LastError))
                return Content("is empty");
            return Content(MvcApplication.LastError);
        }
    }
}