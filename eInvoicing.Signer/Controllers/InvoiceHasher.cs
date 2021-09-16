using eInvoicing.API.Filters;
using eInvoicing.DTO;
using eInvoicing.Service.Helper.Extension;
using eInvoicing.Signer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace eInvoicing.Signer.Controllers
{
    //[HMACAuthentication]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceHasher : ControllerBase
    {
        private readonly string DllLibPath;
        private readonly string TokenPin;
        private readonly IConfiguration _config;
        private readonly ILogger<InvoiceHasher> _logger;
        public InvoiceHasher(IConfiguration config, ILogger<InvoiceHasher> logger)
        {
            _logger = logger;
            _config = config;
            DllLibPath = _config.GetValue<string>("AppSettings:DllLibPath");
            TokenPin = _config.GetValue<string>("AppSettings:TokenPin");
        }
        [Route("SubmitDocument")]
        [HttpPost]
        public DocumentSubmissionDTO Documentsubmissions(SubmitInput paramaters)
        {
            try
            {
                var docs = paramaters.documents.Select(d=> d.ToDocumentWithoutSignatureDTO()).ToList();
                Temp docsFormatted = new Temp() { documents = docs };
                string docsJsonStr = JsonConvert.SerializeObject(docsFormatted);
                var SignedDocuments = SignDocument(TokenPin, docsJsonStr);
                _logger.LogInformation("Info: Here are the submitted documents of date [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")+ "] --> " + SignedDocuments);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paramaters.token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    paramaters.url += "documentsubmissions";
                    client.BaseAddress = new Uri(paramaters.url);
                    var stringContent = new StringContent(SignedDocuments, Encoding.UTF8, "application/json");
                     var postTask = client.PostAsync(paramaters.url, stringContent);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var _res = JsonConvert.DeserializeObject<DocumentSubmissionDTO>(result.Content.ReadAsStringAsync().Result);
                        _logger.LogInformation("Info: Here are the response of submission of date [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "]--> "+ JsonConvert.SerializeObject(_res));
                        return _res;
                    }
                    _logger.LogWarning("Warning: Failed With Status Code ---> ", result.StatusCode);
                    return new DocumentSubmissionDTO() { statusCode = result.StatusCode.ToString() };
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Exception: Failed due to the following exception ---> ", ex.Message.ToString());
                return new DocumentSubmissionDTO() { statusCode = ex.ToString() };
            }
        }
        private string Serialize(JObject document)
        {
            return SerializeToken(JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(document), new JsonSerializerSettings()
            {
                FloatFormatHandling = FloatFormatHandling.String,
                FloatParseHandling = FloatParseHandling.Decimal,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.None
            }));
        }
        private string SerializeToken(JToken request)
        {
            string serialized = "";
            if (request.Parent is null)
            {
                SerializeToken(request.First);
            }
            else
            {
                if (request.Type == JTokenType.Property)
                {
                    string name = ((JProperty)request).Name.ToUpper();
                    serialized += "\"" + name + "\"";
                    foreach (var property in request)
                    {
                        if (property.Type == JTokenType.Object)
                        {
                            serialized += SerializeToken(property);
                        }
                        if (property.Type == JTokenType.Boolean || property.Type == JTokenType.Integer || property.Type == JTokenType.Float || property.Type == JTokenType.Date)
                        {
                            serialized += "\"" + property.Value<string>() + "\"";
                        }
                        if (property.Type == JTokenType.String)
                        {
                            serialized += JsonConvert.ToString(property.Value<string>());
                        }
                        if (property.Type == JTokenType.Array)
                        {
                            foreach (var item in property.Children())
                            {
                                serialized += "\"" + ((JProperty)request).Name.ToUpper() + "\"";
                                serialized += SerializeToken(item);
                            }
                        }
                    }
                }
            }
            if (request.Type == JTokenType.Object)
            {
                foreach (var property in request.Children())
                {

                    if (property.Type == JTokenType.Object || property.Type == JTokenType.Property)
                    {
                        serialized += SerializeToken(property);
                    }
                }
            }

            return serialized;
        }
        private string SignDocument(string pin,string docsJsonStr)
        {
            JObject request = JsonConvert.DeserializeObject<JObject>(docsJsonStr, new JsonSerializerSettings()
            {
                FloatFormatHandling = FloatFormatHandling.String,
                FloatParseHandling = FloatParseHandling.Decimal,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.None
            });
            var documents = request["documents"].ToObject<JArray>();
            JArray inputDocuments = new JArray();

            for (int i = 0; i < documents.Count; i++)
            {
                var document = documents[i].ToObject<JObject>();
                var serializedString = Serialize(document);
                var signatureString = SignWithCMS(Encoding.UTF8.GetBytes(serializedString));
                var signatures = new List<SIGNATURESDTO>();
                signatures.Add(new SIGNATURESDTO
                {
                    signatureType = "I",
                    value = "NA"
                });
                document.Add("signatures", JArray.FromObject(signatures));
                inputDocuments.Add(document);
            }
            request.Remove("documents");
            request.Add("documents", inputDocuments);
            return JsonConvert.SerializeObject(request);
            
        }
        private byte[] HashBytes(byte[] input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var output = sha.ComputeHash(input);
                return output;
            }
        }
        private string SignWithCMS(byte[] data)
        {
            Pkcs11InteropFactories factories = new Pkcs11InteropFactories();
            using (IPkcs11Library pkcs11Library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, DllLibPath, AppType.MultiThreaded))
            {
                ISlot slot = pkcs11Library.GetSlotList(SlotsType.WithTokenPresent).FirstOrDefault();

                if (slot is null)
                {
                    return "No slots found";
                }

                var token = slot.GetTokenInfo();
                var subfi = slot.GetSlotInfo();

                using (var session = slot.OpenSession(SessionType.ReadWrite))
                {

                    session.Login(CKU.CKU_USER, Encoding.UTF8.GetBytes(TokenPin));

                    var searchAttribute = new List<IObjectAttribute>()
                    {
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE),
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true),
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509)
                    };

                    IObjectHandle certificate = session.FindAllObjects(searchAttribute).FirstOrDefault();


                    if (certificate is null)
                    {
                        return "Certificate not found";
                    }

                    var attributeValues = session.GetAttributeValue(certificate, new List<CKA>
                    {
                        CKA.CKA_VALUE
                    });


                    var xcert = new X509Certificate2(attributeValues[0].GetValueAsByteArray());

                    searchAttribute = new List<IObjectAttribute>()
                    {
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY),
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_KEY_TYPE,CKK.CKK_RSA)
                    };

                    IObjectHandle privateKeyHandler = session.FindAllObjects(searchAttribute).FirstOrDefault();

                    RSA privateKey = new TokenRSA(xcert, session, slot, privateKeyHandler);
                  


                    ContentInfo content = new ContentInfo(new Oid("1.2.840.113549.1.7.5"), data);


                    SignedCms cms = new SignedCms(content, true);


                    EssCertIDv2 bouncyCertificate = new EssCertIDv2(new Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier(new DerObjectIdentifier("1.2.840.113549.1.9.16.2.47")), HashBytes(xcert.RawData));

                    SigningCertificateV2 signerCertificateV2 = new SigningCertificateV2(new EssCertIDv2[] { bouncyCertificate });


                    CmsSigner signer = new CmsSigner(xcert);

                    signer.PrivateKey = privateKey;

                    signer.DigestAlgorithm = new Oid("2.16.840.1.101.3.4.2.1");



                    signer.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.UtcNow));
                    signer.SignedAttributes.Add(new AsnEncodedData(new Oid("1.2.840.113549.1.9.16.2.47"), signerCertificateV2.GetEncoded()));

                    cms.ComputeSignature(signer);

                    var output = cms.Encode();

                    return Convert.ToBase64String(output);
                }
            }

        }
    }
}
