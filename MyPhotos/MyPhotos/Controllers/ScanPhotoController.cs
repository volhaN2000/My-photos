﻿using Microsoft.AspNetCore.Mvc;
using MyPhotos.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
using System.Text;

namespace MyPhotos.Controllers
{

    public class ScanPhotoController : Controller
    {
        private readonly ILogger<ScanPhotoController> _logger;
        public ScanPhotoController(ILogger<ScanPhotoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult ScanPhoto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ScanPhoto(IFormFile userfile)
        {
            try
            {
                const string SUBSCRIPTION_KEY = "dfa5543e585e429fafb3ffd14b02d197";
                const string ENDPOINT = "https://facial-recognition-tms.cognitiveservices.azure.com/";
                static ComputerVisionClient Authenticate(string endpoint, string key)
                {
                    ComputerVisionClient client =
                      new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                      { Endpoint = endpoint };
                    return client;
                }
                ComputerVisionClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);

                string filename = userfile.FileName;

                filename = Path.GetFileName(filename);

                bool directory_exists = Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Scan\\", User.Identity.Name));
                if (!directory_exists)
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Scan\\", User.Identity.Name));
                    filename = "image0.jpg";
                }
                else
                {
                    var directory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Scan\\", User.Identity.Name));
                    FileInfo[] files = directory.GetFiles();
                    int i = 0;
                    foreach (FileInfo file in files)
                    { i++; }
                    filename = "image" + i + ".jpg";
                }


                string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Scan\\", User.Identity.Name, filename);
                var stream = new FileStream(uploadfilepath, FileMode.Create);
                userfile.CopyTo(stream);
                ViewBag.message = "File uploaded successfully";
                stream.Close();

                string READ_TEXT_IMAGE = uploadfilepath;

                // Extract text (OCR) from a URL image using the Read API
                ReadFile(client, READ_TEXT_IMAGE).Wait();
                async Task ReadFile(ComputerVisionClient client, string fileName)
                {


                    List<string> imageFileNames = new List<string>
                    {
                     Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Scan\\", User.Identity.Name, filename)
                };

                    foreach (var imageFileName in imageFileNames)
                    {
                        using var stream = new FileStream(imageFileName, FileMode.Open);

                        // Read text from URL
                        var textHeaders = await client.ReadInStreamAsync(stream);
                        // After the request, get the operation location (operation ID)
                        string operationLocation = textHeaders.OperationLocation;
                        Thread.Sleep(2000);

                        // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
                        // We only need the ID and not the full URL
                        const int numberOfCharsInOperationId = 36;
                        string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                        // Extract the text
                        ReadOperationResult results;
                        do
                        {
                            results = await client.GetReadResultAsync(Guid.Parse(operationId));

                        }
                        while ((results.Status == OperationStatusCodes.Running ||
                            results.Status == OperationStatusCodes.NotStarted));

                        // Display the found text.
                        StringBuilder scanResults = new StringBuilder("");
                        var textUrlFileResults = results.AnalyzeResult.ReadResults;
                        foreach (ReadResult page in textUrlFileResults)
                        {
                            foreach (Line line in page.Lines)
                            {
                                scanResults.Append(line.Text);
                            }
                        }
                        ViewBag.scanResults = scanResults;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}