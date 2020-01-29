using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GittedGoProDump.DataStuff;
using Newtonsoft.Json;
using RestProConnect.DataJson;
using RestSharp;
using RestSharp.Extensions;

namespace GittedGoProDump
{
    public class RestClientManager
    {

        //string PathLocal = @"F:\MeshRoom Projects\Office_1.29.2020auto\Pictures_office\";
        string PathLocal = @"F:\MeshRoom Projects\Office_1.29.2020auto\Pictures_office_test\";
        RestClient Client_MediaData;
       // RestClient Client_File;
        GpMediaData Cur_mediadata;

      
         void Request_GpMedia() {
            Client_MediaData = new RestClient("http://10.5.5.9/gp/gpMediaList");
            RestRequest rr = new RestRequest(Method.GET);
            rr.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var query = Client_MediaData.Execute<object>(rr).Data;

            Cur_mediadata = GpMediaData.FromJson(JsonConvert.SerializeObject(query));
        }

        
        public void Request_File(string argGoProFileName)
        {
            if (argGoProFileName == null || argGoProFileName == "") {
                Console.WriteLine("invalid gopro Filename");
                return;
            }

            string RestUL = "http://10.5.5.9:8080/videos/DCIM/100GOPRO/";
            string FullRestPath = RestUL + argGoProFileName;
            string FullPathOnPc = PathLocal + argGoProFileName;

            Client_MediaData = new RestClient(FullRestPath);
            RestRequest rr = new RestRequest(Method.GET);
            byte[] Picture= Client_MediaData.DownloadData(rr);
            File.WriteAllBytes( FullPathOnPc,Picture);
            Console.WriteLine(argGoProFileName + " saved to " + PathLocal);
        }

        //[Obsolete]
        //public void DownloadPicture(string argName)
        //{
        //    if (argName[0] == 'x')
        //    {
        //        Console.WriteLine("nothing to save");
        //        return;
        //    }
        //    string RestUL = "http://10.5.5.9:8080/videos/DCIM/100GOPRO/";
        //    string FullRestPath = RestUL + argName;
        //    Client_MediaData = new RestClient(FullRestPath);
        //    var request = new RestRequest("#", Method.GET);
        //    string localSavepathFull = PathLocal + argName;
        //    Client_MediaData.DownloadData(request).SaveAs(localSavepathFull);
        //    Console.WriteLine("finished");
        //}



        //[Obsolete]
        public void RunQuerySync()
        {

            if (IsCamReady()) {
                Console.WriteLine("sorry, busy");
            }
            else
            {


                Request_GpMedia();
                string NextPic = FindNextToDownload();
                Console.WriteLine("NextPic on Gp is " + NextPic);

                if (string.Compare(NextPic, "0") == 0)
                {
                    Console.WriteLine("GoPro is Synced");
                }
                else
                {
                    Console.WriteLine("Dwonloading... " + NextPic);
                    Request_File(NextPic);
                    Console.WriteLine("Finished! " + NextPic);
                    

                }
            }
        }


        //find latest pic downloaded
          string Latest_Downloaded()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(PathLocal);
            FileInfo latestFile = dirInfo.GetFiles().OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
            if (latestFile != null)
            {
                Console.WriteLine("Latest Downloaded = " + latestFile.Name);
                return latestFile.Name;
            }
            return "0"; //check for 0 in first char 
        }
        //find latest pic snapped , and its index
          string FindLatest_snapped() {
            if (Cur_mediadata != null)
                return Cur_mediadata.LastSnapped();
            else
                return "0";
             }
        //if not same get the next snaped
          string FindNextToDownload() {

            string LastLocal = Latest_Downloaded(); // "GOPR4365.JPG";
            string LastSnapped = FindLatest_snapped();// "GOPR4360.JPG";
            Console.WriteLine("**************** "+LastLocal);


            if (LastSnapped == "0" || LastSnapped == "") {
                return "0";
            }
            if (LastLocal == "0") {
                return Cur_mediadata.FirsttSnapped();
            }

            if (string.Compare(LastSnapped, LastLocal) == 0)
            {
                return "0";
            }

            return Cur_mediadata.Get_NextPicAfter(LastLocal);
        }




        public void RequestStatusJSON()
        {

            var client = new RestClient("http://10.5.5.9/gp/gpControl/status");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            var queryResult = client.Execute<Object>(request).Data;
            string json = JsonConvert.SerializeObject(queryResult);
            System.IO.File.WriteAllText(@"E:\Destination_pretend_Live\test.json", json);
        }
        public bool IsCamReady()
        {
            Client_MediaData = new RestClient("http://10.5.5.9/gp/gpControl/status");
    
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            var queryResult = Client_MediaData.Execute<Object>(request).Data;
            string json = JsonConvert.SerializeObject(queryResult);

            GpStatus gpStatus = GpStatus.FromJson(json);
            long? x = gpStatus.Status["8"].Integer;
            Console.WriteLine(x.ToString());
            if (x != null && x != 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
