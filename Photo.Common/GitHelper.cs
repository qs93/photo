using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gif.Components;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Photo.Common
{
    public class GitHelper
    {

        private static int delay;

        /// <summary>
        /// 合併gif
        /// </summary>
        public static void GetThumbnail(string imgName, string type, Image addImg)
        {
            string re = ""; // 用于保存操作结果的变量
            try
            {
                string outPutPath = HttpContext.Current.Server.MapPath(string.Format("~/timg/{0}.gif", imgName)); // 生成保存圖片的名稱
                AnimatedGifEncoder animate = new AnimatedGifEncoder();
                animate.Start(outPutPath);
                animate.SetRepeat(0); // -1：不重複，0：循環
                List<Image> imgList = GetFrames(HttpContext.Current.Server.MapPath(string.Format("~/timg/demo/{0}-words.gif", type)));
                var imgCount = imgList.Count;
                animate.SetDelay(delay);  //設置時間
                for (int i = 0; i < imgCount; i++)
                {
                    if ((i + 1) == imgCount)
                    {
                        animate.SetDelay(3000);
                        animate.AddFrame(addImg);
                        //addImg.Save(HttpContext.Current.Server.MapPath("../timg/p" + i + ".jpg"));
                    }
                    else
                    {
                        animate.AddFrame(imgList[i]); // 添加帧
                                                      //imgList[i].Save(HttpContext.Current.Server.MapPath("../timg/p" + i + ".jpg"));
                    }

                }
                animate.Finish();
            }
            catch (Exception ex)
            {
                re = ex.Message;
            }
        }

        //解码gif图片
        public static List<Image> GetFrames(string pPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]); //獲取帧數
            int count = gif.GetFrameCount(fd);
            List<Image> gifList = new List<Image>();  //保存帧
            for (int i = 0; i < count; i++)
            {
                gif.SelectActiveFrame(fd, i);
                if (i == 0)
                {
                    for (int j = 0; j < gif.PropertyIdList.Length; j++)//遍歷屬性
                    {
                        if ((int)gif.PropertyIdList.GetValue(j) == 0x5100)//.如果是延時時間
                        {
                            PropertyItem pItem = (PropertyItem)gif.PropertyItems.GetValue(j);//獲取延時時間屬性
                            byte[] delayByte = new byte[4];//延時時間
                            delayByte[0] = pItem.Value[i * 4];
                            delayByte[1] = pItem.Value[1 + i * 4];
                            delayByte[2] = pItem.Value[2 + i * 4];
                            delayByte[3] = pItem.Value[3 + i * 4];
                            delay = BitConverter.ToInt32(delayByte, 0) * 10; //*10，獲取毫秒
                            break;
                        }
                    }
                }
                gifList.Add(new Bitmap(gif));
            }
            gif.Dispose();
            return gifList;
        }

        public static Image Thumbnail(Image originalImage)
        {
            #region 绘制缩略图
            int thumbnailWidth = 400;
            int thumbnailHeight = 400;
            int originalWidth = 400;
            int originalHeight = 400;
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(thumbnailWidth, thumbnailHeight);//新建一个bmp图片
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);//新建一个画板
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;//设置高质量插值法
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            graphic.Clear(System.Drawing.Color.Transparent);//清空画布並以透明背景色填充
            var thumbnailRectangle = new System.Drawing.Rectangle(0, 0, thumbnailWidth, thumbnailHeight);
            var originalRectangle = new System.Drawing.Rectangle(0, 0, originalWidth, originalHeight);
            graphic.DrawImage(originalImage, thumbnailRectangle, originalRectangle, System.Drawing.GraphicsUnit.Pixel);//在指定位置並且按指定大小绘制原图片的指定部分
            #endregion

            return bitmap;
        }

        public static Image MakeThumbnail(Image originalImage, int width, int height)
        {
            //缩略图画布宽高 
            int towidth = width;
            int toheight = height;
            //原始图片写入画布坐标和宽高(用来设置裁减溢出部分) 
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            //原始图片画布,设置写入缩略图画布坐标和宽高(用来原始图片整体宽高缩放) 
            int bg_x = 0;
            int bg_y = 0;
            int bg_w = towidth;
            int bg_h = toheight;
            //倍数变量 
            double multiple = 0;
            //获取宽长的或是高长与缩略图的倍数 
            if (originalImage.Width >= originalImage.Height) multiple = (double)originalImage.Width / (double)width;
            else multiple = (double)originalImage.Height / (double)height;
            //上传的图片的宽和高小等于缩略图 
            if (ow <= width && oh <= height)
            {
                //缩略图按原始宽高 
                bg_w = originalImage.Width;
                bg_h = originalImage.Height;
                //空白部分用背景色填充 
                bg_x = Convert.ToInt32(((double)towidth - (double)ow) / 2);
                bg_y = Convert.ToInt32(((double)toheight - (double)oh) / 2);
            }
            //上传的图片的宽和高大于缩略图 
            else
            {
                //宽高按比例缩放 
                bg_w = Convert.ToInt32((double)originalImage.Width / multiple);
                bg_h = Convert.ToInt32((double)originalImage.Height / multiple);
                //空白部分用背景色填充 
                bg_y = Convert.ToInt32(((double)height - (double)bg_h) / 2);
                bg_x = Convert.ToInt32(((double)width - (double)bg_w) / 2);
            }
            //新建一个bmp图片,并设置缩略图大小. 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板 
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并设置背景色 
            //g.Clear(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            //在指定位置并且按指定大小绘制原图片的指定部分 
            //第一个System.Drawing.Rectangle是原图片的画布坐标和宽高,第二个是原图片写在画布上的坐标和宽高,最后一个参数是指定数值单位为像素 
            g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, bg_w, bg_h), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

            return bitmap;
        }
    }
}
