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

// See https://aka.ms/new-console-template for more information
const string SUBSCRIPTION_KEY = "dfa5543e585e429fafb3ffd14b02d197";
const string ENDPOINT = "https://facial-recognition-tms.cognitiveservices.azure.com/";

try
{
    const string READ_TEXT_URL_IMAGE = @"C:\\Users\\Admin\\source\\repos\\MyPhotos\\MyPhotos\\wwwroot\\Files\\Scan\\ivansidorov @gmail.com\\image3.jpg ";


    Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
    Console.WriteLine();

    ComputerVisionClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);

    // Extract text (OCR) from a URL image using the Read API
    ReadFile(client, READ_TEXT_URL_IMAGE).Wait();

    static ComputerVisionClient Authenticate(string endpoint, string key)
    {
        ComputerVisionClient client =
          new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
          { Endpoint = endpoint };
        return client;
    }

    //static async Task ReadFileUrl(ComputerVisionClient client, string urlFile)
    //{
    //    Console.WriteLine("----------------------------------------------------------");
    //    Console.WriteLine("READ FILE FROM URL");
    //    Console.WriteLine();

    //    // Read text from URL
    //    var textHeaders = await client.ReadAsync(urlFile);
    //    // After the request, get the operation location (operation ID)
    //    string operationLocation = textHeaders.OperationLocation;
    //    Thread.Sleep(2000);

    //    // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
    //    // We only need the ID and not the full URL
    //    const int numberOfCharsInOperationId = 36;
    //    string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

    //    // Extract the text
    //    ReadOperationResult results;
    //    Console.WriteLine($"Extracting text from URL file {Path.GetFileName(urlFile)}...");
    //    Console.WriteLine();
    //    do
    //    {
    //        results = await client.GetReadResultAsync(Guid.Parse(operationId));
    //    }
    //    while ((results.Status == OperationStatusCodes.Running ||
    //        results.Status == OperationStatusCodes.NotStarted));

    //    // Display the found text.
    //    Console.WriteLine();
    //    var textUrlFileResults = results.AnalyzeResult.ReadResults;
    //    foreach (ReadResult page in textUrlFileResults)
    //    {
    //        foreach (Line line in page.Lines)
    //        {
    //            Console.WriteLine(line.Text);
    //        }
    //    }
    //    Console.WriteLine();
    //}
    static async Task ReadFile(ComputerVisionClient client, string fileName)
    {
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine("READ FILE FROM URL");
        Console.WriteLine();

        List<string> imageFileNames = new List<string>
                    {
                       @"C:\\Users\\Admin\\source\\repos\\MyPhotos\\MyPhotos\\wwwroot\\Files\\Scan\\ivansidorov@gmail.com\\image3.jpg "
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
            Console.WriteLine($"Extracting text from URL file {Path.GetFileName(fileName)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));

            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }

    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);

}
Console.ReadLine();
