using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web;
using System.Web.Mvc;

namespace OnePage.Framework.View
{
    public class JsonNetResult : JsonResult
    {
        public bool PrevnetHijacking { get; }

        public JsonNetResult() : this(true)
        {
        }

        public JsonNetResult(bool prevnetHijacking)
        {
            PrevnetHijacking = prevnetHijacking;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCaseExceptDictionaryKeysResolver()
            };

            // If you need special handling, you can call another form of SerializeObject below            
            string serializedObject = JsonConvert.SerializeObject(Data, Formatting.None, jsonSerializerSettings);

            //Add ')]}'\n' for JSON hijacking prevent            
            if (PrevnetHijacking)
                serializedObject = string.Concat(")]}'\n", serializedObject);

            response.Write(serializedObject);
        }
    }

    internal class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

            contract.DictionaryKeyResolver = propertyName => propertyName;

            return contract;
        }
    }
}