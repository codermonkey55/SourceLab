using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.IO;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWSWebSample
{
    public partial class _Default : System.Web.UI.Page
    {
        protected IAmazonEC2 ec2;
        protected IAmazonS3 s3;
        protected IAmazonSimpleDB sdb;
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                try
                {
                    ec2 = new AmazonEC2Client();
                    this.WriteEC2Info();
                }
                catch (AmazonEC2Exception ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon EC2.");
                        sr.WriteLine("<br />");
                        sr.WriteLine("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
                        sr.WriteLine("<br />");
                        sr.WriteLine("<br />");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Request ID: " + ex.RequestId);
                        sr.WriteLine("<br />");
                        sr.WriteLine("<br />");
                    }
                    this.ec2Placeholder.Text = sr.ToString();
                }
            }

            sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                try
                {
                    s3 = new AmazonS3Client();
                    this.WriteS3Info();
                }
                catch (AmazonS3Exception ex)
                {
                    if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                        ex.ErrorCode.Equals("InvalidSecurity")))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon S3");
                        sr.WriteLine("<br />");
                        sr.WriteLine("You can sign up for Amazon S3 at http://aws.amazon.com/s3");
                        sr.WriteLine("<br />");
                        sr.WriteLine("<br />");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Request ID: " + ex.RequestId);
                        sr.WriteLine("<br />");
                        sr.WriteLine("<br />");
                    }
                    this.s3Placeholder.Text = sr.ToString();
                }
            }

            sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                try
                {
                    sdb = new AmazonSimpleDBClient();
                    this.WriteSimpleDBInfo();
                }
                catch (AmazonSimpleDBException ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("InvalidClientTokenId"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon SimpleDB.");
                        sr.WriteLine("<br />");
                        sr.WriteLine("You can sign up for Amazon SimpleDB at http://aws.amazon.com/simpledb");
                        sr.WriteLine("<br />");
                        sr.WriteLine("<br />");
                    }
                    else
                    {
                        sr.WriteLine("Exception Message: " + ex.Message);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("<br />");
                        sr.WriteLine("Request ID: " + ex.RequestId);
                        sr.WriteLine("<br />");
                        sr.WriteLine("<br />");
                    }
                    this.sdbPlaceholder.Text = sr.ToString();
                }
            }
        }

        private void WriteEC2Info()
        {
            StringBuilder output = new StringBuilder();
            DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();
            DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);
            foreach (Reservation reservation in ec2Response.Reservations)
            {
                foreach (Instance instance in reservation.Instances)
                {
                    output.AppendFormat("<li>{0}</li>", instance.InstanceId);
                }
            }
            this.ec2Placeholder.Text = output.ToString();
        }

        private void WriteS3Info()
        {
            StringBuilder output = new StringBuilder();
            ListBucketsResponse response = s3.ListBuckets();
            if (response.Buckets != null && response.Buckets.Count > 0)
                foreach (S3Bucket theBucket in response.Buckets)
                {
                    output.AppendFormat("<li>{0}</li>", theBucket.BucketName);
                }
            this.s3Placeholder.Text = output.ToString();
        }

        private void WriteSimpleDBInfo()
        {
            StringBuilder output = new StringBuilder();
            ListDomainsRequest sdbRequest = new ListDomainsRequest();
            ListDomainsResponse sdbResponse = sdb.ListDomains(sdbRequest);
            foreach (string domain in sdbResponse.DomainNames)
            {
                output.AppendFormat("<li>{0}</li>", domain);
            }
            this.sdbPlaceholder.Text = output.ToString();
        }
    }
}