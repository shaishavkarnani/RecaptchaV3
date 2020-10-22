using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Data.Storage;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;

namespace Recaptcha.Forms
{
    public sealed class Recaptcha3 : FieldType
    {
        public Recaptcha3()
        {
            Id = new Guid("08b8057f-06c9-4ca5-8a42-fd1fc2a46eff");
            Name = "Recaptcha3";
            Description = "Render a Recaptcha v3.";
            Icon = "icon-eye";
            DataType = FieldDataType.String;
            SortOrder = 10;
            SupportsRegex = true;
            HideLabel = true;
        }

        [Setting("Additional Settings", PreValues = "homepage,login,social,e-commerce", Description = "Use case", View = "Dropdownlist")]
        public string AdditionalSettings { get; set; }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContextBase context, IFormStorage formStorage)
        {
            var returnStrings = new List<string>();
            var token = HttpContext.Current.Request["g-recaptcha-response"];
            var secret = Configuration.GetSetting("RecaptchaPrivateKey");
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, token));
            var obj = JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult);

            if (!obj.Success)
            {
                returnStrings.Add("Recaptcha Failed. Try again!");
            }

            return returnStrings;
        }
    }

    public class CaptchaResponse
    {
        public bool Success { set; get; }
    }
}