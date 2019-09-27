using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Data.Storage;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;

namespace Recaptcha.Forms
{
    public class Recaptcha3 : FieldType
    {
        public Recaptcha3()
        {
            this.Id = new Guid("08b8057f-06c9-4ca5-8a42-fd1fc2a46eff");
            this.Name = "Recaptcha3";
            this.Description = "Render a custom text field.";
            this.Icon = "icon-eye";
            this.DataType = FieldDataType.String;
            this.SortOrder = 10;
            this.SupportsRegex = true;
        }

        [Setting("Additional Settings", PreValues = "homepage,login,social,e-commerce", Description = "Use case", View = "Dropdownlist")]
        public string AdditionalSettings { get; set; }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContextBase context, IFormStorage formStorage)
        {
            var returnStrings = new List<string>();
            var token = HttpContext.Current.Request["g-recaptcha-response"];
            string secret = Configuration.GetSetting("RecaptchaPrivateKey");
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, token));
            var obj = JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult);

            if (!obj.Success)
            {
                returnStrings.Add("You need to include 'custom' in the field!");
            }

            return returnStrings;
        }
    }

    public class CaptchaResponse
    {
        public bool Success { set; get; }
    }
}