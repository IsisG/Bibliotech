using Bibliotech.Models;
using Bibliotech.Repository;
using NReco.ImageGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.QrCode;

namespace Bibliotech.Base
{
    public static class Functions
    {
        public static bool IsNull(object obj)
        {
            if (obj == null)
                return true;
            return !obj.GetType().GetProperties().Any(p => p.GetValue(obj) != null);
        }

        public static bool IsNullExcludingProperties(object obj, params string[] properties)
        {
            if (obj == null)
                return true;
            return !obj.GetType().GetProperties().Any(p => p.GetValue(obj) != null && !properties.Contains(p.Name));
        }

        public static MemoryStream GenerateQRCode(string content, int width = -1, int height = -1)
        {
            IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };

            if (width > 0 && height > 0)
                writer.Options = new QrCodeEncodingOptions { Width = width, Height = height };

            var result = writer.Write(content);

            MemoryStream memoryStream = new MemoryStream();
            result.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            return memoryStream;
        }

        public static MemoryStream ConvertHtmlToImage(string htmlContent, string format, string arguments = null)
        {
            HtmlToImageConverter htmlConverter = new HtmlToImageConverter();

            if (arguments != null)
                htmlConverter.CustomArgs = arguments;

            var imgBytes = htmlConverter.GenerateImage(htmlContent, format);
            return new MemoryStream(imgBytes);
        }

        public static string GetPngImageSrc(MemoryStream memoryStream)
        {
            return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);
        }

        public static Usuario GetCurrentUser()
        {
            return UsuarioRepository.Instance.GetById(new Guid(System.Web.HttpContext.Current.User.Identity.Name));
        }

        public static string GetEnumDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;

                        break;
                    }
                }
            }

            return description;
        }
    }
}