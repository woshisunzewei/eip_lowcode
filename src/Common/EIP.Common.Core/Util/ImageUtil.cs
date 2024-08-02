using EIP.Common.Core.Extension;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace EIP.Common.Core.Util
{
    public static class ImageUtil
    {
        /// <summary>
        ///  base64 转成图片
        /// </summary>
        /// <param name="image">图片的base64形式</param>
        /// <param name="path">项目区分</param>
        public static void SaveBase64Image(string image, string path)
        {
            string filepath = Path.GetDirectoryName(path);
            // 如果不存在就创建file文件夹
            if (!Directory.Exists(filepath))
            {
                if (filepath != null) Directory.CreateDirectory(filepath);
            }
            image = image.Substring(image.IndexOf(",") + 1);
            var photoBytes = Convert.FromBase64String(image);
            System.IO.File.WriteAllBytes(path, photoBytes);
        }

        /// <summary>
        /// 合并图片 小图片放在大图片上面
        /// </summary>
        /// <param name="templeBase64Str">模板大图片base64</param>
        /// <param name="outputBase64Str">模板小图片base64</param>
        /// <returns></returns>
        public static string MergeImage(string templeBase64Str, string outputBase64Str, int x, int y, string sn)
        {
            try
            {
                byte[] templebytes = Convert.FromBase64String(templeBase64Str);
                byte[] outputbytes = Convert.FromBase64String(outputBase64Str);
                var imagesTemle = Image.Load(templebytes);
                var outputImg = Image.Load(outputbytes);
                FontCollection fonts = new FontCollection();
                SixLabors.Fonts.FontFamily fontfamily = fonts.Add(AppDomain.CurrentDomain.BaseDirectory + "/font/msyh.ttf"); //字体的路径
                var fontName = new SixLabors.Fonts.Font(fontfamily, 30);
                var font = new SixLabors.Fonts.Font(fontfamily, 30);
                //进行多图片处理
                imagesTemle.Mutate(a =>
                {
                    a.DrawImage(outputImg, new Point(x, y), 1);
                    if (sn.IsNotNullOrEmpty())
                        a.DrawText("NO." + sn, font, new Color(Rgba32.ParseHex("#00000000")), new PointF(x + 35, y + 210));
                    //还是合并 
                });
                var strRet = imagesTemle.ToBase64String(PngFormat.Instance);
                return strRet;
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        /// <summary>
        /// 合并图片 小图片放在大图片上面
        /// </summary>
        /// <param name="templeBase64Str">模板大图片base64</param>
        /// <param name="outputBase64Str">模板小图片base64</param>
        /// <returns></returns>
        public static string MergeImagePersonalQRCode(string templeBase64Str, string outputBase64Str, int x, int y, string sn)
        {
            try
            {
                byte[] templebytes = Convert.FromBase64String(templeBase64Str);
                byte[] outputbytes = Convert.FromBase64String(outputBase64Str);
                var imagesTemle = Image.Load(templebytes);
                var outputImg = Image.Load(outputbytes);
                FontCollection fonts = new FontCollection();
                SixLabors.Fonts.FontFamily fontfamily = fonts.Add(AppDomain.CurrentDomain.BaseDirectory + "/font/msyh.ttf"); //字体的路径
                var fontName = new SixLabors.Fonts.Font(fontfamily, 30);
                var font = new SixLabors.Fonts.Font(fontfamily, 30);
                //进行多图片处理
                imagesTemle.Mutate(a =>
                {
                    a.DrawImage(outputImg, new Point(x, y), 1);
                    if (sn.IsNotNullOrEmpty())
                        a.DrawText("NO." + sn, font, new Color(Rgba32.ParseHex("#00000000")), new PointF(x + 102, y + 366));
                    //还是合并 
                });
                var strRet = imagesTemle.ToBase64String(PngFormat.Instance);
                return strRet;
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        /// <summary>
        /// 合并图片 小图片放在大图片上面
        /// </summary>
        /// <param name="templeBase64Str">模板大图片base64</param>
        /// <param name="outputBase64Str">模板小图片base64</param>
        /// <returns></returns>
        public static string MergeImagePersonalQRCodeBrand(string templeBase64Str, string outputBase64Str, int x, int y, string sn)
        {
            try
            {
                byte[] templebytes = Convert.FromBase64String(templeBase64Str);
                byte[] outputbytes = Convert.FromBase64String(outputBase64Str);
                var imagesTemle = Image.Load(templebytes);
                var outputImg = Image.Load(outputbytes);
                FontCollection fonts = new FontCollection();
                SixLabors.Fonts.FontFamily fontfamily = fonts.Add(AppDomain.CurrentDomain.BaseDirectory + "/font/msyh.ttf"); //字体的路径
                var fontName = new SixLabors.Fonts.Font(fontfamily, 30);
                var font = new SixLabors.Fonts.Font(fontfamily, 30);
                //进行多图片处理
                imagesTemle.Mutate(a =>
                {
                    a.DrawImage(outputImg, new Point(x, y), 1);
                    if (sn.IsNotNullOrEmpty())
                        a.DrawText("NO." + sn, font, new Color(Rgba32.ParseHex("#00000000")), new PointF(x + 122, y + 410));
                    //还是合并 
                });
                var strRet = imagesTemle.ToBase64String(PngFormat.Instance);
                return strRet;
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        #region 随机头像
        private static List<SKColor> colors = new List<SKColor>()
        {
            new SKColor(205,104,0),
            new SKColor(151,155,22),
            new SKColor(108,163,44),
            new SKColor(121,85,72),
            new SKColor(96,125,139),
            new SKColor(76,175,80),
            new SKColor(0,150,136),
            new SKColor(4,167,187),
            new SKColor(33,150,243),
            new SKColor(63,81,181)
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageText"></param>
        /// <returns></returns>
        public static string ResolveName(string imageText)
        {
            imageText.Replace("?", "").Replace(":", "").Replace("?", "").Replace("*", "").Replace("<", "").Replace(">", "").Replace(@"/", "").Replace(@"\", "").Replace(@"|", "").Replace("\"", "");//去除路径不支持的信息
            imageText = imageText.Trim(' ');//去除空格信息
            string temp2 = imageText.Substring(0, 1);//根据第一位的数据判断是走英文规则还是中文规则，都不是的话就是取前两位
            if (temp2.IsChineseCharacter())
            {
                //UserName = UserName.Trim(' ');
                if (imageText.Length > 2 & imageText.Length <= 3)
                {
                    imageText = imageText.Substring(1, 2);
                }
                else if (imageText.Length >= 3)
                {
                    imageText = imageText.Substring(imageText.Length - 2, 2);
                }
            }
            else if (temp2.IsEnglishCharacter())
            {
                string[] temp1 = imageText.Split(' ');
                if (temp1.Length == 2)
                {
                    imageText = (temp1[0].Substring(0, 1) + temp1[1].Substring(0, 1)).ToUpper();
                }
                else
                {
                    if (imageText.Length > 2)
                    {
                        imageText = imageText.Substring(0, 2).ToUpper();
                    }
                }
            }
            else
            {
                if (imageText.Length > 2)
                {
                    imageText = imageText.Substring(0, 2);
                }
            }
            return imageText;
        }

        /// <summary>
        /// 生成图片文字
        /// </summary>
        /// <param name="imageText">文字</param>
        /// <param name="basePath">生成图片基本路径</param>
        /// <returns></returns>
        public static string CreateImageText(string imageText, string path)
        {
            var imageName = ResolveName(imageText);
            SKBitmap bmp = new SKBitmap(128, 128);
            string str = imageName;
            using (SKCanvas canvas = new SKCanvas(bmp))
            {
                Random r = new Random();
                int num = r.Next(0, 9);
                canvas.DrawColor(colors[num]); // colors是图片背景颜色集合，这里代码就不贴出来了，随机找一个
                using (SKPaint sKPaint = new SKPaint())
                {
                    sKPaint.Color = SKColors.White;//字体颜色
                    sKPaint.TextSize = 39;//字体大小
                    sKPaint.IsAntialias = true;//开启抗锯齿
                    sKPaint.Typeface = SkiaSharp.SKTypeface.FromFamilyName("微软雅黑", SKTypefaceStyle.Normal);//字体
                    SKRect size = new SKRect();
                    sKPaint.MeasureText(str, ref size);//计算文字宽度以及高度
                    float temp = (128 - size.Size.Width) / 2;
                    float temp1 = (128 - size.Size.Height) / 2;
                    canvas.DrawText(str, temp, temp1 - size.Top, sKPaint);//画文字
                }
                //保存成图片文件
                using (SKImage img = SKImage.FromBitmap(bmp))
                {
                    using (SKData p = img.Encode(SKEncodedImageFormat.Jpeg, 100))
                    {
                        using (var stream = File.Create(path))
                        {
                            stream.Write(p.ToArray(), 0, p.ToArray().Length);
                            stream.Dispose();
                            stream.Close();
                        }
                    }
                }
            }
            return path;
        }

        #endregion
    }
}