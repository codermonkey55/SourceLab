using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO.Compression;
using Amazon.S3.Transfer;

namespace InMemory_ZipArchiving_S3
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write(GetServiceOutput());
            Console.Read();
        }

        public static string GetServiceOutput()
        {
            StringBuilder sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                sr.WriteLine("===========================================");
                sr.WriteLine("Welcome to the AWS .NET SDK!");
                sr.WriteLine("===========================================");


                using (var zipMemoryArchive = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipMemoryArchive, ZipArchiveMode.Create, true))
                    {
                        ZipArchiveEntry encounterJsonEntry = archive.CreateEntry("EncounterJson.json");
                        using (var encounterJsonStream = encounterJsonEntry.Open())
                        {
                            var randomBytes = new byte[512000];
                            new Random().NextBytes(randomBytes);
                            encounterJsonStream.Write(randomBytes, 0, randomBytes.Length);
                        }

                        ZipArchiveEntry encounterImagesEntry = archive.CreateEntry("Images/EncounterImages.jpg");
                        using (var encounterImagesStream = encounterImagesEntry.Open())
                        {
                            var randomBytes = new byte[512000];
                            new Random().NextBytes(randomBytes);
                            encounterImagesStream.Write(randomBytes, 0, randomBytes.Length);
                        }
                    }

                    //using (var fs = new FileStream("./ZipArchive.zip", FileMode.Create, FileAccess.Write, FileShare.None))
                    //{
                    //    zipMemoryArchive.Seek(0, SeekOrigin.Begin);
                    //    zipMemoryArchive.CopyTo(fs);
                    //}

                    // Print the number of Amazon S3 Buckets.
                    IAmazonS3 s3Client = new AmazonS3Client();
                    ITransferUtility transferAgentUtility = new TransferUtility(s3Client);

                    try
                    {
                        ListBucketsResponse response = s3Client.ListBuckets();
                        int numBuckets = 0;
                        if (response.Buckets != null &&
                            response.Buckets.Count > 0)
                        {
                            numBuckets = response.Buckets.Count;
                        }
                        sr.WriteLine("You have " + numBuckets + " Amazon S3 bucket(s).");
                        sr.WriteLine("");
                        sr.WriteLine("=================== Zip Archiving ========================");

                        //s3Client.PutObject(new PutObjectRequest
                        //{
                        //    BucketName = "zip-archive-store",
                        //    Key = "Encounters/Code2Archive.zip",
                        //    InputStream = zipMemoryArchive
                        //});
                        //s3Client.Dispose();

                        transferAgentUtility.Upload(zipMemoryArchive, "zip-archive-store", "Encounters/Code2Archive.zip");
                        transferAgentUtility.Dispose();

                        sr.WriteLine("");
                        sr.WriteLine("=================== File Archived ========================");
                    }
                    catch (AmazonS3Exception ex)
                    {
                        if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                            ex.ErrorCode.Equals("InvalidSecurity")))
                        {
                            sr.WriteLine("Please check the provided AWS Credentials.");
                            sr.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                        }
                        else
                        {
                            sr.WriteLine("Caught Exception: " + ex.Message);
                            sr.WriteLine("Response Status Code: " + ex.StatusCode);
                            sr.WriteLine("Error Code: " + ex.ErrorCode);
                            sr.WriteLine("Request ID: " + ex.RequestId);
                        }
                    }
                }
                sr.WriteLine("");
                sr.WriteLine("Press any key to continue...");
            }
            return sb.ToString();
        }
    }
}