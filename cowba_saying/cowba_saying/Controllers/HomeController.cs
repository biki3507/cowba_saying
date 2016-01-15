using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cowba_saying.Models;
using System.Web.Hosting;
using System.Drawing.Text;
using System.IO;

namespace cowba_saying.Controllers
{
    public class HomeController : Controller
    {
        static PrivateFontCollection myOwnFonts =
        new PrivateFontCollection();

        static HomeController()
    {
        //截入預先準備好的字型檔案
        myOwnFonts.AddFontFile(
            HostingEnvironment.MapPath("~/TpldKhangXiDictTrial.otf"));
    }
        public ActionResult Index(SayingViewModel data)
        {
            
            if (data.file != null)
            {

               

                if (data.Name != "" || data.Saying != "")
                {
                    string pic = System.IO.Path.GetFileName(data.file.FileName);
                    string path = System.IO.Path.Combine(
                                           HostingEnvironment.MapPath("~/image/upload"), pic);
                    // file is uploaded
                    data.file.SaveAs(path);

                    var p = CombineBitmap(new string[] { pic });
                    p.Save(Server.MapPath("~/image/result/") + @"result.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    WriteSomething(data.Saying, data.Name, "result.jpg");
                    return View(data);
                }
            }
            //var p = CombineBitmap(new string[] { "demo.jpg", "demo02.png" });
            //p.Save(@"C:\Users\David.AD1\Desktop\cowba_saying\cowba_saying\cowba_saying\result.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            if (data.Name==""||data.Saying=="")
            {
                return View(data);
            }
            var model = new SayingViewModel(); 
           return View(model);
        }
       
        public ActionResult Index2(SayingViewModel data)
        {
            
            return View();
        }

       

        public static System.Drawing.Bitmap CombineBitmap(string[] files)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;


                //create a Bitmap from the file and add it to the list
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(HostingEnvironment.MapPath("~")+@"\image\upload\"+files[0], true);

                //update the size of the final bitmap
                width += bitmap.Width;
                height = bitmap.Height > height ? bitmap.Height : height;

                images.Add(bitmap);
                System.Drawing.Bitmap bitmap2 = new System.Drawing.Bitmap(HostingEnvironment.MapPath("~") + @"\image\base01.jpg", true);

                //update the size of the final bitmap
                width += bitmap2.Width;
                height = bitmap2.Height > height ? bitmap2.Height : height;

                images.Add(bitmap2);


                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
        public static void WriteSomething(string saying,string people,string file)
        {
            string firstText = saying;
            string secondText = @" - " + people;

            PointF firstLocation = new PointF(250, 180);
            PointF secondLocation = new PointF(550, 250);
            string imageFilePath =HostingEnvironment.MapPath("~/image/result/")+file;
            Bitmap bitmap = (Bitmap)Image.FromFile(imageFilePath);//load the image file

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (Font arialFont = new Font(myOwnFonts.Families[0], 30))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }
                using (Font arialFont = new Font(myOwnFonts.Families[0], 20))
                {
                    graphics.DrawString(secondText, arialFont, Brushes.White, secondLocation);
                }
            }
            var ppath = HostingEnvironment.MapPath("~/image/result") + @"\result2.jpg";
            bitmap.Save(ppath);//save the image file
        }
    }
}